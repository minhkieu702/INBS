using INBS.Application.DTOs.Customer;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing customers.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController(ICustomerService service) : ControllerBase
    {
        /// <summary>
        /// Updates/Create customer preferences.
        /// </summary>
        /// <param name="request">Preferences request.</param>
        /// <returns>Updated customer entity.</returns>
        [HttpPost("preferences")]
        public async Task<IActionResult> UpdatePreferences([FromBody] PreferencesRequest request)
        {
            try
            {
                var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await service.UpdatePreferencesAsync(customerId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }

}
