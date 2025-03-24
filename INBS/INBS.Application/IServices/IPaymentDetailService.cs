using INBS.Application.DTOs.PaymentDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IPaymentDetailService
    {
        IQueryable<PaymentDetailResponse> Get();
    }
}
