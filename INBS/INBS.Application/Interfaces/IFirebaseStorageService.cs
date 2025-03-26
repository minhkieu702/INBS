using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);

        Task DeleteFileAsync(string imageUrl);
        //Task<BatchResponse> SendToMultipleDevices(List<string> deviceTokens, string title, string body);
        //Task<string> SendNotificationToDevice(string deviceToken, string title, string body);
    }
}
