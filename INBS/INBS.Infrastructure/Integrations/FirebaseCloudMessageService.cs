using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using INBS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INBS.Infrastructure.Integrations
{
    public class FirebaseCloudMessageService : IFirebaseCloudMessageService
    {
        private readonly FirebaseMessaging _firebaseMessaging;

        public FirebaseCloudMessageService(FirebaseMessaging firebaseMessaging)
        {
            _firebaseMessaging = firebaseMessaging;
        }

        public async Task<string> SendNotificationToDevice(string deviceToken, string title, string body)
        {
            try
            {
                var message = new Message
                {
                    Token = deviceToken,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Data = new Dictionary<string, string>
                {
                    { "click_action", "FLUTTER_NOTIFICATION_CLICK" },
                    { "custom_key", "custom_value" }
                }
                };

                string response = await _firebaseMessaging.SendAsync(message);
                Console.WriteLine($"✅ Sent message: {response}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to send message: {ex.Message}");
                throw;
            }
        }

        public async Task<BatchResponse> SendToMultipleDevices(List<string> deviceTokens, string title, string body)
        {
            try
            {
                var messages = new MulticastMessage
                {
                    Tokens = deviceTokens,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    }
                };
                if (deviceTokens.Count() <= 0)
                {
                    return null;
                }
                var response = await _firebaseMessaging.SendEachForMulticastAsync(messages);

                Console.WriteLine($"📤 Sent {response.SuccessCount} messages successfully.");
                Console.WriteLine($"❌ Failed {response.FailureCount} messages.");

                foreach (var (token, result) in deviceTokens.Zip(response.Responses))
                {
                    if (!result.IsSuccess)
                    {
                        Console.WriteLine($"⚠️ Token {token} failed: {result.Exception?.Message}");
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to send multicast messages: {ex.Message}");
                throw;
            }
        }
    }
}
