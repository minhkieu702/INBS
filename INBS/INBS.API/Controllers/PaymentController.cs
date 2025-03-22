using INBS.Application.DTOs.Payment;
using INBS.Application.DTOs.PaymentDetail;
using INBS.Application.DTOs.PayOS;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Net.payOS.Types;
using Newtonsoft.Json;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for handling payment-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ODataController
    {
        private readonly IPaymentService _service;
        private readonly ILogger<PaymentController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="service">The payment service.</param>
        public PaymentController(ILogger<PaymentController> logger, IPaymentService service)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Handles the payment callback.
        /// </summary>
        /// <returns>An IActionResult.</returns>
        [HttpGet("return-url")]
        public async Task<IActionResult> HandlePaymentCallback()
        {
            var code = Request.Query["code"];
            var id = Request.Query["id"];
            var cancel = Request.Query["cancel"];
            var status = Request.Query["status"];
            var orderCode = Request.Query["orderCode"];

            if (long.TryParse(orderCode, out long paymentId) && bool.TryParse(cancel, out bool isCancel))
            {
                await _service.ReturnUrl(paymentId, !isCancel);
            }

            return Ok(new
            {
                Code = code,
                Id = id,
                Cancel = cancel,
                Status = status,
                OrderCode = orderCode
            });
        }

        /// <summary>
        /// Confirms the webhook.
        /// </summary>
        /// <param name="payment">The webhook body.</param>
        /// <returns>An IActionResult.</returns>
        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebHook([FromBody] WebhookBody payment)
        {
            try
            {
                _logger.LogWarning("Hi");
                _logger.LogInformation("Webhook received: {WebhookData}", JsonConvert.SerializeObject(payment));
                await _service.ConfirmWebHook(payment);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error confirming webhook: {Message}", ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Confirms the webhook test.
        /// </summary>
        /// <param name="payment">The webhook type.</param>
        /// <returns>An IActionResult.</returns>
        [HttpPost("confirm_webhook")]
        public async Task<IActionResult> ConfirmWebHookTest([FromBody] WebhookType payment)
        {
            try
            {
                _logger.LogWarning("Hi");
                _logger.LogInformation("Webhook received: {WebhookData}", JsonConvert.SerializeObject(payment));
                await _service.ConfirmWebHook(payment);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error confirming webhook test: {Message}", ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of payment responses.
        /// </summary>
        /// <returns>An IQueryable of PaymentResponse.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 10)]
        public IQueryable<PaymentResponse> Get()
        {
            return _service.Get();
        }

        /// <summary>
        /// Creates a PayOS URL.
        /// </summary>
        /// <param name="paymentDetailRequests">The payment detail requests.</param>
        /// <returns>An IActionResult.</returns>
        [HttpPost("PayOSUrl")]
        public async Task<IActionResult> CreatePayOSUrl([FromForm] IList<PaymentDetailRequest> paymentDetailRequests)
        {
            try
            {
                var service = await _service.CreatePayOSUrl(new PaymentRequest(), paymentDetailRequests);
                return Ok(service.ToString());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Creates a payment for cash.
        /// </summary>
        /// <param name="paymentRequest">The payment request.</param>
        /// <param name="paymentDetailRequests">The payment detail requests.</param>
        /// <returns>An IActionResult.</returns>
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

        /// <summary>
        /// Deletes a payment by ID.
        /// </summary>
        /// <param name="id">The payment ID.</param>
        /// <returns>An IActionResult.</returns>
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
