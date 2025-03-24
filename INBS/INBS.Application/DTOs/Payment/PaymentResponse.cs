using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.PaymentDetail;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Payment
{
    public class PaymentResponse
    {
        [Key]
        public string ID { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;

        public long TotalAmount { get; set; }

        public string Status { get; set; } = string.Empty;

        public virtual ICollection<PaymentDetailResponse> PaymentDetails { get; set; } = [];
    }
}
