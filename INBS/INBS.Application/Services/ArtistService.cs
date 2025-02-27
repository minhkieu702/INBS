using AutoMapper;
using INBS.Application.Common;
using INBS.Application.Common.Enum;
using INBS.Application.DTOs.User.Artist;
using INBS.Application.DTOs.User.Artist.ArtistAvailability;
using INBS.Application.DTOs.User.User;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class ArtistService(IUnitOfWork _unitOfWork, IFirebaseService _firebaseService, IMapper _mapper, IAuthentication _authentication) : IArtistService
    {

        private async Task IsUniquePhoneNumber(string phoneNumber, Guid? userId = null)
        {
            var artist = await _unitOfWork.UserRepository.GetAsync(c => c.ID != userId && c.PhoneNumber == phoneNumber);
            if (artist != null)
            {
                throw new Exception("Phone number already exists");
            }
        }

        private async Task AssignService(Guid artistId, IList<Guid> serviceIds)
        {
            var oldServices = await _unitOfWork.ArtistServiceRepository.GetAsync(c => c.ArtistId == artistId);
            
            if(oldServices.Any()) _unitOfWork.ArtistServiceRepository.DeleteRange(oldServices);

            var newServices = new List<Domain.Entities.ArtistService>();

            foreach (var serviceId in serviceIds)
            {
                newServices.Add(new Domain.Entities.ArtistService
                {
                    ArtistId = artistId,
                    ServiceId = serviceId
                });
            }

            await _unitOfWork.ArtistServiceRepository.InsertRangeAsync(newServices);
        }

        private async Task AssignDesign(Guid artistId, IList<Guid> designIds)
        {
            var oldDesigns = await _unitOfWork.ArtistDesignRepository.GetAsync(c => c.ArtistId == artistId);

            if (oldDesigns.Any()) _unitOfWork.ArtistDesignRepository.DeleteRange(oldDesigns);

            var newDesigns = new List<ArtistDesign>();
            
            foreach (var designId in designIds)
            {
                newDesigns.Add(new ArtistDesign
                {
                    ArtistId = artistId,
                    DesignId = designId
                });
            }
            
            await _unitOfWork.ArtistDesignRepository.InsertRangeAsync(newDesigns);
        }

        private async Task IsStoreExisting(Guid storeId)
        {
            _ = await _unitOfWork.StoreRepository.GetByIdAsync(storeId) ?? throw new Exception("Store not found");
        }

        private async Task<string> GetUsername(string fullname)
        {
            var username = Utils.TransToUsername(fullname);
            
            var users = await _unitOfWork.UserRepository.GetAsync(c => Utils.RemoveNonAlphabetic(c.Username) == username);

            return users.Any() ? username += users.Count() : username;
        }

        public async Task<User> CreateUser(UserRequest userRequest)
        {
            await IsUniquePhoneNumber(userRequest.PhoneNumber);

            var user = _mapper.Map<User>(userRequest);
            
            user.Username = await GetUsername(userRequest.FullName);
            
            user.PasswordHash = _authentication.HashedPassword(user, "password123!@#");
            
            user.Role = (int)Role.Artist;

            user.IsVerified = true;
            
            if (userRequest.NewImage != null)
                user.ImageUrl = await _firebaseService.UploadFileAsync(userRequest.NewImage);
            
            await _unitOfWork.UserRepository.InsertAsync(user);
            
            return user;
        }

        public async Task<ArtistResponse> Create(ArtistRequest artistRequestModel, UserRequest userRequestModel)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                await IsStoreExisting(artistRequestModel.StoreId);
                
                var user = await CreateUser(userRequestModel);

                var artist = _mapper.Map<Artist>(artistRequestModel);

                artist.ID = user.ID;

                await AssignDesign(artist.ID, artistRequestModel.DesignIds);

                await AssignService(artist.ID, artistRequestModel.ServiceIds);

                await _unitOfWork.ArtistRepository.InsertAsync(artist);

                if (await _unitOfWork.SaveAsync() <= 0)
                    throw new Exception("This action failed");
                
                _unitOfWork.CommitTransaction();

                artist.User = user;

                return _mapper.Map<ArtistResponse>(artist);
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
                var artist = await _unitOfWork.ArtistRepository.GetByIdAsync(id) ?? throw new Exception("This artist not found");

                await _unitOfWork.ArtistRepository.DeleteAsync(id);

                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("This action failed");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ArtistResponse>> Get()
        {
            try
            {
                var artist = await _unitOfWork.ArtistRepository.GetAsync(include: query => 
                query.Where(c => c.User != null && !c.User.IsDeleted)
                    .Include(c => c.User)
                    .Include(c => c.Store)
                    .Include(c => c.ArtistAvailabilities.Where(c => !c.IsDeleted))
                    .Include(c => c.ArtistDesigns.Where(c => c.Design != null && !c.Design.IsDeleted))
                        .ThenInclude(c => c.Design)
                    .Include(c => c.ArtistServices.Where(c => c.Service != null && !c.Service.IsDeleted))
                        .ThenInclude(c => c.Service)
                    .Include(c => c.ArtistAvailabilities.Where(c => !c.IsDeleted))
                    );
                return _mapper.Map<IEnumerable<ArtistResponse>>(artist);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Update(Guid id, ArtistRequest artistRequest, UserRequest userRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var artist = await _unitOfWork.ArtistRepository.GetByIdAsync(id) ?? throw new Exception("This artist not found");

                await IsStoreExisting(artistRequest.StoreId);

                await UpdateUser(id, userRequest);

                await AssignDesign(artist.ID, artistRequest.DesignIds);

                await AssignService(artist.ID, artistRequest.ServiceIds);

                _mapper.Map(artistRequest, artist);

                await _unitOfWork.ArtistRepository.UpdateAsync(artist);

                if (await _unitOfWork.SaveAsync() <= 0)
                    throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task UpdateUser(Guid id, UserRequest userRequest)
        {
            await IsUniquePhoneNumber(userRequest.PhoneNumber, id);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(id) ?? throw new Exception("User not found");

            _mapper.Map(userRequest, user);

            if (userRequest.NewImage != null)
                user.ImageUrl = await _firebaseService.UploadFileAsync(userRequest.NewImage);

            await _unitOfWork.UserRepository.UpdateAsync(user);
        }
    }
}
