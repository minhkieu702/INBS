using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service
{
    public class CategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [JsonIgnore]
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;
    }
}
