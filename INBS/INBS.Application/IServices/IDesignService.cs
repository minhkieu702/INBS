using INBS.Application.DTOs.Design;
using INBS.Application.DTOs.Image;
using INBS.Application.DTOs.NailDesign;
using INBS.Application.DTOs.Preference;
using INBS.Application.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IService
{
    public interface IDesignService
    {
        IQueryable<DesignResponse> Get();

        Task Create(DesignRequest modelRequest, PreferenceRequest preferenceRequest, IList<MediaRequest> images, IList<NailDesignRequest> nailDesigns);

        Task Delete(Guid designId);

        Task Update(Guid id, DesignRequest designReq, PreferenceRequest preferenceRequest, IList<MediaRequest> images, IList<NailDesignRequest> nailDesigns);
        Task<List<DesignResponse>> RecommendDesign();
    }
}
