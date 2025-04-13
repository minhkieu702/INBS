using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Artist
{
    public class ArtistCertificateResponse
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
