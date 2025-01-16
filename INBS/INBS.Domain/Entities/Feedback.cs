using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Feedback
    {
        public Feedback()
        {
            ID = Guid.NewGuid();
            Rating = 5;
            Content = string.Empty;
        }
        [Key]
        public Guid ID { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public Guid BookingId { get; set; }
        [ForeignKey(nameof(BookingId))]
        [InverseProperty(nameof(Booking.Feedbacks))]
        public virtual Booking? Booking { get; set; }
    }
}
