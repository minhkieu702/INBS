using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for configuration related actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public partial class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>

        public ConfigController(ILogger<ConfigController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets the connection string from environment variables.
        /// </summary>
        /// <returns>The connection string.</returns>
        [HttpGet("ReadConnectionString")]
        public IActionResult GetConnectionString()
        {
            var connectionString = Environment.GetEnvironmentVariable("connectionString") ?? "Not Found";

            _logger.LogWarning("Connection String: {ConnectionString}", connectionString);
            _logger.LogInformation("hiiiiiiiiiiiiiiiiiiiiiii");

            return Ok(connectionString);
        }

        /// <summary>
        /// Gets the Firebase configuration from environment variables.
        /// </summary>
        /// <returns>The Firebase configuration.</returns>
        [HttpGet("ReadFirebaseConfig")]
        public IActionResult GetFirebaseConfig()
        {
            var firebaseConfig = Environment.GetEnvironmentVariable("FirebaseSettings:config") ?? "Not Found";
            return Ok(firebaseConfig);
        }
    }
}
