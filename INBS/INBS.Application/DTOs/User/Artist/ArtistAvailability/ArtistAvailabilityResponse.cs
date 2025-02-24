using INBS.Application.DTOs.Common;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User.Artist.ArtistAvailability
{
    public class ArtistAvailabilityResponse : BaseEntity
    {
        public Guid ArtistId { get; set; }
        public DateOnly AvailableDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int BreakTime { get; set; } //minute
        public int MaximumBreakTime { get; set; }
        public virtual ArtistResponse? Artist { get; set; }
        //public virtual ICollection<BookingResponse> Bookings { get; set; }
    }
}
