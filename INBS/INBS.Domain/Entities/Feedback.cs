﻿using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Feedback : BaseEntity
    {
        public Feedback() : base()
        {
            Rating = 5;
            Content = string.Empty;
        }
        public Guid TypeId { get; set; }
        public int Type { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.Feedbacks))]
        public virtual Customer? Customer { get; set; }

        public Guid? StoreId { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(Store.Feedbacks))]
        public virtual Store? Store { get; set; }

        public Guid? ArtistId { get; set; }
        [ForeignKey(nameof(ArtistId))]
        [InverseProperty(nameof(Artist.Feedbacks))]
        public virtual Artist? Artist { get; set; }

        public Guid? DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(Design.Feedbacks))]
        public virtual Design? Design { get; set; }
    }
}
