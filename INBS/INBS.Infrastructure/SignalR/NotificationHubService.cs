using INBS.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Infrastructure.SignalR
{
    public class NotificationHubService : INotificationHubService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHubService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyBookingCreated(Guid artistId, string title, string message, object bookingData)
        {
            await _hubContext.Clients.Group($"user_{artistId}")
                .SendAsync("ReceiveBookingNotification", title, message, bookingData);
        }

        public async Task NotifyBookingUpdated(Guid artistId, string title, string message, object bookingData)
        {
            await _hubContext.Clients.Group($"user_{artistId}")
                .SendAsync("ReceiveBookingUpdate", title, message, bookingData);
        }

        public async Task NotifyBookingCanceled(Guid artistId, string title, string message, Guid bookingId)
        {
            await _hubContext.Clients.Group($"user_{artistId}")
                .SendAsync("ReceiveBookingCancellation", title, message, bookingId);
        }
    }
}
