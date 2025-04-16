using INBS.Application.DTOs.NailDesignService;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface INailDesignServiceService
    {
        IQueryable<NailDesignServiceResponse> Get();
        Task UpdateTime(ServiceDurationRequest serviceDuration);
    }
}
