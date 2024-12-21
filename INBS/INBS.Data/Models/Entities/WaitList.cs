using INBS.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Data.Models.Entities
{
    public class WaitList
    {
        public WaitList()
        {
            ID = Guid.NewGuid();
            AddedAt = DateTime.Now;
        }

        [Key]
        public Guid ID { get; set; }

        public DateOnly RequestedDate { get; set; }

        public TimeOnly RequestedTime { get; set; }

        public int Status { get; set; }

        public DateTime AddedAt { get; set; }

        public Guid ArtistId { get; set; }
        [ForeignKey(nameof(ArtistId))]
        [InverseProperty(nameof(Artist.WaitLists))]
        public virtual Artist? Artist { get; set; }

        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.WaitLists))]
        public virtual Customer? Customer { get; set; }
    }
}
