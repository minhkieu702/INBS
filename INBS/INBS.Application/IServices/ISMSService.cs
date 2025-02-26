using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface ISMSService
    {
        Task SendOtpSmsAsync(string toNumber, string otp);
        string GenerateOtp();
    }
}
