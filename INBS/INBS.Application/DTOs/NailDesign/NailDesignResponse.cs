using INBS.Application.DTOs.Design;
using INBS.Application.DTOs.NailDesignService;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.NailDesign
{
    public class NailDesignResponse
    {
        [Key]
        public Guid ID { get; set; }

        public Guid DesignId { get; set; }
        public virtual DesignResponse? Design { get; set; }

        public string ImageUrl { get; set; } = Constants.DEFAULT_IMAGE_URL;

        public int NailPosition { get; set; } //4, 8, 12, 16, 20

        public bool IsLeft { get; set; } //true - left, false - right

        public virtual ICollection<NailDesignServiceResponse> NailDesignServices { get; set; } = [];
    }
}
