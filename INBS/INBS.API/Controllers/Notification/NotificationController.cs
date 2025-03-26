using INBS.Application.DTOs.Notification;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.Notification
{
    /// <summary>
    /// Get notification.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="NotificationController"/> class.
    /// </remarks>
    /// <param name="logger">The logger instance.</param>
    /// <param name="service">The notification service instance.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController(ILogger<NotificationController> logger, INotificationService service) : ODataController
    {

        /// <summary>
        /// Gets the list of notifications.
        /// </summary>
        /// <returns>An <see cref="IQueryable{NotificationResponse}"/> of notifications.</returns>
        [HttpGet]
        [EnableQuery]
        public IQueryable<NotificationResponse> Get()
        {
            try
            {
                return service.Get();
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred while getting notification: {Message}", ex.Message);
                throw;
            }
        }
    }
}
