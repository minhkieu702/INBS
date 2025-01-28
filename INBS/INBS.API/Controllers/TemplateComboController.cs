using INBS.Application.DTOs.Service.ServiceTemplateCombo;
using INBS.Application.DTOs.Service.TemplateCombo;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing template combos.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TemplateComboController"/> class.
    /// </remarks>
    /// <param name="service">The template combo service.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateComboController(ITemplateComboService service) : ControllerBase
    {

        /// <summary>
        /// Gets the list of template combos.
        /// </summary>
        /// <returns>A list of template combos.</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var templateCombos = await service.Get();
                return Ok(templateCombos.AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Creates a new template combo.
        /// </summary>
        /// <param name="templateCombo">The template combo request.</param>
        /// <param name="services">The list of service template combo requests.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TemplateComboRequest templateCombo, [FromForm] IList<ServiceTemplateComboRequest> services)
        {
            try
            {
                await service.Create(templateCombo, services);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing template combo.
        /// </summary>
        /// <param name="id">The template combo ID.</param>
        /// <param name="templateCombo">The template combo request.</param>
        /// <param name="services">The list of service template combo requests.</param>
        /// <returns>An action result.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] TemplateComboRequest templateCombo, [FromForm] IList<ServiceTemplateComboRequest> services)
        {
            try
            {
                await service.Update(id, templateCombo, services);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a template combo.
        /// </summary>
        /// <param name="id">The template combo ID.</param>
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
