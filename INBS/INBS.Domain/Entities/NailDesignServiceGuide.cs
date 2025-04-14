using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class NailDesignServiceGuide : BaseEntity
    {
        public Guid NailDesignServiceGuideId { get; set; }

        public Guid NailDesignServiceId { get; set; }

        [ForeignKey(nameof(NailDesignServiceId))]
        [InverseProperty(nameof(NailDesignService.Guides))]
        public virtual NailDesignService? NailDesignService { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
    }
}
