using INBS.Application.DTOs.Common.Preference;
using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Design.NailDesign;
using INBS.Application.DTOs.Service.Service;
using INBS.Application.IService;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing designs.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DesignController"/> class.
    /// </remarks>
    /// <param name="service">The design service.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class DesignController(IDesignService service) : ControllerBase
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
                var designs = await service.Get();
                return Ok(designs.AsQueryable());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new design.
        /// </summary>
        /// <param name="design">The design creation request.</param>
        /// <param name="images">The list of image requests.</param>
        /// <param name="nailDesigns">The list of nail design requests.</param>
        /// <param name="preference">The preference of design</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DesignRequest design, [FromForm] PreferenceRequest preference, [FromForm] IList<ImageRequest> images, [FromForm] IList<NailDesignRequest> nailDesigns)
        {
            try
            {
                await service.Create(design, preference, images, nailDesigns);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing design.
        /// </summary>
        /// <param name="id">The design ID.</param>
        /// <param name="design">The design update request.</param>
        /// <param name="preference">The preference of design</param>
        /// <param name="images">The list of image requests.</param>
        /// <param name="nailDesigns">The list of nail design requests.</param>
        /// <returns>An action result.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] DesignRequest design,
[FromForm] PreferenceRequest preference, [FromForm] IList<ImageRequest> images, [FromForm] IList<NailDesignRequest> nailDesigns)
        {
            try
            {
                await service.Update(id, design, preference, images, nailDesigns);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
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
