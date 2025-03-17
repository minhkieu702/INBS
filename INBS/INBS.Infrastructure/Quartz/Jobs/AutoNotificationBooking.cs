using INBS.Application.IService;
using INBS.Application.IServices;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Infrastructure.Quartz.Jobs
{
    public class AutoNotificationBooking : IJob
    {
        private readonly IBookingService _bookingService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<AutoNotificationBooking> _logger;

        public AutoNotificationBooking(IBookingService bookingService, INotificationService notificationService, ILogger<AutoNotificationBooking> logger)
        {
            _bookingService = bookingService;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Running SendReminderJob...");

            // Lấy danh sách booking sắp diễn ra trong vòng 30 phút
            var upcomingBookings = await _bookingService.GetBookingsWithinNextMinutes(30);

            foreach (var booking in upcomingBookings)
            {
                // Gửi thông báo đến khách hàng
                var message = $"Xin chào {booking.CustomerSelected.Customer.User.FullName}, lịch hẹn của bạn sẽ diễn ra vào {booking.StartTime}. Vui lòng đến đúng giờ!";
                await _notificationService.SendNotificationAsync(booking.CustomerSelected.Customer.User.PhoneNumber, message);

                _logger.LogInformation($"Sent reminder to {booking.CustomerSelected.Customer.User.PhoneNumber} for Booking {booking.ID}");
            }
        }
    }
}
