using INBS.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Data.Models.Entities
{
    public class Booking
    {
        public Booking()
        {
            ID = Guid.NewGuid();
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

        public Guid ArtistId { get; set; }
        [ForeignKey(nameof(ArtistId))]
        [InverseProperty(nameof(Artist.Bookings))]
        public virtual Artist? Artist { get; set; }

        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.Bookings))]
        public virtual Customer? Customer { get; set; }


        [InverseProperty(nameof(Cancellation.Booking))]
        public virtual Cancellation? Cancellation { get; set; }
    }
}
