using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.NailDesignServiceSelected;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.CustomerSelected
{
    public class CustomerSelectedResponse
    {
        [Key]
        public Guid ID { get; set; }

        public bool IsFavorite { get; set; }

        public Guid CustomerID { get; set; }

        public virtual CustomerResponse? Customer { get; set; }

        public virtual ICollection<NailDesignServiceSelectedResponse> NailDesignServiceSelecteds { get; set; } = [];

#warning Add Bookings property
    }
}
