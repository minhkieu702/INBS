using INBS.Application.DTOs.Service.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.Store
{
    public class StoreServiceResponse
    {
        public Guid StoreId { get; set; }
        public virtual StoreResponse? Store { get; set; }

        public Guid ServiceId { get; set; }
        public virtual ServiceResponse? Service { get; set; }
    }
}
