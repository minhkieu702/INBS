using INBS.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Data.Models.Entities
{
    public class SkinTone
    {
        public SkinTone()
        {
            ID = Guid.NewGuid();
            Customer = [];
            NailDesignSkinTones = [];
            RGPColor = string.Empty;
        }
        [Key]
        public Guid ID { get; set; }

        public string? Name { get; set; }

        public string RGPColor { get; set; }

        [InverseProperty(nameof(Entities.Customer.SkinTone))]
        public virtual ICollection<Customer> Customer { get; set; }

        [InverseProperty(nameof(NailDesignSkinTone.SkinTone))]
        public virtual ICollection<NailDesignSkinTone> NailDesignSkinTones { get; set; }
    }
}
