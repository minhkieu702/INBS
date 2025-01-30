using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.User.Customer;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.CustomCombo
{
    public class CustomComboResponse : BaseEntity
    {
        public bool IsFavorite { get; set; }

        public ICollection<ServiceCustomComboResponse> ServiceCustomCombos { get; set; } = [];

        public Guid CustomerID { get; set; }

        public CustomerResponse? Customer { get; set; }
    }
}
