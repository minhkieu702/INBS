﻿using INBS.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.Image
{
    public class ImageRequest : BaseEntity
    {
        public string? ImageUrl { get; set; }
        public int NumerialOrder { get; set; }
        public string? Description { get; set; }
    }
}
