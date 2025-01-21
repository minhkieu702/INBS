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
        async Task IFirebaseService.DeleteImageAsync(string imageUrl)
        {
            
        }

        async Task<string> IFirebaseService.UploadFileAsync(IFormFile file)
        {
            try
            {
                var _apiKey = Environment.GetEnvironmentVariable("FirebaseSettings:apiKey");
                var _storage = Environment.GetEnvironmentVariable("FirebaseSettings:storage");
                var _appId = Environment.GetEnvironmentVariable("FirebaseSettings:email");
                var _projectId = Environment.GetEnvironmentVariable("FirebaseSettings:password");

                var auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(_appId, _projectId);

                var fileName = $"{Guid.NewGuid()}_{file.FileName}";

                string folderName;

                var fileExtension = Path.GetExtension(file.FileName);

                switch (fileExtension)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                        folderName = "images";
                        break;
                    case ".docx":
                        folderName = "docx";
                        break;
                    case ".ppt":
                    case ".pptx":
                        folderName = "ppt";
                        break;
                    case ".mp4":
                    case ".mov":
                        folderName = "videos";
                        break;
                    default:
                        folderName = "other";
                        break;
                }

                var storage = new FirebaseStorage(_storage, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                });

                using (var stream = file.OpenReadStream())
                {
                    var storageReference = storage.Child(folderName).Child(fileName);
                    await storageReference.PutAsync(stream);

                    return await storageReference.GetDownloadUrlAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
