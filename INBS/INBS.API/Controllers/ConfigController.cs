using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController(ILogger<ConfigController> logger) : ControllerBase
    {
        [HttpGet("ReadConnectionString")]
        public IActionResult GetconnectionString()
        {
            logger.LogWarning(Environment.GetEnvironmentVariable("connectionString"));
            logger.LogInformation("hiiiiiiiiiiiiiiiiiiiiiii");
            return Ok(Environment.GetEnvironmentVariable("connectionString"));
        }

        [HttpGet("ReadFirebaseConfig")]
        public IActionResult GetFirebaseConfig()
        {
            return Ok(Environment.GetEnvironmentVariable("FirebaseSettings:config"));
        }
        }
}
