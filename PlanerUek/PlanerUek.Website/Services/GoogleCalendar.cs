﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using PlanerUek.Storage.Models;
using PlanerUek.Website.Models;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using PlanerUek.Website.Configuration;
using PlanerUek.Website.Exceptions;

namespace PlanerUek.Website.Services
{
    public class GoogleCalendar : IGoogleCalendar
    {
        private readonly IPlanerConfig _planerConfig;
        private readonly IDataStore _dataStore;

        public GoogleCalendar(IPlanerConfig planerConfig, IDataStore dataStore)
        {
            _planerConfig = planerConfig;
            _dataStore = dataStore;
        }

        public async Task<CalendarUpdateResult> AddStudentGroupSchedule(StudentGroupSchedule schedule)
        {
            var calendarEvents = ResolveEventsFromSchedule(schedule);
            try
            {
                await AddEventsToCalendar(calendarEvents);
                return new CalendarUpdateResult(true);
            }
            catch (GoogleAuthorizationException)
            {
                return new CalendarUpdateResult("Google authorization failed.");
            }
            catch (Exception e)
            {
                return new CalendarUpdateResult(e.Message);
            }
        }

        private async Task AddEventsToCalendar(IEnumerable<Event> calendarEvents)
        {
            var calendarService = await GetCalendarService();
            var batchRequest = new BatchRequest(calendarService);

            foreach (var calendarEvent in calendarEvents.Take(3)) //TODO: Take all at the end of project
            {
                batchRequest.Queue(calendarService.Events.Insert(calendarEvent, "primary"),
                    async (Event content, RequestError error, int index, HttpResponseMessage message) =>
                    {
                        var messageContent = await message.Content.ReadAsStringAsync();
                        if (error is null)
                        {
                            return;
                        }
                        
                        throw new Exception(error.Message);
                    });
            }

            await batchRequest.ExecuteAsync();
        }
        
        private async Task<CalendarService> GetCalendarService()
        {
            var credentials = await AuthorizeGoogleUser();
            var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = _planerConfig.GetApplicationName()
                });
            return service;
        }

        private IEnumerable<Event> ResolveEventsFromSchedule(StudentGroupSchedule schedule)
        {
            var result = schedule.ScheduleClasses
                .Select(x => new Event()
                {
                    Start = ResolveEventDate(x.Date, x.FromHour),
                    End = ResolveEventDate(x.Date, x.ToHour),
                    Summary = x.Subject,
                    Location = x.ClassRoom,
                    Description = $"{x.Type}, {x.Teacher.Text}",
                    Reminders = new Event.RemindersData {UseDefault = false}
                });
            return result;
        }

        private EventDateTime ResolveEventDate(DateTime date, string hour)
        {
            var hourDate = DateTime.Parse(hour);
            return new EventDateTime
            {
                TimeZone = "Europe/Warsaw",
                DateTime = new DateTime(date.Year, date.Month, date.Day, hourDate.Hour, hourDate.Minute, 0)
            };
        }

        private async Task<UserCredential> AuthorizeGoogleUser()
        {
            UserCredential credential;
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(120));
            var cred = new ClientSecrets()
            {
                ClientId = _planerConfig.GetGoogleClientId(),
                ClientSecret = _planerConfig.GetGoogleClientSecret()
            };
            try
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(cred, new[] { CalendarService.Scope.Calendar }, "user",
                    cancellationTokenSource.Token, _dataStore);
                return credential;
            }
            catch (Exception e)
            {
                throw new GoogleAuthorizationException();
            };
        }
    }
}