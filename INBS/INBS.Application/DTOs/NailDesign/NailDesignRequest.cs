using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.NailDesign
{
    public class NailDesignRequest
    {
        public IFormFile? NewImage { get; set; }

        public string? ImageUrl { get; set; }

        public int NailPosition { get; set; } //4, 8, 12, 16, 20

        public bool IsLeft { get; set; } //true - left, false - right

        public IList<NailDesignServiceRequest> NailDesignServices { get; set; } = [];
    }
}
