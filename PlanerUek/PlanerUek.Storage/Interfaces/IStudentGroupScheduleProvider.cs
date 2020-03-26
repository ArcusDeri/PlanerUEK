using PlanerUek.Storage.Models;

namespace PlanerUek.Storage.Interfaces
{
    public interface IStudentGroupScheduleProvider
    {
        StudentGroupSchedule GetCurrentSchedule(string groupId);
    }
}