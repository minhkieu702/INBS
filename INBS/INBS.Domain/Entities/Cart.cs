using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Cart
    {
        [Key]
        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.Carts))]
        public virtual Customer? Customer { get; set; }

        [Key]
        public Guid NailDesignServiceId { get; set; }
        [ForeignKey(nameof(NailDesignServiceId))]
        [InverseProperty(nameof(NailDesignService.Carts))]
        public virtual NailDesignService? NailDesignService { get; set; }
    }
}
