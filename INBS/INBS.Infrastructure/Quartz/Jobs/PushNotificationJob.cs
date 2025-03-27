using INBS.Application.Interfaces;
using INBS.Infrastructure.Expo;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System.Text;

namespace INBS.Infrastructure.Quartz.Jobs
{
    public class PushNotificationJob(ILogger<PushNotificationJob> logger) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var to = context.JobDetail.JobDataMap.GetString("to");
            var title = context.JobDetail.JobDataMap.GetString("title");
            var body = context.JobDetail.JobDataMap.GetString("body");
            var screen = context.JobDetail.JobDataMap.GetString("screen");

            if (string.IsNullOrEmpty(to)) return;

            using (var client = new HttpClient())
            {
                var payload = new
                {
                    to,
                    title,
                    body,
                    sound = "default",
                    data = new { screen }
                };

                var json = JsonConvert.SerializeObject(payload);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://exp.host/--/api/v2/push/send", content);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError("Send notification failed");
                }
            }
        }
    }
}
