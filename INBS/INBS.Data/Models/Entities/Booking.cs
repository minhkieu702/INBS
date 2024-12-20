using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBS.Data.Models.Entities
{
    public class Booking
    {
        public Booking()
        {
            ID = Guid.NewGuid();
            UserBookings = [];
        }

        [Key]
        public Guid ID { get; set; }

        public DateTime ServiceDate { get; set; }

        public long Duration { get; set; }

        public int Status { get; set; }

        public long TotalAmount { get; set; }

        public int PaymentMethod { get; set; }

        public string? Preferences { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(Design.Bookings))]
        public virtual NailDesign? Design { get; set; }

        [InverseProperty(nameof(UserBooking.Booking))]
        public virtual ICollection<UserBooking> UserBookings { get; set; }

        public virtual Cancellation? Cancellation { get; set; }
    }
}
