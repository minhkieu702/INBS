using INBS.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Artist
{
    public class ArtistRequest
    {
        public int Level { get; set; }

        public int YearsOfExperience { get; set; }

        public IList<ArtistCertificateRequest> Certificates { get; set; } 
    }
}
