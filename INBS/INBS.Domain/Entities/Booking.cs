using INBS.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace INBS.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public Booking() : base()
        {
            Cancellations = [];
            PaymentDetails = [];
        }
        public DateOnly ServiceDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly PredictEndTime { get; set; }

        public int Status { get; set; } 

        public long TotalAmount { get; set; }

        public Guid CustomerSelectedId { get; set; }
        [ForeignKey(nameof(CustomerSelectedId))]
        [InverseProperty(nameof(CustomerSelected.Bookings))]
        public virtual CustomerSelected? CustomerSelected { get; set; }

        public Guid ArtistStoreId { get; set; }
        [ForeignKey(nameof(ArtistStoreId))]
        [InverseProperty(nameof(ArtistStore.Bookings))]
        public virtual ArtistStore? ArtistStore { get; set; }

        [InverseProperty(nameof(Cancellation.Booking))]
        public virtual ICollection<Cancellation> Cancellations { get; set; }

        [InverseProperty(nameof(PaymentDetail.Booking))]
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
