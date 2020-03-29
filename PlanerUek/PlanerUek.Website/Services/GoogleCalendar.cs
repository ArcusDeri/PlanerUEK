using System;
using System.Collections.Generic;
using System.Linq;
using PlanerUek.Storage.Models;
using PlanerUek.Website.Models;
using Google.Apis.Calendar.v3.Data;

namespace PlanerUek.Website.Services
{
    public class GoogleCalendar : IGoogleCalendar
    {
        public CalendarUpdateResult AddStudentGroupSchedule(StudentGroupSchedule schedule)
        {
            var calendarEvents = ResolveEventsFromSchedule(schedule);
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
    }
}