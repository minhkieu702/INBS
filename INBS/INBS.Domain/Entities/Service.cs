using INBS.Domain.Common;
using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Service : BaseEntity
    {
        public Service() : base()
        {
            Name = string.Empty;
            ImageUrl = Constants.DEFAULT_IMAGE_URL;
            ImageDescriptionUrl = Constants.DEFAULT_IMAGE_URL;
            NailDesignServices = [];
            CategoryServices = [];
            ArtistServices = [];
            ServicePriceHistories = [];
        }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string ImageDescriptionUrl { get; set; }

        public string ImageUrl { get; set; }

        public long AverageDuration { get; set; }

        public bool IsAdditional { get; set; }

        [InverseProperty(nameof(ServicePriceHistory.Service))]
        public virtual ICollection<ServicePriceHistory> ServicePriceHistories { get; set; }

        [InverseProperty(nameof(CategoryService.Service))]
        public virtual ICollection<CategoryService> CategoryServices { get; set; }

        [InverseProperty(nameof(ArtistService.Service))]
        public virtual ICollection<ArtistService> ArtistServices { get; set; }

        [InverseProperty(nameof(NailDesignService.Service))]
        public virtual ICollection<NailDesignService> NailDesignServices { get; set; }
    }
}
