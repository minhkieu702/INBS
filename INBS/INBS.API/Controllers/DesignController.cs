using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Service.Service;
using INBS.Application.IService;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DesignController(IDesignService _service) : ControllerBase
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
        public async Task<IActionResult> Create([FromForm] DesignRequest design, [FromForm] IList<NewImageRequest> newImages)
        {
            try
            {
                await _service.Create(design, newImages);
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
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] DesignRequest design, [FromForm] IList<NewImageRequest> newImages, [FromForm] IList<ImageRequest> currentImages)
        {
            try
            {
                await _service.Update(id, design, newImages, currentImages);
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
                return Ok(ex.Message);
            }
        }
    }
}
