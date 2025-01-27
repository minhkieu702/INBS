using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Booking.CustomNailDesign
{
    public class CustomNailDesignRequest
    {
        public string? ImageUrl { get; set; }

        public IFormFile? CustomImage { get; set; }

        public int NailPosition { get; set; }

        public bool IsLeft { get; set; }

        public IList<Guid> AccessoryIds { get; set; } = [];
    }
}
