using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        [HttpGet("ReadConnectionString")]
        public IActionResult GetconnectionString()
        {
            return Ok(Environment.GetEnvironmentVariable("connectionString"));
        }

        [HttpGet("ReadFirebaseConfig")]
        public IActionResult GetFirebaseConfig()
        {
            return Ok(Environment.GetEnvironmentVariable("FirebaseSettings:config"));
        }
        }
}
