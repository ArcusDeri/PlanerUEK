using System.Threading.Tasks;
using PlanerUek.Storage.Models;
using PlanerUek.Website.Models;

namespace PlanerUek.Website.Services
{
    public interface IGoogleCalendar
    {
        Task<CalendarUpdateResult> AddStudentGroupSchedule(StudentGroupSchedule schedule);
    }
}