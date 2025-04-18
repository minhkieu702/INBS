using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.Preference;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace INBS.API.Controllers.Customer
{
    /// <summary>
    /// Controller for managing customers.
    /// </summary>
    [ApiController]
    [Route("api/Customer")]
    public class CustomerCommandController(ICustomerService service, IHttpClientFactory httpClientFactory) : ControllerBase
    {   
        /// <summary>
        /// Updates/Create customer preferences.
        /// </summary>
        /// <param name="request">Preferences request.</param>
        /// <returns>Updated customer entity.</returns>
        [HttpPost("preferences")]
        public async Task<IActionResult> UpdatePreferences([FromBody] PreferenceRequest request)
        {
            try
            {
                var user = User;
                await service.UpdatePreferencesAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpPost("detect")]
        public async Task<IActionResult> DetectSkinTone(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image uploaded.");

            using var stream = image.OpenReadStream();
            var skinTone = await service.DetectSkinToneFromImage(stream);

            return Ok(skinTone);
        }

        [HttpGet("RecommendDesign")]
        public async Task<IActionResult> RecommendDesign([FromForm] Guid customerId, [FromForm] IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                    return BadRequest(new { Message = "Image is required." });

                using var imageStream = image.OpenReadStream();

                var recommendation = await service.GetDesignRecommendation(customerId, imageStream);

                return Ok(new { Recommendation = recommendation });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
