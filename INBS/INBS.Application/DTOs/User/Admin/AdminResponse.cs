using INBS.Application.DTOs.Store;
using INBS.Application.DTOs.User.User;
using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User.Admin
{
    public class AdminResponse : BaseEntity
    {
        public Guid UserId { get; set; }
        public UserResponse? User { get; set; }

        public virtual ICollection<StoreResponse> Stores { get; set; } = [];
    }
}
