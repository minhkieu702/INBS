using INBS.Application.DTOs.Booking;
using INBS.Application.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.PaymentDetail
{
    public class PaymentDetailResponse
    {
        [Key]
        public string PaymentId { get; set; }
        public virtual PaymentResponse? Payment { get; set; }

        [Key]
        public Guid BookingId { get; set; }
        public virtual BookingResponse? Booking { get; set; }
    }
}
