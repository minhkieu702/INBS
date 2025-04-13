using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class ArtistCertificate
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ArtistId { get; set; }

        public int NumerialOrder { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public string ImageUrl { get; set; }

        [ForeignKey(nameof(ArtistId))]
        [InverseProperty(nameof(Artist.Certificates))]
        public virtual Artist Artist { get; set; } = null!;
    }
}
