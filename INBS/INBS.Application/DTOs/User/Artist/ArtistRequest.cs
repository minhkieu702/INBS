using INBS.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User.Artist
{
    public class ArtistRequest
    {
        public string FullName { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public string ImageUrl { get; set; } = Constants.DEFAULT_IMAGE_URL;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid StoreId { get; set; }
        public IList<Guid> DesignIds { get; set; } = [];
        public IList<Guid> ServiceIds { get; set; } = [];
    }
}
