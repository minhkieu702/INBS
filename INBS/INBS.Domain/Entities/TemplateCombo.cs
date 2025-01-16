using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class TemplateCombo
    {
        public TemplateCombo()
        {
            ID = Guid.NewGuid();
            ServiceTemplateCombos = [];
            Name = string.Empty;
        }

        [Key]
        public Guid ID { get; set; }
        
        public string Name { get; set; }

        public string? Description { get; set; }

        [InverseProperty(nameof(ServiceTemplateCombo.TemplateCombo))]
        public virtual ICollection<ServiceTemplateCombo> ServiceTemplateCombos { get; set; }
    }
}
