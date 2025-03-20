using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.Payment;
using INBS.Application.DTOs.PaymentDetail;
using INBS.Application.DTOs.PayOS;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class PaymentService(IPayOSHandler _payOS, IUnitOfWork _unitOfWork, IMapper _mapper) : IPaymentService
    {
        private async Task<IEnumerable<Booking>> ValidatePaymentRequest(IEnumerable<Guid> bookingIds)
        {
            var bookings = await _unitOfWork.BookingRepository.GetAsync(query 
                => query.Where(c => !c.IsDeleted && c.Status == (int)BookingStatus.isServing && bookingIds.Contains(c.ID)));
            if (bookings.Count() != bookingIds.Count())
            {
                throw new Exception("Some booking is not ready to serve");
            }

            return bookings;
        }

        private async Task DeletePaymentDetails(IEnumerable<Guid> bookingIds)
        {

            var oldPaymentDetails = await _unitOfWork.PaymentDetailRepository.GetAsync(c => c.Where(pd => bookingIds.Contains(pd.BookingId)).Include(c => c.Booking).AsNoTracking());
            if (oldPaymentDetails.Any())
            {
                if (oldPaymentDetails.Any(c => c.Booking!=null && !c.Booking.IsDeleted && c.Booking.Status != (int)BookingStatus.isServing))
                {
                    throw new Exception("Some booking are not ready to pay");
                }
                _unitOfWork.PaymentDetailRepository.DeleteRange(oldPaymentDetails);
            }
        }

        private async Task HandlePaymentDetail(int id, IList<PaymentDetailRequest> paymentDetailRequests)
        {
            var paymentDetails = _mapper.Map<IList<PaymentDetail>>(paymentDetailRequests).ToList();

            paymentDetails.ForEach(c => c.PaymentId = id);

            await _unitOfWork.PaymentDetailRepository.InsertRangeAsync(paymentDetails);
        }
        public static int GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999);
        }

        public async Task<string> CreatePayOSUrl(PaymentRequest paymentRequest, IList<PaymentDetailRequest> paymentDetailRequests)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                await DeletePaymentDetails(bookingIds: paymentDetailRequests.Select(c => c.BookingId));

                var payment = new Payment
                {
                    Method = (int)PaymentMethod.QRCode,

                    Status = (int)PaymentStatus.Pending
                };

                var bookings = await ValidatePaymentRequest(paymentDetailRequests.Select(c => c.BookingId));

                var totalAmount = bookings.Sum(c => c.TotalAmount);

                payment.TotalAmount = totalAmount;

                await _unitOfWork.PaymentRepository.InsertAsync(payment);

                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("Something was wrong");
                }

                await HandlePaymentDetail(payment.ID, paymentDetailRequests);
                
                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("Something was wrong");
                }
                
                var payOSUrl = await _payOS.GetPaymentLinkAsync(payment.ID, (int)totalAmount, "", bookings.ToList());

                _unitOfWork.CommitTransaction();

                return payOSUrl;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private async Task UpdateBookingStatus(IEnumerable<Guid> bookingIds, int status)
        {
            var bookings = (await _unitOfWork.BookingRepository.GetAsync(query => query.Where(c => !c.IsDeleted && c.Status == (int)BookingStatus.isServing && bookingIds.Contains(c.ID)))).ToList();
            if(bookings.Count != bookingIds.Count())
            {
                throw new Exception("Some booking is not existed");
            }
            bookings.ForEach(c => c.Status = status);
            _unitOfWork.BookingRepository.UpdateRange(bookings);
        }

        public async Task CreatePaymentForCash(PaymentRequest paymentRequest, IList<PaymentDetailRequest> paymentDetailRequests)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                await DeletePaymentDetails(bookingIds: paymentDetailRequests.Select(c => c.BookingId));

                var payment = _mapper.Map<Payment>(paymentRequest);
                
                payment.Method = (int)PaymentMethod.Cash;
                payment.Status = (int)PaymentStatus.Success;

                var totalAmount = await ValidatePaymentRequest(paymentDetailRequests.Select(c => c.BookingId));
                
                await UpdateBookingStatus(paymentDetailRequests.Select(c => c.BookingId), (int)BookingStatus.isCompleted);

                await _unitOfWork.PaymentRepository.InsertAsync(payment);
                                
                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("Something was wrong");
                }

                await HandlePaymentDetail(payment.ID, paymentDetailRequests);

                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("Something was wrong");
                }

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private async Task RemovePayment(long id)
        {
            var payments = await _unitOfWork.PaymentRepository.GetAsync(query 
                => query.Where(c => id == c.ID)
                .Include(c => c.PaymentDetails)
                .AsNoTracking()
                );

            var payment = payments.FirstOrDefault() ?? throw new Exception("Payment not found");

            //var deletePaymentDetails = new List<PaymentDetail>();
            //var deletePayment = new List<Payment>();

            //foreach (var p in payment)
            //{
            //    deletePayment.Add(p);
            //    deletePaymentDetails.AddRange(p.PaymentDetails);
            //}

            //_unitOfWork.PaymentDetailRepository.DeleteRange(deletePaymentDetails);

            payment.Status = (int)PaymentStatus.Failed;

            _unitOfWork.PaymentRepository.Update(payment);
        }

        private async Task AcceptPayment(long id)
        {
            var payment = (await _unitOfWork.PaymentRepository.GetAsync(query
                => query.Where(c => id == c.ID)
                .Include(c => c.PaymentDetails)
                    .ThenInclude(c=>c.Booking))
                ).FirstOrDefault() ?? throw new Exception("Payment not found");

            payment.Status = (int)PaymentStatus.Success;

            var bookings = payment.PaymentDetails.Select(c => c.Booking).ToList();
            
            bookings.ForEach(c => c!.Status = (int)BookingStatus.isCompleted);

            _unitOfWork.BookingRepository.UpdateRange(bookings!);

            await _unitOfWork.PaymentRepository.UpdateAsync(payment);
        }

        public async Task ConfirmWebHook(WebhookBody webhookBody)
        {
            try
            {
                _unitOfWork.BeginTransaction(); 
                switch (webhookBody.Success)
                {
                    case true:
                        await AcceptPayment(webhookBody.Data!.OrderCode);
                        break;
                    case false:
                        await RemovePayment(webhookBody.Data!.OrderCode);
                        break;
                    default:
                }
                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("This action failed");
                }

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task ReturnUrl(long orderCode, bool cancel)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                switch (cancel)
                {
                    case true:
                        await AcceptPayment(orderCode);
                        break;
                    case false:
                        await RemovePayment(orderCode);
                        break;
                    default:
                }
                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("This action failed");
                }

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                await RemovePayment(id);

                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("This action failed");
                }

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public IQueryable<PaymentResponse> Get()
        {
            try
            {
                return _unitOfWork.PaymentRepository.Query().ProjectTo<PaymentResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
