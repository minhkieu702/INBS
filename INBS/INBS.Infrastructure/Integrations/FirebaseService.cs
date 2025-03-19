using Firebase.Auth;
using Firebase.Storage;
using INBS.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;

namespace INBS.Infrastructure.Integrations
{
    public class FirebaseService(FirebaseMessaging _firebaseMessaging) : IFirebaseService
    {
        // Lấy các biến môi trường
        private readonly string _apiKey = Environment.GetEnvironmentVariable("FirebaseSettings:apiKey") ?? string.Empty;
        private readonly string _storage = Environment.GetEnvironmentVariable("FirebaseSettings:storage") ?? string.Empty;
        private readonly string _email = Environment.GetEnvironmentVariable("FirebaseSettings:email") ?? string.Empty;
        private readonly string _password = Environment.GetEnvironmentVariable("FirebaseSettings:password") ?? string.Empty;

        public async Task<string> SendNotificationToDevice(string deviceToken, string title, string body)
        {
            var message = new Message
            {
                Token = deviceToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };
            return await _firebaseMessaging.SendAsync(message);
        }

        public async Task<BatchResponse> SendToMultipleDevices(List<string> deviceTokens, string title, string body)
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
            return await _firebaseMessaging.SendEachForMulticastAsync(messages);
        }
        
        //private static string ExtractFileNameFromUrl(string imageUrl)
        //{
        //    try
        //    {
        //        // Tách file name từ URL
        //        var uri = new Uri(imageUrl);
        //        var pathSegments = uri.AbsolutePath.Split('/');
        //        return string.Join("/", pathSegments.Skip(2)); // Bỏ phần đầu URL để lấy đường dẫn file trên Firebase Storage
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Invalid file URL format.", ex);
        //    }
        //}

        async Task<string> IFirebaseService.UploadFileAsync(IFormFile file)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(_email, _password);

                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var fileExtension = Path.GetExtension(file.FileName);
                string folderName = fileExtension switch
                {
                    ".jpg" or ".jpeg" or ".png" => "images",
                    ".docx" => "docx",
                    ".ppt" or ".pptx" => "ppt",
                    ".mp4" or ".mov" => "videos",
                    _ => "other",
                };
                var storage = new FirebaseStorage(_storage, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                });

                using var stream = file.OpenReadStream();
                var storageReference = storage.Child(folderName).Child(fileName);
                await storageReference.PutAsync(stream);

                return await storageReference.GetDownloadUrlAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task DeleteFileAsync(string imageUrl)
        {
            throw new NotImplementedException();
        }
    }
}