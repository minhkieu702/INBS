using INBS.Application.DTOs.Design.CustomDesign;
using INBS.Application.DTOs.Design.CustomNailDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface ICustomDesignService
    {
        Task<IEnumerable<CustomDesignResponse>> Get();

        Task Create(ClaimsPrincipal claims, CustomDesignRequest request, IList<CustomNailDesignRequest> customNailDesignRequests);

        Task Delete(Guid id);

        Task Update(ClaimsPrincipal claims, Guid id, CustomDesignRequest request, IList<CustomNailDesignRequest> customNailDesignRequests);
    }
}
