using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Identity.Client;

namespace INBS.Infrastructure.Quartz.Jobs
{
    public class SendSmsNotificationJob : IJob
    {
        private readonly ILogger<SendSmsNotificationJob> _logger;
        private readonly IConfiguration _configuration;

        public SendSmsNotificationJob(ILogger<SendSmsNotificationJob> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var accountSid = Environment.GetEnvironmentVariable("Twilio:AccountSid");
            var authToken = Environment.GetEnvironmentVariable("Twilio:AuthToken");
            var fromNumber = Environment.GetEnvironmentVariable("Twilio:FromNumber");

            var phoneNumber = context.JobDetail.JobDataMap.GetString("PhoneNumber");
            var message = context.JobDetail.JobDataMap.GetString("Message");

            try
            {
                TwilioClient.Init(accountSid, authToken);

                var messageResponse = await MessageResource.CreateAsync(
                    to: new PhoneNumber(phoneNumber),
                    from: new PhoneNumber(fromNumber),
                    body: message
                );

                _logger.LogInformation($"SMS sent to {phoneNumber}. SID: {messageResponse.Sid}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send SMS to {phoneNumber}: {ex.Message}");
            }
        }
    }
}
