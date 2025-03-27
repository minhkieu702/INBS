using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Interfaces
{
    public interface IFirebaseCloudMessageService
    {
        Task<string> SendNotificationToDevice(string deviceToken, string title, string body);
        Task<BatchResponse> SendToMultipleDevices(List<string> deviceTokens, string title, string body);
    }
}
