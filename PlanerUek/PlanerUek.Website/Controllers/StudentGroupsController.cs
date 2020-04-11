using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using PlanerUek.Storage.Interfaces;
using PlanerUek.Website.Configuration;
using PlanerUek.Website.Models;
using PlanerUek.Website.Services;

namespace PlanerUek.Website.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentGroupsController : ControllerBase
    {
        private readonly IStudentGroupsRepository _studentGroupsRepository;
        private readonly IStudentGroupScheduleProvider _scheduleProvider;
        private readonly IGoogleCalendar _googleCalendar;
        private readonly IDataStore _dataStore;
        private readonly IPlanerConfig _planerConfig;

        public StudentGroupsController(IStudentGroupsRepository studentGroupsRepository,
            IStudentGroupScheduleProvider scheduleProvider, IGoogleCalendar googleCalendar, IDataStore dataStore, IPlanerConfig planerConfig)
        {
            _studentGroupsRepository = studentGroupsRepository;
            _scheduleProvider = scheduleProvider;
            _googleCalendar = googleCalendar;
            _dataStore = dataStore;
            _planerConfig = planerConfig;
        }

        [HttpPost(nameof(HandleTimetableForClient))]
        public async Task<IActionResult> HandleTimetableForClient(AddScheduleToCalendarRequest clientRequest)
        {
            var groupId = HttpContext.Session.GetString("groupId") ?? await _studentGroupsRepository.GetGroupId(clientRequest.GroupName);
            if (string.IsNullOrEmpty(groupId))
            {
                return Problem("Id for provided group not found.");
            }
            
            HttpContext.Session.SetString("groupId", groupId);
            var userId = clientRequest.UserId ?? HttpContext.Session.GetString("userId");
            var authResult = await AuthorizeGoogleUser(userId);
            if (authResult.RedirectUri != null)
            {
                var redirectResult = new CalendarUpdateResult("Authentication required.")
                {
                    AuthorizationEndpoint = authResult.RedirectUri
                };
                return Ok(redirectResult);
            }

            var schedule = _scheduleProvider.GetCurrentSchedule(groupId);
            var calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = authResult.Credential,
                ApplicationName = _planerConfig.GetApplicationName()
            });
            var result = await _googleCalendar.AddStudentGroupSchedule(calendarService, schedule);
            
            if (!result.IsSuccess)
            {
                return Problem(result.ErrorMessage);
            }
            
            return Ok(result);
        }

        private async Task<AuthorizationCodeWebApp.AuthResult> AuthorizeGoogleUser(string userId)
        {
            var uri = Request.GetEncodedUrl();
            var redirectUri = _planerConfig.GetRedirectUriForGoogleAuth();
            var result =
               await new AuthorizationCodeWebApp(GetGoogleAuthFlow(), redirectUri, uri)
                    .AuthorizeAsync(userId, CancellationToken.None);
            HttpContext.Session.SetString("userId", userId);
            return result;
        }

        [HttpGet(nameof(GoogleResponse))]
        public async Task<IActionResult> GoogleResponse(string code)
        {
            var userId = HttpContext.Session.GetString("userId");
            var redirectUri = _planerConfig.GetRedirectUriForGoogleAuth();
            await GetGoogleAuthFlow().ExchangeCodeForTokenAsync(userId, code, redirectUri, CancellationToken.None);

            return Redirect($"/?refresh=true");
        }

        private GoogleAuthorizationCodeFlow GetGoogleAuthFlow()
        {
            var clientSecrets = new ClientSecrets()
            {
                ClientId = _planerConfig.GetGoogleClientId(),
                ClientSecret = _planerConfig.GetGoogleClientSecret()
            };
            var googleFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = new[] {CalendarService.Scope.Calendar},
                DataStore = _dataStore
            });
            return googleFlow;
        }
    }
}