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
        public bool IsSave { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile NewImage { get; set; }

        public Guid DesignId { get; set; }
    }
}
