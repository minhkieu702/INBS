using INBS.Application.DTOs.DeviceToken;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.DeviceToken
{
    /// <summary>
    /// Get device tokens.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DeviceTokenController"/> class.
    /// </remarks>
    /// <param name="service">The device token service.</param>
    /// <param name="logger"></param>
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceTokenController(IDeviceTokenService service, ILogger<DeviceTokenController> logger) : ODataController
    {

        /// <summary>
        /// Gets the list of device tokens.
        /// </summary>
        /// <returns>An <see cref="IQueryable{DeviceTokenResponse}"/> of device tokens.</returns>
        [HttpGet]
        [EnableQuery]
        public IQueryable<DeviceTokenResponse> Get()
        {
            try
            {
                return service.Get();
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred while getting device tokens: {Message}", ex.Message);
                throw;
            }
        }
    }
}
