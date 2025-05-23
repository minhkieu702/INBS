﻿using INBS.Application.DTOs.ArtistStore;
using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.CustomerSelected;
using INBS.Application.DTOs.Feedback;
using INBS.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Booking
{
    public class BookingResponse : BaseEntity
    {
        public DateOnly ServiceDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly PredictEndTime { get; set; }

        public bool IsFavorite { get; set; }

        public int Status { get; set; }

        public long TotalAmount { get; set; }

        public Guid CustomerSelectedId { get; set; }

        public virtual CustomerSelectedResponse? CustomerSelected { get; set; }

        public Guid ArtistStoreId { get; set; }
        
        public virtual ArtistStoreResponse? ArtistStore { get; set; }

        public virtual ICollection<CancellationResponse> Cancellations { get; set; } = [];
    }
}
