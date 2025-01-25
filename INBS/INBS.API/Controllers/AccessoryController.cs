using INBS.Application.DTOs.Design.Accessory;
using INBS.Application.DTOs.Store;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing accessory operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="StoreController"/> class.
    /// </remarks>
    /// <param name="service">The accessory service.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class AccessoryController(IAccessoryService service) : ControllerBase
    {/// <summary>
     /// Gets the list of accessories.
     /// </summary>
     /// <returns>A list of accessories.</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var accessories = await service.Get();
                return Ok(accessories.AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Creates a new accessory.
        /// </summary>
        /// <param name="accessory">The accessory creation request.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AccessoryRequest accessory)
        {
            try
            {
                await service.Create(accessory);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing accessory.
        /// </summary>
        /// <param name="id">The accessory ID.</param>
        /// <param name="accessory">The accessory update request.</param>
        /// <returns>An action result.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] AccessoryRequest accessory)
        {
            try
            {
                await service.Update(id, accessory);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a accessory by ID.
        /// </summary>
        /// <param name="id">The accessory ID.</param>
        /// <returns>An action result.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            try
            {
                await service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
