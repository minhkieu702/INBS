using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class PaymentDetail
    {
        public Guid PaymentId { get; set; }
        [ForeignKey(nameof(PaymentId))]
        [InverseProperty(nameof(Payment.PaymentDetails))]
        public virtual Payment? Payment { get; set; }

        public Guid BookingId { get; set; }
        [ForeignKey(nameof(BookingId))]
        [InverseProperty(nameof(Booking.PaymentDetails))]
        public virtual Booking? Booking { get; set; }
    }
}
