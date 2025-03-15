﻿using INBS.Application.DTOs.CustomerSelected;
using System.ComponentModel.DataAnnotations;

namespace INBS.Application.DTOs.NailDesignServiceSelected
{
    public class NailDesignServiceSelectedResponse
    {
        [Key]
        public Guid CustomerSelectedId { get; set; }
        public virtual CustomerSelectedResponse? CustomerSelected { get; set; }

        [Key]
        public Guid NailDesignServiceId { get; set; }
        public virtual NailDesignServiceSelectedResponse? NailDesignService { get; set; }

        public long Duration { get; set; }
    }
}
