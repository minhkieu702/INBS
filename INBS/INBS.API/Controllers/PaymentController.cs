using INBS.Application.DTOs.PayOS;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebHook([FromBody] PayOSRequest payment)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
