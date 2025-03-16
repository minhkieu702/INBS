using INBS.Application.DTOs.Payment;
using INBS.Application.DTOs.PaymentDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IPaymentService
    {
        Task Create(PaymentRequest paymentRequest, IList<PaymentDetailRequest> paymentDetailRequests);
        Task Update(Guid id, PaymentRequest paymentRequest, IList<PaymentDetailRequest> paymentDetailRequests);
        IQueryable<PaymentResponse> Get();
        Task Delete(Guid id);
    }
}
