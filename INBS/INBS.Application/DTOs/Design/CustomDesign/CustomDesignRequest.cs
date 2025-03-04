using INBS.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.CustomDesign
{
    public class CustomDesignRequest
    {
        public TimeOnly? StartAt { get; set; }

        public TimeOnly? FinishAt { get; set; }

        public bool IsSave { get; set; } = false;

        public string? ImageUrl { get; set; } = Constants.DEFAULT_IMAGE_URL;

        public IFormFile? NewImage { get; set; }

        public Guid DesignId { get; set; }
    }
}
