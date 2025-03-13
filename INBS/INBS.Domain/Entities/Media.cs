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
    public class Media
    {
        public Media()
        {
            ImageUrl = Constants.DEFAULT_IMAGE_URL;
        }
        public int NumerialOrder { get; set; }

        public string ImageUrl { get; set; }

        public int MediaType { get; set; }

        public Guid DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(Design.Medias))]
        public virtual Design? Design { get; set; }
    }
}
