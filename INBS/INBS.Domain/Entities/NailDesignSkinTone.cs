using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class NailDesignSkinTone
    {
        public Guid DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(NailDesign.NailDesignSkinTones))]
        public virtual NailDesign? Design { get; set; }

        public Guid SkinToneId { get; set; }
        [ForeignKey(nameof(SkinToneId))]
        [InverseProperty(nameof(SkinTone.NailDesignSkinTones))]
        public virtual SkinTone? SkinTone { get; set; }
    }
}
