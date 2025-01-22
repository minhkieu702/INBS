using INBS.Application.DTOs.Service.ServiceTemplateCombo;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing template combos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateComboController : ControllerBase
    {
        private readonly ITemplateComboService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateComboController"/> class.
        /// </summary>
        /// <param name="service">The template combo service.</param>
        public TemplateComboController(ITemplateComboService service)
        {
            _service = service;
        }

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
                var templateCombos = await _service.Get();
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
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TemplateComboRequest templateCombo)
        {
            try
            {
                await _service.Create(templateCombo);
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
        /// <returns>An action result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] TemplateComboRequest templateCombo)
        {
            try
            {
                await _service.Update(id, templateCombo);
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
