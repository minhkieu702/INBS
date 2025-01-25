using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.Image
{
    public class NewImageRequest
    {
        public IFormFile? Image { get; set; }
        public int NumerialOrder { get; set; }
        public string? Description { get; set; }
    }
}
