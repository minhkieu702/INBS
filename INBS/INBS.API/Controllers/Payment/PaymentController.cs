using INBS.Application.DTOs.Payment;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.Payment
{
    /// <summary>
    /// Controller for handling payment-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ODataController
    {
        private readonly IPaymentService _service;
        private readonly ILogger<PaymentCommandController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentCommandController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="service">The payment service.</param>
        public PaymentController(ILogger<PaymentCommandController> logger, IPaymentService service)
        {
            _service = service;
            _logger = logger;
        }
        /// <summary>
        /// Retrieves a list of payment responses.
        /// </summary>
        /// <returns>An IQueryable of PaymentResponse.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 10)]
        public IQueryable<PaymentResponse> Get()
        {
            try
            {
                return _service.Get();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    }
