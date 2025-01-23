using INBS.Application.DTOs.Design.Design;
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

        Task Create(DesignRequest modelRequest);

        Task Delete(Guid designId);

        Task Update(Guid designId, DesignRequest modelRequest);
    }
}
