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
        Task IFirebaseService.DeleteImageAsync(string imageUrl)
        {
            throw new NotImplementedException();
        }

        Task<string> IFirebaseService.UploadImageAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
