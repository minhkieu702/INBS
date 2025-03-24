using INBS.Application.IServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public async Task SendNotificationAsync(string phoneNumber, string message)
        {
            // Giả lập gửi tin nhắn (có thể thay bằng Twilio, Firebase, v.v.)
            _logger.LogInformation($"Sending SMS to {phoneNumber}: {message}");
            await Task.CompletedTask;
        }

        public async Task AddDeviceToken(string deviceToken)
        {
            try
            {
                _logger.LogInformation($"Add device token: {deviceToken}");

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
