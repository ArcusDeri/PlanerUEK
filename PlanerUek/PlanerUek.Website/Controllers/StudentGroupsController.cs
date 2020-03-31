using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlanerUek.Storage.Interfaces;
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

        public StudentGroupsController(IStudentGroupsRepository studentGroupsRepository,
            IStudentGroupScheduleProvider scheduleProvider, IGoogleCalendar googleCalendar)
        {
            _studentGroupsRepository = studentGroupsRepository;
            _scheduleProvider = scheduleProvider;
            _googleCalendar = googleCalendar;
        }

        [HttpPost(nameof(HandleTimetableForGroup))]
        public async Task<IActionResult> HandleTimetableForGroup([FromForm] string groupName)
        {
            var groupId = await _studentGroupsRepository.GetGroupId(groupName);
            if (string.IsNullOrEmpty(groupName))
            {
                return Ok();
            }
            var schedule = _scheduleProvider.GetCurrentSchedule(groupId);
            _googleCalendar.AddStudentGroupSchedule(schedule);

            return Ok();
        }
        
        [HttpGet(nameof(GoogleResponse))]
        public async Task<IActionResult> GoogleResponse(string state, string code)
        {
            var xd = state;
            return Ok();
        }
    }
}