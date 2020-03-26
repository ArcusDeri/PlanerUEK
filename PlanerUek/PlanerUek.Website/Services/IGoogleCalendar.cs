using PlanerUek.Storage.Models;
using PlanerUek.Website.Models;

namespace PlanerUek.Website.Services
{
    public interface IGoogleCalendar
    {
        CalendarUpdateResult AddStudentGroupSchedule(StudentGroupSchedule schedule);
    }
}