﻿using INBS.Application.DTOs.Artist;
using INBS.Application.DTOs.ArtistService;
using INBS.Application.DTOs.ArtistStore;
using INBS.Application.DTOs.User;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistResponse>> Get();
        Task<ArtistResponse> Create(ArtistRequest artistRequest, UserRequest userRequest, IList<ArtistServiceRequest> artistServiceRequest, IList<ArtistStoreRequest> artistStoreRequest);
        Task Update(Guid id, ArtistRequest requestModel, UserRequest userRequest, IList<ArtistServiceRequest> artistServiceRequest, IList<ArtistStoreRequest> artistStoreRequest);
        Task Delete(Guid id);
    }
}
