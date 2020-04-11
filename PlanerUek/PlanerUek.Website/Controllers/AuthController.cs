using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlanerUek.Storage.Interfaces;
using PlanerUek.Website.Models;
using PlanerUek.Website.Services;

namespace PlanerUek.Website.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        [HttpPost(nameof(Google))]
        public async Task<IActionResult> Google(GoogleAuthRequest authRequest)
        {
            var userId = authRequest.UserId; 
            return Redirect($"/setup/{userId}");
        }

        [HttpGet(nameof(GoogleResponse))]
        public IActionResult GoogleResponse()
        {
            return Ok();
        }
    }
}