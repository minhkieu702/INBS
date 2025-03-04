using INBS.Application.DTOs.Booking;
using INBS.Application.IService;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class BookingService : IBookingService
    {
        public Task Create(BookingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookingResponse>> Get()
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid id, BookingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
