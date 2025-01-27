using INBS.Domain.Common;
using INBS.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class NailDesign
    {
        public NailDesign()
        {
            ImageUrl = Constants.DEFAULT_IMAGE_URL;
            IsLeft = false;
        }

        public Guid DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(Design.NailDesigns))]
        public virtual Design? Design { get; set; }

        public string ImageUrl { get; set; }

        public int NailPosition { get; set; } //4, 8, 12, 16, 20

        public bool IsLeft { get; set; } //true - left, false - right

    }
}
