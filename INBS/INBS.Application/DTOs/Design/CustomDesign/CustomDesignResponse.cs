using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.Design.CustomNailDesign;
using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.User.Customer;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.CustomDesign
{
    public class CustomDesignResponse : BaseEntity
    {
        public bool IsSave { get; set; }
        public int Price { get; set; }
        public Guid CustomerId { get; set; }
        public virtual CustomerResponse? Customer { get; set; }
        //public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<CustomNailDesignResponse> CustomNailDesigns { get; set; } = [];

        public Guid DesignID { get; set; }
        public virtual DesignResponse? Design { get; set; }
    }
}
