using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController() : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                return Ok("DONE");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
