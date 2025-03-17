using INBS.Application.DTOs.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IService
{
    public interface IBookingService
    {
        Task Create(BookingRequest request);

        Task CancelBooking(Guid id);

        Task Update(Guid id, BookingRequest request);

        IQueryable<BookingResponse> Get();

        Task CompleteBooking(Guid id);
    }
}
