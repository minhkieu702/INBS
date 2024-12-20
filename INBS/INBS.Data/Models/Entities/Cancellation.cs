using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBS.Data.Models.Entities
{
    public class Cancellation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string? Reason { get; set; }

        public DateTime CancelledAt { get; set; }

        public Guid BookingId { get; set; }
        [ForeignKey(nameof(BookingId))]
        [InverseProperty(nameof(Booking.Cancellation))]
        public virtual Booking? Booking { get; set; }
    }
}
