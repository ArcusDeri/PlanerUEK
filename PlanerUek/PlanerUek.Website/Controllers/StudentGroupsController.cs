using Microsoft.AspNetCore.Mvc;
using PlanerUek.Website.Configuration;

namespace PlanerUek.Website.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentGroupsController : ControllerBase
    {
        private readonly IPlanerConfig _config;

        public StudentGroupsController(IPlanerConfig config)
        {
            _config = config;
        }

        [HttpPost(nameof(HandleTimetableForGroup))]
        public IActionResult HandleTimetableForGroup([FromForm]string groupName)
        {
            var envName = _config.EnvironmentName();
            
            return Ok();
        }
    }
}