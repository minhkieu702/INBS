using INBS.Application.DTOs.Service;
using INBS.Application.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for handling category-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService categoryService) : ODataController
    {
        private readonly ICategoryService _categoryService = categoryService;

        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = await _categoryService.Get();
                return Ok(categories.AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Creates a category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryRequest category)
        {
            try
            {
                await _categoryService.Create(category);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Updates a category.
        /// </summary>
        /// <param name="id">detects the updated category</param>
        /// <param name="category">uses to update new information</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CategoryRequest category)
        {
            try
            {
                await _categoryService.Update(id, category);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Deletes a category.
        /// </summary>
        /// <param name="id">detects the deleted category</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _categoryService.DeleteById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            var some = Environment.GetEnvironmentVariable("connectionString");
            return Ok(some);
        }
    }
}
