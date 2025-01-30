using INBS.Application.DTOs.Service.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.CustomCombo
{
    public class ServiceCustomComboResponse
    {
        [Key]
        public Guid CustomComboId { get; set; }
        public CustomComboResponse? CustomCombo { get; set; }
        [Key]
        public Guid ServiceId { get; set; }
        public ServiceResponse? Service { get; set; }
        public int NumerialOrder { get; set; }
    }
}
