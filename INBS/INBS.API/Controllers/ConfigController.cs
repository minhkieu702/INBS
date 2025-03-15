using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Environment.GetEnvironmentVariable("connectionString"));
        }
        }
}
