using INBS.Application.DTOs.Store;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing store operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="StoreController"/> class.
    /// </remarks>
    /// <param name="service">The store service.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController(IStoreService service) : ControllerBase
    {
        /// <summary>
        /// Gets the list of stores.
        /// </summary>
        /// <returns>A list of stores.</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var stores = await service.Get();
                return Ok(stores.AsQueryable());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new store.
        /// </summary>
        /// <param name="store">The store creation request.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StoreRequest store)
        {
            try
            {
                await service.Create(store);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing store.
        /// </summary>
        /// <param name="id">The store ID.</param>
        /// <param name="store">The store update request.</param>
        /// <returns>An action result.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] StoreRequest store)
        {
            try
            {
                await service.Update(id, store);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a store by ID.
        /// </summary>
        /// <param name="id">The store ID.</param>
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
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
