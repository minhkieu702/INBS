using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBS.Data.Models.Entities
{
    public class UserBooking
    {
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.UserBookings))]
        public virtual User? User { get; set; }

        public Guid BookingId { get; set; }
        [ForeignKey(nameof(BookingId))]
        [InverseProperty(nameof(Booking.UserBookings))]
        public virtual Booking? Booking { get; set; }

        public bool IsArtist { get; set; }
    }
}
