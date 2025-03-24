using INBS.Application.IService;
using INBS.Application.IServices;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IMemoryCache _cache;

        private readonly ILogger<AutoNotificationBooking> _logger;

        public AutoNotificationBooking(IBookingService bookingService, INotificationService notificationService, 
            ISchedulerFactory schedulerFactory,
            IMemoryCache cache,
            ILogger<AutoNotificationBooking> logger)
        {
            _bookingService = bookingService;
            _notificationService = notificationService;
            _schedulerFactory = schedulerFactory;
            _cache = cache;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //_logger.LogInformation("Running AutoNotificationBooking Job...");

            // Lấy danh sách booking sắp diễn ra trong vòng 30 phút
            var upcomingBookings = await _bookingService.GetBookingsWithinNextMinutes(30);
            var scheduler = await _schedulerFactory.GetScheduler();

            foreach (var booking in upcomingBookings)
            {
                if (booking == null 
                    || booking.CustomerSelected == null 
                    || booking.CustomerSelected.Customer == null
                    || booking.CustomerSelected.Customer.User == null
                    || booking.CustomerSelected.Customer.User.PhoneNumber == null
                    )
                {
                    continue;
                }
                string phoneNumber = booking.CustomerSelected.Customer.User.PhoneNumber;
                string message = $"Xin chào {booking.CustomerSelected.Customer.User.FullName}, lịch hẹn của bạn sẽ diễn ra vào {booking.StartTime}. Vui lòng đến đúng giờ!";

                if (IsValidPhoneNumber(phoneNumber))
                {
                    // Kiểm tra nếu số điện thoại đã được gửi tin nhắn trong 30 phút gần đây
                    if (_cache.TryGetValue($"SentSMS:{phoneNumber}", out _))
                    {
                        _logger.LogInformation($"SMS already sent to {phoneNumber}. Skipping...");
                        continue;
                    }

                    // Lưu vào cache để tránh gửi lại trong 30 phút
                    _cache.Set($"SentSMS:{phoneNumber}", true, TimeSpan.FromMinutes(30));

                    // Tạo một Job để gửi SMS
                    var job = JobBuilder.Create<SendSmsNotificationJob>()
                        .UsingJobData("PhoneNumber", phoneNumber)
                        .UsingJobData("Message", message)
                        .Build();

                    // Tạo Trigger để chạy ngay lập tức
                    var trigger = TriggerBuilder.Create()
                        .StartNow()
                        .Build();

                    await scheduler.ScheduleJob(job, trigger);

                    _logger.LogInformation($"Scheduled SMS Job for {phoneNumber} (Booking ID: {booking.ID})");
                }
                else
                {
                    _logger.LogWarning($"Invalid phone number: {phoneNumber} (Booking ID: {booking.ID})");
                }
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length >= 10;
        }
    }
}
