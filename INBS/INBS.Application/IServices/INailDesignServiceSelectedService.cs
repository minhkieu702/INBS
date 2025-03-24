using INBS.Application.DTOs.NailDesignServiceSelected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface INailDesignServiceSelectedService
    {
        IQueryable<NailDesignServiceSelectedResponse> Get();
    }
}
