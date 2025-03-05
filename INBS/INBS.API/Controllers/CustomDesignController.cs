using INBS.Application.DTOs.Design.CustomDesign;
using INBS.Application.DTOs.Design.CustomNailDesign;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for handling custom design related requests.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomDesignController(ICustomDesignService service) : ControllerBase
    {
        /// <summary>
        /// Retrieves all custom designs.
        /// </summary>
        /// <returns>An IActionResult containing the list of custom designs.</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await service.Get();
                return Ok(result.AsQueryable());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new custom design.
        /// </summary>
        /// <param name="customDesign">The custom design request.</param>
        /// <param name="customNailDesigns">The list of custom nail design requests.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CustomDesignRequest customDesign, [FromForm] IList<CustomNailDesignRequest> customNailDesigns)
        {
            try
            {
                var result = await service.Create(customDesign, customNailDesigns);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing custom design.
        /// </summary>
        /// <param name="id">The ID of the custom design to update.</param>
        /// <param name="customDesign">The custom design request.</param>
        /// <param name="customNailDesigns">The list of custom nail design requests.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] CustomDesignRequest customDesign, [FromForm] IList<CustomNailDesignRequest> customNailDesigns)
        {
            try
            {
                await service.Update(id, customDesign, customNailDesigns);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an existing custom design.
        /// </summary>
        /// <param name="id">The ID of the custom design to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
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
