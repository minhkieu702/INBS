using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.NailDesignServiceGuide
{
    public class NailDesignServiceGuideResponse
    {
        public Guid NailDesignServiceGuideId { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
    }
}
