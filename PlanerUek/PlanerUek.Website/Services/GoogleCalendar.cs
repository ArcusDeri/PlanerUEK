using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using PlanerUek.Storage.Models;
using PlanerUek.Website.Models;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Util.Store;
using PlanerUek.Website.Configuration;

namespace PlanerUek.Website.Services
{
    public class GoogleCalendar : IGoogleCalendar
    {
        private readonly IPlanerConfig _planerConfig;

        public GoogleCalendar(IPlanerConfig planerConfig)
        {
            _planerConfig = planerConfig;
        }

        public CalendarUpdateResult AddStudentGroupSchedule(StudentGroupSchedule schedule)
        {
            var calendarEvents = ResolveEventsFromSchedule(schedule);
            var credentials = AuthorizeGoogleUser();
            throw new System.NotImplementedException();
        }

        private IEnumerable<Event> ResolveEventsFromSchedule(StudentGroupSchedule schedule)
        {
            var result = schedule.ScheduleClasses
                .Where(w => !w.Type.ToLower().Contains("przeniesienie"))
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

        private UserCredential AuthorizeGoogleUser()
        {
            var cred = new ClientSecrets()
            {
                ClientId = _planerConfig.GetGoogleClientId(),
                ClientSecret = _planerConfig.GetGoogleClientSecret()
            };
            return GoogleWebAuthorizationBroker.AuthorizeAsync(cred, new[] {CalendarService.Scope.Calendar}, "user",
                CancellationToken.None, new NullDataStore()).Result;
        }
    }
}