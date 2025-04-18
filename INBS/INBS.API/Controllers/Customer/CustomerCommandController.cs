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
        public async Task<IActionResult> UpdatePreferences([FromBody] CustomerPreferenceRequest request)
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
    }
}
