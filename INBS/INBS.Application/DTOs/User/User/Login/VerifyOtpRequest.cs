using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User.User.Login
{
    public class VerifyOtpRequest
    {
        public string PhoneNumber { get; set; }
        public string OtpCode { get; set; }
    }
}
