using INBS.Application.DTOs.Payment;
using INBS.Application.DTOs.PaymentDetail;
using INBS.Application.DTOs.PayOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IPaymentService
    {
        Task<string> CreatePayOSUrl(PaymentRequest paymentRequest, IList<PaymentDetailRequest> paymentDetailRequests);
        IQueryable<PaymentResponse> Get();
        Task Delete(int id);
        Task CreatePaymentForCash(PaymentRequest paymentRequest, IList<PaymentDetailRequest> paymentDetailRequests);
        Task ConfirmWebHook(WebhookBody webhookBody);
        Task ReturnUrl(long orderCode, bool cancel);
    }
}
