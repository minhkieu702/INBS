using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Common.Preference
{
    public class PreferenceResponse
    {
        [Key]
        public int ID { get; set; }
        public Guid CustomerId { get; set; }
        public Guid DesignId { get; set; }
        public int PreferenceId { get; set; }
        public string PreferenceType { get; set; } = string.Empty;

        public object? Data { get; set; }
    }
}
