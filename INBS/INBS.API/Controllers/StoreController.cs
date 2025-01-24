using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Store;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController(IStoreService _service) : ControllerBase
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
                var stores = await _service.Get();
                return Ok(stores.AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Creates a new store
        /// </summary>
        /// <param name="store">The store creation request.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StoreRequest store)
        {
            try
            {
                await _service.Create(store, User);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing store.
        /// </summary>
        /// <param name="id">The store ID.</param>
        /// <param name="store">The store update request.</param>
        /// <returns>An action result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] StoreRequest store)
        {
            try
            {
                await _service.Update(id, store);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a store by ID.
        /// </summary>
        /// <param name="id">The store ID.</param>
        /// <returns>An action result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
