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
    public class Feedback : BaseEntity
    {
        public Feedback() : base()
        {
            Rating = 5;
            Content = string.Empty;
        }
        public int Rating { get; set; }
        public string Content { get; set; }
        public Guid BookingId { get; set; }
        [ForeignKey(nameof(BookingId))]
        [InverseProperty(nameof(Booking.Feedbacks))]
        public virtual Booking? Booking { get; set; }
    }
}
