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
                return new BadRequestObjectResult(ex.Message);
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
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Get all occasions
        /// </summary>
        /// <returns></returns>
        [HttpGet("Occasions")]
        [EnableQuery]
        public async Task<IActionResult> GetOccasions()
        {
            try
            {
                var occasions = await _service.GetOccasions();
                return Ok(occasions.AsQueryable());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Get all skintone
        /// </summary>
        /// <returns></returns>
        [HttpGet("Skintone")]
        [EnableQuery]
        public async Task<IActionResult> GetSkintones()
        {
            try
            {
                var paintType = await _service.GetSkinTone();
                return Ok(paintType.AsQueryable());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Get all paint types
        /// </summary>
        /// <returns></returns>
        [HttpGet("PaintType")]
        [EnableQuery]
        public async Task<IActionResult> GetPaintTypes()
        {
            try
            {
                var paintTypes = await _service.GetPaintType();
                return Ok(paintTypes.AsQueryable());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
