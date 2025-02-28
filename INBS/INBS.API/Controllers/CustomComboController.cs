using INBS.Application.DTOs.Service.CustomCombo;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing custom combos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomComboController(ICustomComboService service) : ControllerBase
    {
        /// <summary>
        /// Gets the list of custom combos.
        /// </summary>
        /// <returns>A list of custom combos.</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var cc = await service.Get();
                return Ok(cc.AsQueryable());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new custom combo.
        /// </summary>
        /// <param name="customCombo">The custom combo request.</param>
        /// <param name="additionalServiceIds">The list of service custom combo requests.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CustomComboRequest customCombo, [FromForm] IList<Guid> additionalServiceIds)
        {
            try
            {
                await service.Create(customCombo, additionalServiceIds);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing custom combo.
        /// </summary>
        /// <param name="id">The ID of the custom combo to update.</param>
        /// <param name="customCombo">The custom combo request.</param>
        /// <param name="additionalServiceIds">The list of service custom combo requests.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] CustomComboRequest customCombo, [FromForm] IList<Guid> additionalServiceIds)
        {
            try
            {
                await service.Update(id, customCombo, additionalServiceIds);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a custom combo.
        /// </summary>
        /// <param name="id">The ID of the custom combo to delete.</param>
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
