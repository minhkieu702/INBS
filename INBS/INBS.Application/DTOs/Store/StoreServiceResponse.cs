using INBS.Application.DTOs.Service.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Store
{
    public class StoreServiceResponse
    {
        [Key]
        public Guid StoreId { get; set; }
        public virtual StoreResponse? Store { get; set; }

        [Key]
        public Guid ServiceId { get; set; }
        public virtual ServiceResponse? Service { get; set; }
    }
}
