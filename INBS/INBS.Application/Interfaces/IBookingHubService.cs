using INBS.Application.DTOs.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Interfaces
{
    public interface IBookingHubService
    {
        Task SendAppointmentAsync(BookingHubResponse response);
    }
}
