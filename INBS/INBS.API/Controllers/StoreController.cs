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
        /// Gets the list of designs.
        /// </summary>
        /// <returns>A list of designs.</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var designs = await _service.Get();
                return Ok(designs.AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Creates a new design
        /// </summary>
        /// <param name="design">The design creation request.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StoreRequest design)
        {
            try
            {
                await _service.Create(design);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing design.
        /// </summary>
        /// <param name="id">The design ID.</param>
        /// <param name="design">The design update request.</param>
        /// <returns>An action result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] StoreRequest design)
        {
            try
            {
                await _service.Update(id, design);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a design by ID.
        /// </summary>
        /// <param name="id">The design ID.</param>
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
