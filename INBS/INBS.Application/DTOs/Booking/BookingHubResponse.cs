using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Booking
{
    public class BookingHubResponse
    {
        public Guid CustomerId { get; set; }
        public Guid ArtistId { get; set; }
        public Guid AdminId { get; set; }
        public DateTime ServiceTime { get; set; }
    }
}
