﻿using INBS.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Image
{
    public class MediaRequest
    {
        public IFormFile? NewImage { get; set; }
        public string? ImageUrl { get; set; }
        public int MediaType { get; set; }
        public int NumerialOrder { get; set; }
    }
}
