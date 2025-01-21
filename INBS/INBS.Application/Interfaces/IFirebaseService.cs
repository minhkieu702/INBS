using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> UploadImageAsync(IFormFile file);

        Task DeleteImageAsync(string imageUrl);
    }
}
