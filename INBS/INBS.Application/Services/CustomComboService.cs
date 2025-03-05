using AutoMapper;
using INBS.Application.DTOs.Service.CustomCombo;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class CustomComboService(IUnitOfWork _unitOfWork, IMapper _mapper, IAuthentication _authentication, IHttpContextAccessor _contextAccesstor) : ICustomComboService
    {
        private async Task ValidateService(IEnumerable<Guid> serviceIdsRequest)
        {
            var services = await _unitOfWork.ServiceRepository.GetAsync(include: query => query.Where(c => !c.IsDeleted && serviceIdsRequest.Contains(c.ID)));
            if (services.Count() != serviceIdsRequest.Count())
            {
                    throw new Exception("Some service is not existed");
            }
        }

        public async Task<Guid> Create(CustomComboRequest request,
            IList<Guid> serviceCustomCombos)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var cusId = _authentication.GetUserIdFromHttpContext(_contextAccesstor.HttpContext);

                await ValidateService(serviceCustomCombos);

                var entity = _mapper.Map<CustomCombo>(request);

                entity.CreatedAt = DateTime.Now;

                entity.CustomerID = cusId;

                await _unitOfWork.CustomComboRepository.InsertAsync(entity);

                serviceCustomCombos.Add(request.MainService);

                await HandleServiceCustomCombo(entity.ID, serviceCustomCombos);

                if (await _unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();

                return entity.ID;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.CustomComboRepository.GetByIdAsync(id) ?? throw new Exception($"The entity with {id} is not existed.");

                _unitOfWork.BeginTransaction();

                entity.IsDeleted = true;

                await _unitOfWork.CustomComboRepository.UpdateAsync(entity);

                if (await _unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<IEnumerable<CustomComboResponse>> Get()
        {
            try
            {
                var entites = await _unitOfWork.CustomComboRepository.GetAsync(include: query =>
                query
                    .Include(c => c.ServiceCustomCombos.Where(c => !c.Service!.IsDeleted))
                        .ThenInclude(c => c.Service)
                    .Include(c => c.Customer).ThenInclude(c => c!.User)
                    .Where(c => !c.IsDeleted && !c.Customer!.User!.IsDeleted)
                    //.Include(c => c.Bookings.Where(c => !c.IsDeleted)
                    );
                return _mapper.Map<IEnumerable<CustomComboResponse>>(entites);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task HandleServiceCustomCombo(Guid customComboId, IList<Guid> serviceIds)
        {
            var oldServiceCustomCombos = await _unitOfWork.ServiceCustomComboRepository.GetAsync(c => c.CustomComboId == customComboId);
            
            if (oldServiceCustomCombos.Any())
                _unitOfWork.ServiceCustomComboRepository.DeleteRange(oldServiceCustomCombos);

            var insertServiceCustomCombos = new List<ServiceCustomCombo>();

            foreach (var serviceId in serviceIds)
            {
                insertServiceCustomCombos.Add(new ServiceCustomCombo
                {
                    CustomComboId = customComboId,
                    ServiceId = serviceId
                });
            }

            await _unitOfWork.ServiceCustomComboRepository.InsertRangeAsync(insertServiceCustomCombos);
        }

        public async Task Update(Guid id, CustomComboRequest request, IList<Guid> serviceCustomCombos)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var entity = await _unitOfWork.CustomComboRepository.GetByIdAsync(id) ?? throw new Exception($"The entity with {id} is not existed.");

                await ValidateService(serviceCustomCombos);

                await HandleServiceCustomCombo(id, serviceCustomCombos);

                if (await _unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}
