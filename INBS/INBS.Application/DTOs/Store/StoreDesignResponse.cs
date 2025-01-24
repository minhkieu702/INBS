using INBS.Application.DTOs.Design.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Store
{
    public class StoreDesignResponse
    {
        [Key]
        public Guid StoreId { get; set; }
        public StoreResponse? Store { get; set; }

        [Key]
        public Guid DesignId { get; set; }
        public DesignResponse? Design { get; set; }
    }
}
