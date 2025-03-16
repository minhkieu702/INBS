using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Payment() : base()
        {
            PaymentDetails = [];
        }

        public int Method { get; set; }

        public long TotalAmount { get; set; }

        public int Status { get; set; }

        [InverseProperty(nameof(PaymentDetail.Payment))]
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
