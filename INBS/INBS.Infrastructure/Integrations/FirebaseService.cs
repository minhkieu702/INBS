using Firebase.Auth;
using Firebase.Storage;
using INBS.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Infrastructure.Integrations
{
    public class FirebaseService : IFirebaseService
    {
        private readonly string _apiKey = Environment.GetEnvironmentVariable("FirebaseSettings:apiKey") ?? string.Empty;
        private readonly string _storage = Environment.GetEnvironmentVariable("FirebaseSettings:storage") ?? string.Empty;
        private readonly string _appId = Environment.GetEnvironmentVariable("FirebaseSettings:email") ?? string.Empty;
        private readonly string _projectId = Environment.GetEnvironmentVariable("FirebaseSettings:password") ?? string.Empty;

        private readonly Lazy<Task<FirebaseAuthLink>> _firebaseAuth;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(5); // Giới hạn 5 tệp tải lên cùng lúc

        public FirebaseService()
        {
            _firebaseAuth = new Lazy<Task<FirebaseAuthLink>>(async () =>
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
                return await auth.SignInWithEmailAndPasswordAsync(_appId, _projectId);
            });
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl))
                {
                    throw new ArgumentException("Invalid file URL");
                }

                // Lấy tên file từ URL (Firebase Storage format: https://firebasestorage.googleapis.com/...)
                var fileName = ExtractFileNameFromUrl(fileUrl);

                var storage = new FirebaseStorage(_storage, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = async () =>
                    {
                        var auth = await _firebaseAuth.Value;
                        return auth.FirebaseToken;
                    },
                    ThrowOnCancel = true
                });

                await storage.Child(fileName).DeleteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting file from Firebase Storage: " + ex);
            }
        }

        private string ExtractFileNameFromUrl(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var pathSegments = uri.AbsolutePath.Split('/');
            return string.Join("/", pathSegments.Skip(2)); // Bỏ phần đầu URL để lấy đường dẫn file trên Firebase Storage
        }


        public async Task<string> UploadFileAsync(IFormFile file)
        {
            await _semaphore.WaitAsync();
            try
            {
                var auth = await _firebaseAuth.Value;
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                string folderName = GetFolderName(Path.GetExtension(file.FileName));

                var storage = new FirebaseStorage(_storage, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken),
                    ThrowOnCancel = true
                });

                using (var stream = file.OpenReadStream())
                {
                    var storageReference = storage.Child(folderName).Child(fileName);
                    await storageReference.PutAsync(stream);
                    return await storageReference.GetDownloadUrlAsync();
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private string GetFolderName(string fileExtension)
        {
            return fileExtension.ToLower() switch
            {
                ".jpg" or ".jpeg" or ".png" => "images",
                ".docx" => "docx",
                ".ppt" or ".pptx" => "ppt",
                ".mp4" or ".mov" => "videos",
                _ => "other"
            };
        }
    }

}
