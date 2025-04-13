using INBS.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.FeedbackImage
{
    public class FeedbackImageRequest
    {
        public IFormFile? NewImage { get; set; }
        public string? ImageUrl { get; set; }
        public MediaType MediaType { get; set; }
        public int NumerialOrder { get; set; }
    }
}
