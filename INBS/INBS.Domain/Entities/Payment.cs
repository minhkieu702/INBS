using INBS.Domain.Entities.Common;
using INBS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Payment
    {
        public Payment()
        {
            PaymentDetails = [];
            Status = (int)PaymentStatus.Pending;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public int Method { get; set; }

        public long TotalAmount { get; set; }

        public int Status { get; set; }

        [InverseProperty(nameof(PaymentDetail.Payment))]
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
