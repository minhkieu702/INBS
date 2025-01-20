using INBS.Application.DTOs.Booking;
using INBS.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Infrastructure.SignalR
{
    public class BookingHubService : IBookingHubService
    {
        private readonly IHubContext<BookingHub> _hubContext;

        public BookingHubService(IHubContext<BookingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendAppointmentAsync(BookingHubResponse response)
        {
            await _hubContext.Clients.All.SendAsync("SendAppointment", response);
        }


    }
}
