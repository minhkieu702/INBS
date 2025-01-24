using INBS.Application.Interfaces;
using INBS.Application.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Net.Http;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for handling category-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AdjectiveController(IAdjectiveService _service) : ControllerBase
    {
        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        [HttpGet("Categories")]
        [EnableQuery]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _service.GetCategory();
                return Ok(categories.AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get all colors
        /// </summary>
        /// <returns></returns>
        [HttpGet("Colors")]
        [EnableQuery]
        public async Task<IActionResult> GetColors()
        {
            try
            {
                var colors = await _service.GetColor();
                return Ok(colors.AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


    }
}
