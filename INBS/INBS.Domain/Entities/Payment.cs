using INBS.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INBS.Domain.Entities
{
    public class Payment
    {
        public Payment()
        {
            ID = string.Empty;
            PaymentDetails = [];
            Status = (int)PaymentStatus.Pending;
        }

        [Key]
        public string ID { get; set; }

        public int Method { get; set; }

        public long TotalAmount { get; set; }

        public int Status { get; set; }

        [InverseProperty(nameof(PaymentDetail.Payment))]
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
