using INBS.Application.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface INotificationService
    {
        IQueryable<NotificationResponse> Get();
        Task MarkSeenNotification();
        Task SendNotificationAsync(string phoneNumber, string message);
    }
}
