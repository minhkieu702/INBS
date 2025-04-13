using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Artist
{
    public class ArtistCertificateRequest
    {
        public int NumerialOrder { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public IFormFile? NewImage { get; set; } 

        public string? ImageUrl { get; set; }  
    }
}
