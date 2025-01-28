using INBS.Application.DTOs.Service.TemplateCombo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.ServiceTemplateCombo
{
    public class TemplateComboRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

    }
}
