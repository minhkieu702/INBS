using INBS.Application.DTOs.Design;
using INBS.Application.DTOs.NailDesign;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.DTOs.Service;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.DesignService
{
    public class NailDesignServiceResponse
    {
        [Key]
        public Guid ID { get; set; }
        
        public Guid NailDesignId { get; set; }
        public virtual NailDesignResponse? Design { get; set; }

        public Guid ServiceId { get; set; }
        public virtual ServiceResponse? Service { get; set; }

        public long ExtraPrice { get; set; }

        public virtual ICollection<NailDesignServiceSelectedResponse> NailDesignServiceSelecteds { get; set; } = [];
    }
}
