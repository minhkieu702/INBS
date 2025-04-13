using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.CustomerSelected;
using INBS.Application.DTOs.NailDesign;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace INBS.Application.Services
{
    public class CustomerSelectedService(IUnitOfWork _unitOfWork, IMapper _mapper, IAuthentication _authentication, IHttpContextAccessor _contextAccesstor, IFirebaseCloudMessageService _fcmService) : ICustomerSelectedService
    {
        private async Task HandleCart(IEnumerable<Guid> nailDesignServiceIds)
        {
            var cart = await _unitOfWork.CartRepository.GetAsync(c => c.Where(c => nailDesignServiceIds.Contains(c.NailDesignServiceId)));

            _unitOfWork.CartRepository.DeleteRange(cart);
        }
        private async Task HandleNailDesignServiceSelected(Guid customerSelectedId, IList<NailDesignServiceSelectedRequest> nailDesignServiceSelectedRequests)
        {
            if (!nailDesignServiceSelectedRequests.Any())
            {
                throw new Exception("You are not selecting any service of nail design");
            }
            await ValidateNailDesignServiceSelected(nailDesignServiceSelectedRequests);

            var oldNailDesignServiceSelecteds = await _unitOfWork.NailDesignServiceSelectedRepository.GetAsync(c => c.Where(c => c.CustomerSelectedId == customerSelectedId));

            //var updateList = new List<NailDesignServiceSelected>();
            var insertList = new List<NailDesignServiceSelected>();
            var deleteList = new List<NailDesignServiceSelected>();
            var newNDSIds = nailDesignServiceSelectedRequests.Select(c => c.NailDesignServiceId);
            var processedItems = new HashSet<NailDesignServiceSelectedRequest>();

            foreach (var oldItem in oldNailDesignServiceSelecteds)
            {
                var newItem = nailDesignServiceSelectedRequests.FirstOrDefault(c => c.NailDesignServiceId == oldItem.NailDesignServiceId);
                
                if (newItem != null)
                {
                    processedItems.Add(newItem);
                }
                else
                {
                    deleteList.Add(oldItem);
                }
            }

            nailDesignServiceSelectedRequests = nailDesignServiceSelectedRequests.Except(processedItems).ToList();

            if (nailDesignServiceSelectedRequests.Count != 0)
            {
                var newEntities = _mapper.Map<List<NailDesignServiceSelected>>(nailDesignServiceSelectedRequests);
                
                newEntities.ForEach(c => c.CustomerSelectedId = customerSelectedId);
                
                insertList.AddRange(newEntities);
            }

            if (deleteList.Count != 0)
                _unitOfWork.NailDesignServiceSelectedRepository.DeleteRange(deleteList);

            if (insertList.Count != 0)
                await _unitOfWork.NailDesignServiceSelectedRepository.InsertRangeAsync(insertList);
        }

        private async Task ValidateNailDesignServiceSelected(IList<NailDesignServiceSelectedRequest> nailDesignServiceSelectedRequests)
        {
            var newIds = nailDesignServiceSelectedRequests.Select(c => c.NailDesignServiceId);
            var nailDesignServices = await _unitOfWork.NailDesignServiceRepository.GetAsync(query => query.Where(c => newIds.Contains(c.ID)));

            if (nailDesignServices.Count() != newIds.Count())
            {
                throw new Exception("Some services of nail design not found");
            }
        }

        public async Task<Guid> Create(CustomerSelectedRequest request, IList<NailDesignServiceSelectedRequest> nailDesignServiceSelectedRequests)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var cusId = _authentication.GetUserIdFromHttpContext(_contextAccesstor.HttpContext);

                var entity = _mapper.Map<CustomerSelected>(request);

                entity.CustomerID = cusId;

                await _unitOfWork.CustomerSelectedRepository.InsertAsync(entity);

                await HandleCart(nailDesignServiceSelectedRequests.Select(c => c.NailDesignServiceId));

                await HandleNailDesignServiceSelected(entity.ID, nailDesignServiceSelectedRequests);

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
                var entity = await _unitOfWork.CustomerSelectedRepository.GetByIdAsync(id) ?? throw new Exception($"The entity with {id} is not existed.");

                _unitOfWork.BeginTransaction();

                entity.IsDeleted = !entity.IsDeleted;

                await _unitOfWork.CustomerSelectedRepository.UpdateAsync(entity);

                if (await _unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public IQueryable<CustomerSelectedResponse> Get()
        {
            try
            {
                var role = _authentication.GetUserRoleFromHttpContext(_contextAccesstor.HttpContext);
                if (role == 2)
                {
                    return _unitOfWork.CustomerSelectedRepository.Query().ProjectTo<CustomerSelectedResponse>(_mapper.ConfigurationProvider);
                }
                return _unitOfWork.CustomerSelectedRepository.Query().Where(c => !c.IsDeleted).ProjectTo<CustomerSelectedResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Update(Guid id, CustomerSelectedRequest request, IList<NailDesignServiceSelectedRequest> nailDesignServiceSelectedRequests)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var entity = await _unitOfWork.CustomerSelectedRepository.GetByIdAsync(id) ?? throw new Exception($"The entity with {id} is not existed.");

                _mapper.Map(request, entity);

                await _unitOfWork.CustomerSelectedRepository.UpdateAsync(entity);

                await HandleNailDesignServiceSelected(id, nailDesignServiceSelectedRequests);

                var check = await _unitOfWork.SaveAsync();

                if (check == 0) throw new Exception("This action failed");

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
