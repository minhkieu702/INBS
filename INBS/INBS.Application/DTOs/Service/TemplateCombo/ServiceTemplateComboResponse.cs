using INBS.Application.DTOs.Service.Service;
using INBS.Application.DTOs.Service.ServiceTemplateCombo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.TemplateCombo
{
    public class ServiceTemplateComboResponse
    {
        [Key]
        public Guid TemplateComboId { get; set; }
        public TemplateComboResponse? TemplateCombo { get; set; }

        [Key]
        public Guid ServiceId { get; set; }
        public int NumerialOrder { get; set; }
        public ServiceResponse? Service { get; set; }
    }
}
