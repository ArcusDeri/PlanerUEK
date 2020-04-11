using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using PlanerUek.Storage.Models;
using PlanerUek.Website.Models;

namespace PlanerUek.Website.Services
{
    public interface IGoogleCalendar
    {
        Task<CalendarUpdateResult> AddStudentGroupSchedule(CalendarService calendarService, StudentGroupSchedule schedule);
    }
}