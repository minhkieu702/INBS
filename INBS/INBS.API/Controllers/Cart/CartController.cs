using INBS.Application.DTOs.Cart;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.Cart
{

    /// <summary>
    /// Controller for managing cart operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ODataController
    {
        private readonly ICartService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="service">The cart service.</param>
        public CartController(ICartService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the list of cart responses.
        /// </summary>
        /// <returns>An <see cref="IQueryable{CartResponse}"/> of cart responses.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth =100)]
        public IQueryable<CartResponse> Get()
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
