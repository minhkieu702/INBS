using INBS.Application.DTOs.Customer;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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
                await service.UpdatePreferencesAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Gets customers.
        /// </summary>
        /// <returns>Customer preferences.</returns>
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
    }
}
