using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.User;

namespace INBS.Application.DTOs.DeviceToken
{
    public class DeviceTokenResponse
    {
        [Key]
        public int ID { get; set; }

        public string Token { get; set; } = string.Empty;

        public string Platform { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime LastActiveAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }
        public virtual UserResponse? User { get; set; }
    }
}
