using Microsoft.AspNetCore.Mvc;

namespace Ticketing.Api.Controllers
{
    [ApiController]
    [Route("health")]
    [Tags("Health")]
    public sealed class HealthController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(new
            {
                Status = "Healthy",
                UtcNow = DateTimeOffset.UtcNow
            });
        }
    }
}
