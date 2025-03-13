using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Customer
{
    public class DeviceTokenResponse
    {
        [Key]
        public int ID { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public Guid CustomerId { get; set; }
        public virtual CustomerResponse? Customer { get; set; }
    }
}
