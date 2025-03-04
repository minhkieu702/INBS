using INBS.Application.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Booking
{
    public class BookingRequest
    {
        public DateOnly ServiceDate { get; set; }

        public TimeOnly ServiceTime { get; set; }

        public long Duration { get; set; }

        public BookingStatus Status { get; set; }

        public long TotalAmount { get; set; }

        public int? PaymentMethod { get; set; }

        public string? Preferences { get; set; }

        public Guid CustomDesignId { get; set; }

        public Guid CustomComboId { get; set; }

        public Guid ArtistAvailabilityId { get; set; }
    }
}
