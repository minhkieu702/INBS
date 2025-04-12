using INBS.Application.DTOs.Booking;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Infrastructure.SignalR
{
    public class BookingHub : Hub
    {
        public async Task SendBookingNotification(BookingHubResponse response)
        {
            await Clients.All.SendAsync("ReceiveBookingNotification", response);
        }
    }
}
