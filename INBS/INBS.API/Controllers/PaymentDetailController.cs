using INBS.Application.DTOs.NailDesignService;
using INBS.Application.DTOs.PaymentDetail;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for handling payment details.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentDetailController : ODataController
    {
        private readonly IPaymentDetailService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentDetailController"/> class.
        /// </summary>
        /// <param name="service">The payment detail service.</param>
        public PaymentDetailController(IPaymentDetailService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets the payment details.
        /// </summary>
        /// <returns>An <see cref="IQueryable{PaymentDetailResponse}"/> of payment details.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<PaymentDetailResponse> Get()
        {
            return service.Get();
        }
    }
}
