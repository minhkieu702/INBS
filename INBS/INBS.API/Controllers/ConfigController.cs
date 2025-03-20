using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;

        public ConfigController(ILogger<ConfigController> logger)
        {
            _logger = logger;
        }

        [HttpGet("ReadConnectionString")]
        public IActionResult GetConnectionString()
        {
            var connectionString = Environment.GetEnvironmentVariable("connectionString") ?? "Not Found";

            _logger.LogWarning("Connection String: {ConnectionString}", connectionString);
            _logger.LogInformation("hiiiiiiiiiiiiiiiiiiiiiii");

            return Ok(connectionString);
        }

        [HttpGet("ReadFirebaseConfig")]
        public IActionResult GetFirebaseConfig()
        {
            var firebaseConfig = Environment.GetEnvironmentVariable("FirebaseSettings_config") ?? "Not Found";
            return Ok(firebaseConfig);
        }
    }
}
