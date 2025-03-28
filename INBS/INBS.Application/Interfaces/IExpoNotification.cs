using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Interfaces
{
    public interface IExpoNotification
    {
        Task SchedulePushNotification(string to, string title, string body, string screen, DateTime sendTime, Guid noficationId);
        Task<bool> SendPushNotificationAsync(string to, string title, string body, string screen);
    }
}
