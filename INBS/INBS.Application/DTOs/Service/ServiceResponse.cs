using INBS.Application.DTOs.ArtistService;
using INBS.Application.DTOs.CategoryService;
using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.NailDesignService;
using INBS.Application.DTOs.ServicePriceHistory;
using INBS.Application.DTOs.Store;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service
{
    public class ServiceResponse : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string ImageUrl { get; set; } = Constants.DEFAULT_IMAGE_URL;

        public long Price { get; set; }

        public bool IsAdditional { get; set; }

        public long AverageDuration { get; set; }

        public virtual ICollection<CategoryServiceResponse> CategoryServices { get; set; } = [];

        public virtual ICollection<ArtistServiceResponse> ArtistServices { get; set; } = [];

        public virtual ICollection<NailDesignServiceResponse> NailDesignServices { get; set; } = [];

        public virtual ICollection<ServicePriceHistoryResponse> ServicePriceHistories { get; set; } = [];
    }
}
