using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.CustomCombo
{
    public class CustomComboRequest
    {
        public bool IsFavorite { get; set; }
        public Guid MainService { get; set; }
    }
}
