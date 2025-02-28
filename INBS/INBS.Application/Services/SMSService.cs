using INBS.Application.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace INBS.Application.Services
{
    public class SMSService : ISMSService
    {
        private readonly string _accountSid = Environment.GetEnvironmentVariable("Twilio:AccountSid") ?? string.Empty;
        private readonly string _authToken = Environment.GetEnvironmentVariable("Twilio:AuthToken") ?? string.Empty;
        private readonly string _fromNumber = Environment.GetEnvironmentVariable("Twilio:FromNumber") ?? string.Empty;

        public async Task SendOtpSmsAsync(string toNumber, string otp)
        {
            TwilioClient.Init(_accountSid, _authToken);

            var message = await MessageResource.CreateAsync(
                to: new PhoneNumber(toNumber),
                from: new PhoneNumber(_fromNumber),
                body: $"Your OTP is: {otp}"
            );
        }
        public string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
