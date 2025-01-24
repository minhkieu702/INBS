using INBS.Application.Services;
using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Store
{
    public class StoreResponse : BaseEntity
    {
        public string Address { get; set; }

        public string? Description { get; set; }

        public string ImageUrl { get; set; }

        public int Status { get; set; } // 0: inactive, 1: active

#warning Add ArtistResponse, AdminResponse, DesignResponse

        public ICollection<StoreServiceResponse> StoreServices { get; set; } = [];

        public ICollection<StoreDesignResponse> StoreDesigns { get; set; }

    }
}
