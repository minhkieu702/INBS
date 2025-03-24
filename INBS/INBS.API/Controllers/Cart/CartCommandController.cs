using INBS.Application.DTOs.Cart;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers.Cart
{

    /// <summary>
    /// Controller for handling cart commands.
    /// </summary>
    [ApiController]
    [Route("api/Cart")]
    public class CartCommandController : ControllerBase
    {
        private readonly ICartService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartCommandController"/> class.
        /// </summary>
        /// <param name="service">The cart service.</param>
        public CartCommandController(ICartService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new cart.
        /// </summary>
        /// <param name="cart">The cart request.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the action.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CartRequest cart)
        {
            try
            {
                await _service.Create(cart);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a cart by ID.
        /// </summary>
        /// <param name="id">The cart ID.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the action.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
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
