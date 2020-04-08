using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlanerUek.Storage.Interfaces;
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

        public StudentGroupsController(IStudentGroupsRepository studentGroupsRepository,
            IStudentGroupScheduleProvider scheduleProvider, IGoogleCalendar googleCalendar)
        {
            _studentGroupsRepository = studentGroupsRepository;
            _scheduleProvider = scheduleProvider;
            _googleCalendar = googleCalendar;
        }

        [HttpPost(nameof(HandleTimetableForGroup))]
        public async Task<IActionResult> HandleTimetableForGroup(AddScheduleToCalendarRequest clientRequest)
        {
            var groupId = await _studentGroupsRepository.GetGroupId(clientRequest.GroupName);
            if (string.IsNullOrEmpty(groupId))
            {
                return Problem("Id for provided group not found.");
            }
            var schedule = _scheduleProvider.GetCurrentSchedule(groupId);
            var result = await _googleCalendar.AddStudentGroupSchedule(schedule);
            
            if (!result.IsSuccess)
            {
                return Problem(result.ErrorMessage);
            }
            
            return Ok(result);
        }
    }
}