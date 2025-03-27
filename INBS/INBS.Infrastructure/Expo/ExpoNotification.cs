using INBS.Application.Interfaces;
using INBS.Infrastructure.Quartz.Jobs;
using MailKit;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System.Text;

namespace INBS.Infrastructure.Expo
{
    public class ExpoNotification(HttpClient httpClient) : IExpoNotification
    {
        public async Task<bool> SendPushNotificationAsync(string to, string title, string body, string screen)
        {
            var payload = new
            {
                to = $"ExponentPushToken[{to}]",
                title,
                body,
                sound = "default",
                data = new { screen }
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://exp.host/--/api/v2/push/send", content);
            return response.IsSuccessStatusCode;
        }

        public async Task SchedulePushNotification(string to, string title, string body, string screen, DateTime sendTime, Guid notificationId)
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            //create jobkey
            var jobkey = new JobKey($"{notificationId}");

            //check is jobkey existed, delete before create new (avoid duplicate)
            if (await scheduler.CheckExists(jobkey))
            {
                await scheduler.DeleteJob(jobkey);
            }

            var job = JobBuilder.Create<PushNotificationJob>()
                .UsingJobData("to", to)
                .UsingJobData("title", title)
                .UsingJobData("body", body)
                .UsingJobData("screen", screen)
                .Build();

            var trigger = TriggerBuilder.Create()
                .StartAt(sendTime)
                .WithSimpleSchedule(x => x.WithMisfireHandlingInstructionFireNow())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        public static async Task CancelScheduleNotification(Guid notificationId)
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            var jobkey = new JobKey($"{notificationId}");

            if (await scheduler.CheckExists(jobkey))
            {
                await scheduler.DeleteJob(jobkey);
            }
        }
    }
}
