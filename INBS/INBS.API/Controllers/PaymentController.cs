using INBS.Application.DTOs.Payment;
using INBS.Application.DTOs.PaymentDetail;
using INBS.Application.DTOs.PayOS;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for handling payment-related operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PaymentController"/> class.
    /// </remarks>
    /// <param name="service">The payment service.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController(IPaymentService service) : ControllerBase
    {
        private readonly IPaymentService _service = service;

        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebHook([FromBody] WebhookBody payment)
        {
            try
            {
                await _service.ConfirmWebHook(payment);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet]
        public IQueryable Get()
        {
            return _service.Get();
        }

        [HttpPost("PayOSUrl")]
        public async Task<IActionResult> CreatePayOSUrl([FromForm] PaymentRequest paymentRequest, [FromForm] IList<PaymentDetailRequest> paymentDetailRequests)
        {
            try
            {
                var service = await _service.CreatePayOSUrl(paymentRequest, paymentDetailRequests);

                return Ok(service.ToString());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpPost("PaymentForCash")]
        public async Task<IActionResult> CreatePaymentForCash([FromForm] PaymentRequest paymentRequest, [FromForm] IList<PaymentDetailRequest> paymentDetailRequests)
        {
            try
            {
                await _service.CreatePaymentForCash(paymentRequest, paymentDetailRequests);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                await _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
