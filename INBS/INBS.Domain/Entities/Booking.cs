using INBS.Domain.Entities;
using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public Booking() : base()
        {
        }
        public DateOnly ServiceDate { get; set; }

        public TimeOnly ServiceTime { get; set; }

        public long Duration { get; set; }

        public int Status { get; set; } 

        public long TotalAmount { get; set; }

        public int? PaymentMethod { get; set; } //Cash (0), Card (1)

        public string? Preferences { get; set; }

        public Guid CustomDesignId { get; set; }
        [ForeignKey(nameof(CustomDesignId))]
        [InverseProperty(nameof(CustomDesign.Bookings))]
        public virtual CustomDesign? CustomDesign { get; set; }

        public Guid CustomComboId { get; set; }
        [ForeignKey(nameof(CustomComboId))]
        [InverseProperty(nameof(CustomCombo.Bookings))]
        public virtual CustomCombo? CustomCombo { get; set; }

        public Guid ArtistId { get; set; }
        [ForeignKey(nameof(ArtistId))]
        [InverseProperty(nameof(Artist.Bookings))]
        public virtual Artist? Artist { get; set; }

        [InverseProperty(nameof(Cancellation.Booking))]
        public virtual Cancellation? Cancellation { get; set; }
    }
}
