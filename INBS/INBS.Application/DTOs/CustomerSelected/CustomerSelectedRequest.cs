﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.CustomerSelected
{
    public class CustomerSelectedRequest
    {
        [Key]
        public Guid ID { get; set; }

        public bool IsFavorite { get; set; }
    }
}
