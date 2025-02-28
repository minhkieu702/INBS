using INBS.Application.DTOs.User.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User.Customer
{
    public class CustomerResponse
    {
        [Key]
        public Guid ID { get; set; }

        public string? Preferences { get; set; }

        public UserResponse? User { get; set; }
    }
}
