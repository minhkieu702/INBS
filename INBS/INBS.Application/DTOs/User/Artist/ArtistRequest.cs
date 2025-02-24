using INBS.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User.Artist
{
    public class ArtistRequest
    {
        public Guid StoreId { get; set; }
        public IList<Guid> DesignIds { get; set; } = [];
        public IList<Guid> ServiceIds { get; set; } = [];
    }
}
