using INBS.Domain.Entities;
using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Design : BaseEntity
    {
        public Design() : base()
        {
            Name = string.Empty;
            Images = [];
            CustomDesigns = [];
            NailDesigns = [];
            StoreDesigns = [];
            DesignPreferences = [];
        }

        public string Name { get; set; }

        public float TrendScore { get; set; }

        public string? Description { get; set; }

        public int Price { get; set; }

        [InverseProperty(nameof(Image.Design))]
        public virtual ICollection<Image> Images { get; set; }

        [InverseProperty(nameof(CustomDesign.Design))]
        public virtual ICollection<CustomDesign> CustomDesigns { get; set; }
        [InverseProperty(nameof(NailDesign.Design))]
        public virtual ICollection<NailDesign> NailDesigns { get; set; }

        [InverseProperty(nameof(StoreDesign.Design))]
        public virtual ICollection<StoreDesign> StoreDesigns { get; set; }

        [InverseProperty(nameof(DesignPreference.Design))]
        public virtual ICollection<DesignPreference> DesignPreferences { get; set; }
    }
}
