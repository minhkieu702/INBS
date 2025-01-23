using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.Preference
{
    public class DesignPreferenceResponse
    {
        [Key]
        public Guid DesignId { get; set; }
        [Key]
        public int PreferenceId { get; set; }
        [Key]
        public string PreferenceType { get; set; } = string.Empty;

        public object? Data { get; set; }
    }
}
