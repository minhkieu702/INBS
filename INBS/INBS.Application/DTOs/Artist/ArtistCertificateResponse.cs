using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Artist
{
    public class ArtistCertificateResponse
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ArtistId { get; set; }

        public int NumerialOrder { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
    }
}
