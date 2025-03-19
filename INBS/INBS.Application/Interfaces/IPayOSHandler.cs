using INBS.Application.DTOs.PayOS;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Interfaces
{
    public interface IPayOSHandler
    {
        Task<string> GetPaymentLinkAsync(int orderId, int amount, string description, IList<Booking> bookings);
    }
}
