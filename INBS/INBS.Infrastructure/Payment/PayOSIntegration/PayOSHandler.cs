using INBS.Application.DTOs.PayOS;
using INBS.Application.Interfaces;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Infrastructure.Payment.PayOSIntegration
{
    public class PayOSHandler : IPayOSHandler
    {
        public async Task<string> GetPaymentLinkAsync(long orderId, int amount, string description, IList<Booking> bookings)
        {
            var PAYOS_CLIENT_ID = Environment.GetEnvironmentVariable("PAYOS_CLIENT_ID");
            var PAYOS_API_KEY = Environment.GetEnvironmentVariable("PAYOS_API_KEY");
            var PAYOS_CHECKSUM_KEY = Environment.GetEnvironmentVariable("PAYOS_CHECKSUM_KEY");
            var PAYOS_RETURN_URL = Environment.GetEnvironmentVariable("PAYOS_RETURN_URL");
            var PAYOS_CANCEL_URL = Environment.GetEnvironmentVariable("PAYOS_CANCEL_URL");

            if (string.IsNullOrEmpty(PAYOS_CLIENT_ID) || string.IsNullOrEmpty(PAYOS_API_KEY) || string.IsNullOrEmpty(PAYOS_CHECKSUM_KEY) || string.IsNullOrEmpty(PAYOS_CANCEL_URL) || string.IsNullOrEmpty(PAYOS_RETURN_URL))
            {
                throw new Exception("PayOS configuration is missing");
            }

            var payOS = new PayOS(PAYOS_CLIENT_ID, PAYOS_API_KEY, PAYOS_CHECKSUM_KEY);

            var itemData = new List<ItemData>();

            foreach (var booking in bookings)
            {
                itemData.Add(new ItemData(booking.ID.ToString(), 1, (int)booking.TotalAmount));
            }
            var expired = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds();
            var paymentLink = await payOS.createPaymentLink(new PaymentData(orderId, amount, description, itemData, PAYOS_RETURN_URL, PAYOS_CANCEL_URL, "", "", "", "", "", expired));

            return paymentLink.checkoutUrl;
        }

        public async Task GetAsync(int orderId)
        {
            try
            {
                var PAYOS_CLIENT_ID = Environment.GetEnvironmentVariable("PAYOS_CLIENT_ID");
                var PAYOS_API_KEY = Environment.GetEnvironmentVariable("PAYOS_API_KEY");
                var PAYOS_CHECKSUM_KEY = Environment.GetEnvironmentVariable("PAYOS_CHECKSUM_KEY");
                var PAYOS_RETURN_URL = Environment.GetEnvironmentVariable("PAYOS_RETURN_URL");
                var PAYOS_CANCEL_URL = Environment.GetEnvironmentVariable("PAYOS_CANCEL_URL");

                if (string.IsNullOrEmpty(PAYOS_CLIENT_ID) || string.IsNullOrEmpty(PAYOS_API_KEY) || string.IsNullOrEmpty(PAYOS_CHECKSUM_KEY) || string.IsNullOrEmpty(PAYOS_CANCEL_URL) || string.IsNullOrEmpty(PAYOS_RETURN_URL))
                {
                    throw new Exception("PayOS configuration is missing");
                }

                var payOS = new PayOS(PAYOS_CLIENT_ID, PAYOS_API_KEY, PAYOS_CHECKSUM_KEY);

                var detail = await payOS.getPaymentLinkInformation(orderId);
                detail.Deconstruct(out string id, out long orderCode, out int amount, out int amountPaid, out int amountRemaining, out string status, out string createdAt, out List<Transaction> transactions, out string? canceledAt, out string? cancellationReason);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
