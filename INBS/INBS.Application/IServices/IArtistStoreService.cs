﻿using INBS.Application.DTOs.ArtistStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IArtistStoreService
    {
        IQueryable<ArtistStoreResponse> GetAll();
    }
}
