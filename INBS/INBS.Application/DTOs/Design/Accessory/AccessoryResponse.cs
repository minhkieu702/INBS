using INBS.Application.DTOs.Common;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.Accessory
{
    public class AccessoryResponse : BaseEntity
    {
        public AccessoryResponse() : base()
        {
            Name = string.Empty;
            ImageUrl = string.Empty;
        }

        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
