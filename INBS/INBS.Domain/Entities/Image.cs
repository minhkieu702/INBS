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
    public class Image
    {
        public Image()
        {
            ImageUrl = Constants.DEFAULT_IMAGE_URL;
        }

        [Key]
        public Guid ID { get; set; }

        public int NumerialOrder { get; set; }

        public string ImageUrl { get; set; }

        public string? Description { get; set; }

        public Guid DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(Design.Images))]
        public virtual Design? Design { get; set; }
    }
}
