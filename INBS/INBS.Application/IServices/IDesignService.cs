using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Design.NailDesign;
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
        Task<IEnumerable<DesignResponse>> Get();

        Task Create(DesignRequest modelRequest, IList<ImageRequest> images, IList<NailDesignRequest> nailDesigns);

        Task Delete(Guid designId);

        Task Update(Guid id, DesignRequest designReq, IList<ImageRequest> images, IList<NailDesignRequest> nailDesigns);
    }
}
