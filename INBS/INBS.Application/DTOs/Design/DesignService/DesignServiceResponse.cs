using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Service.Service;
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
        public virtual DesignResponse? Design { get; set; }

        [Key]
        public Guid ServiceId { get; set; }
        public virtual ServiceResponse? Service { get; set; }
    }
}
