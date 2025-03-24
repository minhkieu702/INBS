using System.ComponentModel.DataAnnotations.Schema;

namespace INBS.Domain.Entities
{
    public class PaymentDetail
    {
        public string PaymentId { get; set; } = string.Empty;
        [ForeignKey(nameof(PaymentId))]
        [InverseProperty(nameof(Payment.PaymentDetails))]
        public virtual Payment? Payment { get; set; }

        public Guid BookingId { get; set; }
        [ForeignKey(nameof(BookingId))]
        [InverseProperty(nameof(Booking.PaymentDetails))]
        public virtual Booking? Booking { get; set; }
    }
}
