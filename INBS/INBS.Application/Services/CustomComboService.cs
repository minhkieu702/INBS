using AutoMapper;
using INBS.Application.DTOs.Service.CustomCombo;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class CustomComboService(IUnitOfWork _unitOfWork, IMapper _mapper) : ICustomComboService
    {
        private async Task ValidateService(IEnumerable<Guid> serviceIdsRequest)
        {
            var services = await _unitOfWork.ServiceRepository.GetAsync(include: query => query.Where(c => !c.IsDeleted && serviceIdsRequest.Contains(c.ID)));
            if (services.Count() != serviceIdsRequest.Count())
            {
                throw new Exception("Some service is not existed");
            }
        }

        private async Task<bool> ValidateServiceCustomCombo(Guid customerId,
            IList<ServiceCustomComboRequest> serviceCustomCombos)
        {
            var ccs = await _unitOfWork.CustomComboRepository
                .GetAsync(include: query => query.Where(c =>
                    c.CustomerID == customerId &&
                    c.IsFavorite)
                .Include(c => c.ServiceCustomCombos));

            var newCombos = SortAndSelect(_mapper.Map<IEnumerable<ServiceCustomCombo>>(serviceCustomCombos));

            foreach (var customCombo in ccs)
            {
                var existingCmbos = SortAndSelect(customCombo.ServiceCustomCombos);

                if (existingCmbos.SequenceEqual(newCombos))
                {
                    return false;
                }
            }

            return true;
        }

        private async Task InsertServiceCustomCombos(Guid customComboId, IList<ServiceCustomComboRequest> serviceCustomCombos)
        {
            var list = new List<ServiceCustomCombo>();
            foreach (var serviceCustomCombo in serviceCustomCombos)
            {
                list.Add(new ServiceCustomCombo
                {
                    NumerialOrder = serviceCustomCombo.NumerialOrder,
                    CustomComboId = customComboId,
                    ServiceId = serviceCustomCombo.ServiceId,
                });
            }
            await _unitOfWork.ServiceCustomComboRepository.InsertRangeAsync(list);
        }

        public async Task Create(CustomComboRequest request,
            IList<ServiceCustomComboRequest> serviceCustomCombos,
            ClaimsPrincipal claims)
        {
            try
            {
                //var cusId = claims.FindFirst(ClaimTypes.NameIdentifier) ?? throw new Exception("You must login first");

                await ValidateService(serviceCustomCombos.Select(c => c.ServiceId));

                if (await ValidateServiceCustomCombo(Guid.Parse(/*cusId.Value*/"e492d8f4-43ee-4ae2-be26-6128e2d8c582"), serviceCustomCombos) == false)
                    throw new Exception("This combo is already in your favorite combo service, you don't need create more!");

                _unitOfWork.BeginTransaction();

                var entity = _mapper.Map<CustomCombo>(request);

                entity.CustomerID = Guid.Parse(/*cusId.Value*/"e492d8f4-43ee-4ae2-be26-6128e2d8c582");

                await _unitOfWork.CustomComboRepository.InsertAsync(entity);

                await InsertServiceCustomCombos(entity.ID, serviceCustomCombos);

                if (await _unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();
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
            var entites = await _unitOfWork.CustomComboRepository.GetAsync(include: query =>
                query.Where(c => !c.IsDeleted)
                    .Include(c => c.ServiceCustomCombos.Where(c => c.CustomCombo != null && !c.CustomCombo.IsDeleted))
                        .ThenInclude(c => c.Service)
                    //.Include(c => c.Customer!).ThenInclude(c => c.User!)
                    //.Include(c => c.Bookings.Where(c => !c.IsDeleted)
                    );
            return _mapper.Map<IEnumerable<CustomComboResponse>>(entites);
        }

        private static IEnumerable<(Guid, int)> SortAndSelect(IEnumerable<ServiceCustomCombo> customCombos) => customCombos
            .Select(c => (c.ServiceId, c.NumerialOrder))
            .OrderBy(c => c.NumerialOrder);

        public async Task Update(Guid id, CustomComboRequest request, IList<ServiceCustomComboRequest> serviceCustomCombos, ClaimsPrincipal claims)
        {
            try
            {
                //var cusId = claims.FindFirst(ClaimTypes.NameIdentifier) ?? throw new Exception("You must login first");

                var entity = await _unitOfWork.CustomComboRepository.GetByIdAsync(id) ?? throw new Exception($"The entity with {id} is not existed.");

                await ValidateService(serviceCustomCombos.Select(c => c.ServiceId));

                _unitOfWork.BeginTransaction();

                if (SortAndSelect(_mapper.Map<IEnumerable<ServiceCustomCombo>>(serviceCustomCombos))
                    .SequenceEqual(SortAndSelect(entity.ServiceCustomCombos)))
                {
                    await _unitOfWork.CustomComboRepository.UpdateAsync(_mapper.Map(request, entity));
                }
                else if (await ValidateServiceCustomCombo(Guid.Parse(/*cusId.Value*/"e492d8f4-43ee-4ae2-be26-6128e2d8c582"), serviceCustomCombos) == false)
                {
                    entity.IsDeleted = true;
                    await _unitOfWork.CustomComboRepository.UpdateAsync(_mapper.Map(request, entity));
                }
                else
                {
                    _unitOfWork.ServiceCustomComboRepository.DeleteRange(entity.ServiceCustomCombos);
                    await InsertServiceCustomCombos(id, serviceCustomCombos);
                }

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
