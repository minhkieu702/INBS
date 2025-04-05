using INBS.Application.DTOs.DeviceToken;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers.DeviceToken
{
    /// <summary>
    /// Controller for handling device token commands.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DeviceTokenCommandController"/> class.
    /// </remarks>
    /// <param name="logger">The logger instance.</param>
    /// <param name="services">The device token service instance.</param>
    [ApiController]
    [Route("api/DeviceToken")]
    public class DeviceTokenCommandController(ILogger<DeviceTokenCommandController> logger, IDeviceTokenService services) : ControllerBase
    {
        private readonly ILogger<DeviceTokenCommandController> _logger = logger;

        /// <summary>
        /// Adds a new device token.
        /// </summary>
        /// <param name="deviceToken">The device token request.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the action.</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] DeviceTokenRequest deviceToken)
        {
            try
            {
                await services.AddDeviceToken(deviceToken);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while adding device tokens: {Message}", ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an existing device token.
        /// </summary>
        /// <param name="deviceToken">The device token to delete.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the action.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] string deviceToken)
        {
            try
            {
                await services.RemoveDeviceToken(deviceToken);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting device tokens: {Message}", ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
