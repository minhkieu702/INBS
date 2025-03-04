using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.DesignService
{
    public class DesignServiceResponse
    {
        [Key]
        public Guid DesignId { get; set; }
        public virtual Domain.Entities.Design? Design { get; set; }

        [Key]
        public Guid ServiceId { get; set; }
        public virtual Domain.Entities.Service? Service { get; set; }
    }
}
