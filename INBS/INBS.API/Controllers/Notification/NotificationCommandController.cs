using INBS.Application.DTOs.DeviceToken;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers.Notification
{
    /// <summary>
    /// Controller for handling notification commands.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="NotificationCommandController"/> class.
    /// </remarks>
    /// <param name="logger">The logger instance.</param>
    /// <param name="service">The notification service instance.</param>
    [ApiController]
    [Route("api/Notification")]
    public class NotificationCommandController(ILogger<NotificationCommandController> logger, INotificationService service) : ControllerBase
    {

        /// <summary>
        /// Marks a notification as seen.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> MarkSeen()
        {
            try
            {
                await service.MarkSeenNotification();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred while marking the notification as seen: {Message}", ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
