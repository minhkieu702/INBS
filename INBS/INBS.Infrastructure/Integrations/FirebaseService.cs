using Firebase.Auth;
using Firebase.Storage;
using INBS.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace INBS.Infrastructure.Integrations
{
    public class FirebaseService : IFirebaseService
    {
        // Lấy các biến môi trường
        private readonly string _apiKey = Environment.GetEnvironmentVariable("FirebaseSettings:apiKey") ?? string.Empty;
        private readonly string _storage = Environment.GetEnvironmentVariable("FirebaseSettings:storage") ?? string.Empty;
        private readonly string _appId = Environment.GetEnvironmentVariable("FirebaseSettings:email") ?? string.Empty;
        private readonly string _projectId = Environment.GetEnvironmentVariable("FirebaseSettings:password") ?? string.Empty;

        async Task IFirebaseService.DeleteFileAsync(string imageUrl)
        {
            try
            {
                // Xác thực Firebase
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(_appId, _projectId);

                // Lấy tên file từ URL (Firebase Storage format: https://firebasestorage.googleapis.com/...)
                var fileName = ExtractFileNameFromUrl(imageUrl);

                var storage = new FirebaseStorage(_storage, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                });

                // Xóa file trên Firebase Storage
                await storage.Child(fileName).DeleteAsync();
            }
            catch (Exception ex)
            {
                // Bắt và ghi log lỗi
                Console.WriteLine($"Error deleting file from Firebase Storage: {ex.Message}");
                throw new Exception("Failed to delete file from Firebase Storage.", ex);
            }
        }

        private static string ExtractFileNameFromUrl(string imageUrl)
        {
            try
            {
                // Tách file name từ URL
                var uri = new Uri(imageUrl);
                var pathSegments = uri.AbsolutePath.Split('/');
                return string.Join("/", pathSegments.Skip(2)); // Bỏ phần đầu URL để lấy đường dẫn file trên Firebase Storage
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid file URL format.", ex);
            }
        }

        async Task<string> IFirebaseService.UploadFileAsync(IFormFile file)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(_appId, _projectId);

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
    }

}