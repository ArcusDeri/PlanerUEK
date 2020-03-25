using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlanerUek.Storage.Interfaces;

namespace PlanerUek.Website.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentGroupsController : ControllerBase
    {
        private readonly IStudentGroupsRepository _studentGroupsRepository;
        private readonly IStudentGroupScheduleProvider _scheduleProvider;

        public StudentGroupsController(IStudentGroupsRepository studentGroupsRepository,
            IStudentGroupScheduleProvider scheduleProvider)
        {
            _studentGroupsRepository = studentGroupsRepository;
            _scheduleProvider = scheduleProvider;
        }

        [HttpPost(nameof(HandleTimetableForGroup))]
        public async Task<IActionResult> HandleTimetableForGroup([FromForm] string groupName)
        {
            var groupId = await _studentGroupsRepository.GetGroupId(groupName);
            var schedule = _scheduleProvider.GetSchedule(groupId);

            return Ok();
        }
    }
}