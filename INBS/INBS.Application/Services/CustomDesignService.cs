using AutoMapper;
using INBS.Application.DTOs.Design.CustomDesign;
using INBS.Application.DTOs.Design.CustomNailDesign;
using INBS.Application.DTOs.Design.NailDesign;
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
    public class CustomDesignService(IUnitOfWork _unitOfWork, IFirebaseService _firebaseService, IMapper _mapper, IAuthentication _authentication, IHttpContextAccessor _contextAccessor) : ICustomDesignService
    {
        public async Task Delete(Guid id)
        {
            try
            {
                var result = await _unitOfWork.CustomDesignRepository.GetByIdAsync(id) ?? throw new Exception("This custom design is not found");

                result.IsDeleted = true;

                await _unitOfWork.CustomDesignRepository.UpdateAsync(result);

                if (await _unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<IEnumerable<CustomDesignResponse>> Get()
        {
            var customDesigns = await _unitOfWork.CustomDesignRepository.GetAsync(include: query => query
            .Include(c => c.CustomNailDesigns)
                .ThenInclude(c => c.AccessoryCustomNailDesigns)
                    .ThenInclude(c => c.Accessory)
            .Include(c => c.Design)
                .ThenInclude(c => c!.NailDesigns)
            .Include(c => c.Customer)
                .ThenInclude(c => c!.User)
            .AsNoTracking()
            );

            return _mapper.Map<IEnumerable<CustomDesignResponse>>(customDesigns);
        }

        private async Task<int> ValidateAccessories(IEnumerable<Guid> Ids)
        {
            var result = await _unitOfWork.AccessoryRepository.GetAsync(c => Ids.Contains(c.ID)) ?? throw new Exception("Some accessory's Ids is not existed");

            return result.Select(x => x.Price).Sum(x => x);
        }

        private async Task<long> ValidationDesignId(Guid id)
        {
            var result = await _unitOfWork.DesignRepository.GetByIdAsync(id) ?? throw new Exception("This design is not existed");
            return 0;
        }

        public async Task<Guid> Create(CustomDesignRequest request, IList<CustomNailDesignRequest> customNailDesignRequests)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var newEntity = _mapper.Map<CustomDesign>(request);

                var totalPrice = await ValidationDesignId(request.DesignId);

                foreach (var customNailDesignRequest in customNailDesignRequests)
                {
                    totalPrice += await ValidateAccessories(customNailDesignRequest.Acessories.Select(c => c.AccessoryId));
                }

                newEntity.CreatedAt = DateTime.Now;

                newEntity.Price = totalPrice;

                newEntity.CustomerID = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                await _unitOfWork.CustomDesignRepository.InsertAsync(newEntity);

                await HandleCustomNailDesign(newEntity.ID, customNailDesignRequests);

                if (await _unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();

                return newEntity.ID;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private async Task HandleCustomNailDesign(Guid customDesignID, IList<CustomNailDesignRequest> customNailDesignRequests)
        {
            var cNDlist = new List<CustomNailDesign>();
            var aCNDlist = new List<AccessoryCustomNailDesign>();

            var existedCustomNailDesign = await _unitOfWork.CustomNailDesignRepository.GetAsync(include:
                query => query
                .Where(c => c.CustomDesignId == customDesignID)
                .Include(c => c.AccessoryCustomNailDesigns));

            if (existedCustomNailDesign.Any())
            {
                HandleClearCustomNailDesign(existedCustomNailDesign);
            }

            foreach (var customNailDesignRequest in customNailDesignRequests)
            {
                var customNailDesign = _mapper.Map<CustomNailDesign>(customNailDesignRequest);

                if (customNailDesignRequest.NewImage != null)
                {
                    customNailDesign.ImageUrl = await _firebaseService.UploadFileAsync(customNailDesignRequest.NewImage);
                }

                customNailDesign.CustomDesignId = customDesignID;
                
                cNDlist.Add(customNailDesign);

                foreach (var accessoryReq in customNailDesignRequest.Acessories)
                {
                    var accessory = _mapper.Map<AccessoryCustomNailDesign>(accessoryReq);
                    accessory.CustomNailDesignId = customNailDesign.ID;
                    aCNDlist.Add(accessory);
                }

                await _unitOfWork.CustomNailDesignRepository.InsertRangeAsync(cNDlist);
                await _unitOfWork.AccessoryCustomNailDesignRepository.InsertRangeAsync(aCNDlist);
            }
        }

        private void HandleClearCustomNailDesign(IEnumerable<CustomNailDesign> existedCustomNailDesigns)
        {
            var acndlist = new List<AccessoryCustomNailDesign>();
            foreach (var customNailDesign in existedCustomNailDesigns)
            {
                acndlist.AddRange(customNailDesign.AccessoryCustomNailDesigns);
            }

            _unitOfWork.AccessoryCustomNailDesignRepository.DeleteRange(acndlist);
            _unitOfWork.CustomNailDesignRepository.DeleteRange(existedCustomNailDesigns);
        }

        public async Task Update(Guid id, CustomDesignRequest request, IList<CustomNailDesignRequest> customNailDesignRequests)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.CustomDesignRepository.GetByIdAsync(id) ?? throw new Exception("This custom design is not exist");

                _mapper.Map(request, existedEntity);

                if (request.NewImage != null)
                {
                    request.ImageUrl = await _firebaseService.UploadFileAsync(request.NewImage);
                }

                var totalPrice = await ValidationDesignId(request.DesignId);

                foreach (var customNailDesignRequest in customNailDesignRequests)
                {
                    totalPrice += await ValidateAccessories(customNailDesignRequest.Acessories.Select(c => c.AccessoryId));
                }

                existedEntity.Price = totalPrice;

                existedEntity.CustomerID = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                await _unitOfWork.CustomDesignRepository.UpdateAsync(existedEntity);

                await HandleCustomNailDesign(id, customNailDesignRequests);

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
