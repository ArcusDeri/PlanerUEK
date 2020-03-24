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

        public StudentGroupsController(IStudentGroupsRepository studentGroupsRepository)
        {
            _studentGroupsRepository = studentGroupsRepository;
        }

        [HttpPost(nameof(HandleTimetableForGroup))]
        public async Task<IActionResult> HandleTimetableForGroup([FromForm] string groupName)
        {
            var groupId = await _studentGroupsRepository.GetGroupId(groupName);

            return Ok();
        }
    }
}