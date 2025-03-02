using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User.Customer
{
    public class CustomerPreferenceResponse
    {
        [Key]
        public Guid CustomerId { get; set; }
        [Key]
        public int PreferenceId { get; set; }
        [Key]
        public string PreferenceType { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
