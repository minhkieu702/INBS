using AutoMapper;
using INBS.Application.DTOs.Service.CustomCombo;
using INBS.Application.IServices;
using INBS.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class CustomComboService(IUnitOfWork _unitOfWork, IMapper _mapper) : ICustomComboService
    {
        public Task Create(CustomComboRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<CustomComboResponse> Get()
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid id, CustomComboRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
