using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.NailDesignService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Cart
{
    public class CartResponse
    {
        [Key]
        public Guid CustomerId { get; set; }
        public virtual CustomerResponse? Customer { get; set; }

        [Key]
        public Guid NailDesignServiceId { get; set; }
        public virtual NailDesignServiceResponse? NailDesignService { get; set; }
    }
}
