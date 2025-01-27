using INBS.Domain.Common;
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
    public class CustomNailDesign
    {
        public CustomNailDesign()
        {
            AccessoryCustomNailDesigns = [];
            ID = Guid.NewGuid();
            IsLeft = false;
            ImageUrl = Constants.DEFAULT_IMAGE_URL;
        }

        [Key]
        public Guid ID { get; set; }

        public Guid CustomDesignId { get; set; }
        [ForeignKey(nameof(CustomDesignId))]
        [InverseProperty(nameof(CustomDesign.CustomNailDesigns))]
        public virtual CustomDesign? CustomDesign { get; set; }

        public string ImageUrl { get; set; }

        public int NailPosition { get; set; } //4, 8, 12, 16, 20

        public bool IsLeft { get; set; } //true - left, false - right

        [InverseProperty(nameof(AccessoryCustomNailDesign.CustomNailDesign))]
        public virtual ICollection<AccessoryCustomNailDesign> AccessoryCustomNailDesigns { get; set; }
    }
}
