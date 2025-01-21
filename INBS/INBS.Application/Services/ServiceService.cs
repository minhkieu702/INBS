using AutoMapper;
using INBS.Application.DTOs.Service;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        private async Task<int> InsertCategoryService(Guid serviceId, Guid categoryId)
        {
            int count = 0;

            if (_unitOfWork.CategoryServiceRepository.GetByID(categoryId) != null)
            {
                var categoryService = new Domain.Entities.CategoryService
                {
                    CategoryId = categoryId,
                    ServiceId = serviceId
                };
                await _unitOfWork.CategoryServiceRepository.InsertAsync(categoryService);
                count++;
            }

            return count;
        }

        public async Task Create(ServiceRequest modelRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                int count = 0;

                var existedEntity = await _unitOfWork.ServiceRepository.GetAsync(x => x.Name == modelRequest.Name);

                if (existedEntity != null && existedEntity.Count() > 0)
                    throw new Exception("Service already exists");

                //
                //push image to firebase storage function (do later)
                //

                var newService = _mapper.Map<Service>(modelRequest);
                await _unitOfWork.ServiceRepository.InsertAsync(newService);
                count++;

                var insertTasks = modelRequest.CategoryIds
                    .Select(async x => await InsertCategoryService(newService.ID, x));
                count += (await Task.WhenAll(insertTasks)).Sum();

                if (await _unitOfWork.SaveAsync() != count)
                {
                    _unitOfWork.RollBack();
                    throw new Exception("Create service failed");
                }
                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        public Task DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ServiceResponse>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid id, ServiceRequest category)
        {
            throw new NotImplementedException();
        }
    }
}
