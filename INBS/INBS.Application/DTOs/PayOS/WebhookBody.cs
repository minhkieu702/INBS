using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.PayOS
{
    public class WebhookBody
    {
        public string Code { get; set; } = "";

        public string Desc { get; set; } = "";

        public bool Success { get; set; }

        public PayOSData? Data { get; set; }

        public string Signature { get; set; } = "";
    }

    public class PayOSData
    {
        public int OrderCode { get; set; }

        public int Amount { get; set; }

        public string Description { get; set; } = "";

        public string AccountNumber { get; set; } = "";

        public string Currency { get; set; } = "";

        public string PaymentLinkId { get; set; } = "";

        public string Code { get; set; } = "";

        public string Desc { get; set; } = "";
    }
}
