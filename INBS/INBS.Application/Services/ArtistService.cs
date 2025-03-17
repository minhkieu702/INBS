using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.Common;
using INBS.Application.DTOs.Artist;
using INBS.Application.DTOs.ArtistService;
using INBS.Application.DTOs.ArtistStore;
using INBS.Application.DTOs.User;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace INBS.Application.Services
{
    public class ArtistService(IUnitOfWork _unitOfWork, IFirebaseService _firebaseService, IMapper _mapper, IAuthentication _authentication) : IArtistService
    {

        private async Task IsUniquePhoneNumber(string phoneNumber, Guid? userId = null)
        {
            var artist = await _unitOfWork.UserRepository.GetAsync(query => query.Where(c => c.ID != userId && c.PhoneNumber == phoneNumber));
            if (artist.Any())
            {
                throw new Exception("Phone number already exists");
            }
        }

        private async Task AssignService(Guid artistId, IList<Guid> serviceIds)
        {
            var oldServices = await _unitOfWork.ArtistServiceRepository.GetAsync(query => query.Where(c => c.ArtistId == artistId));
            
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

        private async Task AssignStore(Guid artistId, IList<ArtistStoreRequest> aSRs)
        {
            await ValidateTime(artistId, aSRs);

            var storeIds = aSRs.Select(c => c.StoreId).ToList();
            var today = DateOnly.FromDateTime(DateTime.Now);
            var minDateAllowed = today.AddDays(1); // Chỉ cho phép đổi lịch từ ngày mai trở đi

            // Lấy danh sách lịch làm cũ
            var oldArtistStores = await _unitOfWork.ArtistStoreRepository.GetAsync(query =>
                query.Where(c => c.ArtistId == artistId && storeIds.Contains(c.StoreId) && c.WorkingDate >= minDateAllowed));

            var newArtistStores = _mapper.Map<List<ArtistStore>>(aSRs);

            foreach (var old in oldArtistStores.OrderBy(c => c.WorkingDate).ThenBy(c => c.StartTime))
            {
                var matchingNew = newArtistStores.FirstOrDefault(newAS =>
                    newAS.StoreId == old.StoreId && newAS.WorkingDate == old.WorkingDate);

                if (matchingNew != null)
                {
                    // Nếu lịch mới trùng ngày & Store → Cập nhật StartTime, EndTime và đánh dấu không bị xóa
                    old.StartTime = matchingNew.StartTime;
                    old.EndTime = matchingNew.EndTime;
                    old.IsDeleted = false;

                    // Xóa khỏi danh sách mới để tránh chèn trùng
                    newArtistStores.Remove(matchingNew);
                }
                else
                {
                    // Nếu lịch cũ không còn khớp với lịch mới nào → Đánh dấu xóa
                    old.IsDeleted = true;
                }
            }

            // Cập nhật các lịch cũ bị thay đổi (nếu có)
            if (oldArtistStores.Any())
            {
                _unitOfWork.ArtistStoreRepository.UpdateRange(oldArtistStores.ToList());
            }

            // Thêm mới nếu có lịch mới chưa tồn tại trong DB
            if (newArtistStores.Count != 0)
            {
                newArtistStores.ForEach(c => c.ArtistId = artistId);
                await _unitOfWork.ArtistStoreRepository.InsertRangeAsync(newArtistStores);
            }
        }

        private async Task ValidateStore(IEnumerable<Guid> ids)
        {
            var stores = await _unitOfWork.StoreRepository.GetAsync( query => query.Where(c => !c.IsDeleted && ids.Contains(c.ID)));
            if (stores.Count() != ids.Count())
            {
                throw new Exception("Some store is not existed");
            }
        }
        
        private async Task ValidateTime(Guid artistId, IEnumerable<ArtistStoreRequest> aSR)
        {
            var changeDate = aSR.Select(c => c.WorkingDate).Distinct().Min();
            var today = DateOnly.FromDateTime(DateTime.Now);
            var minDateAllowed = today.AddDays(1);

            if (changeDate < minDateAllowed)
                throw new Exception("You can only change the working date at least 1 day before");

            var bookings = await _unitOfWork.BookingRepository.GetAsync( query => query
            .Include(c => c.ArtistStore)
            .Where(c => c.ServiceDate >= minDateAllowed && c!.ArtistStore!.ArtistId == artistId && aSR.Select(c => c.StoreId).Contains(c!.ArtistStore!.StoreId))
            );

            foreach (var booking in bookings)
            {
                foreach (var a in aSR)
                {
                    if (booking.ServiceDate == a.WorkingDate)
                    {
                        throw new Exception("You can not change this date because this date is already booked");
                    }
                }
            }
        }

        private async Task ValidateService(IEnumerable<Guid> serviceIds)
        {
            var service = await _unitOfWork.ServiceRepository.GetAsync( query => query.Where(c => !c.IsDeleted && serviceIds.Contains(c.ID)));
            if (service.Count() != serviceIds.Count())
            {
                throw new Exception("Some design is not existed");
            }
        }

        private async Task<string> GetUsername(string fullname)
        {
            var username = Utils.TransToUsername(fullname);
            
            var users = await _unitOfWork.ArtistRepository.GetAsync(c => c.Where(c => c.Username.Equals(username)));

            return users.Any() ? username += users.Count() : username;
        }

        public async Task<User> CreateUser(UserRequest userRequest)
        {
            try
            {
                await IsUniquePhoneNumber(userRequest.PhoneNumber);

                var user = _mapper.Map<User>(userRequest);

                user.PasswordHash = _authentication.HashedPassword(user, "password123!@#");

                user.Role = (int)Role.Artist;

                user.CreatedAt = DateTime.Now;

                if (userRequest.NewImage != null)
                    user.ImageUrl = await _firebaseService.UploadFileAsync(userRequest.NewImage);

                await _unitOfWork.UserRepository.InsertAsync(user);

                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ArtistResponse> Create(ArtistRequest artistRequest, UserRequest userRequest, IList<ArtistServiceRequest> artistServiceRequest, IList<ArtistStoreRequest> artistStoreRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (artistStoreRequest.Count != 0) 
                    await ValidateStore(artistStoreRequest.Select(c => c.StoreId));

                if(artistServiceRequest.Count != 0) 
                    await ValidateService(artistServiceRequest.Select(c => c.ServiceId));

                var user = await CreateUser(userRequest);

                var artist = _mapper.Map<Artist>(artistRequest);

                artist.ID = user.ID;

                artist.Username = await GetUsername(userRequest.FullName);

                await AssignStore(artist.ID, artistStoreRequest);

                if (artistServiceRequest.Count != 0)
                    await ValidateService(artistServiceRequest.Select(c => c.ServiceId));


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

        public IQueryable<ArtistResponse> Get()
        {
            try
            {
                return _unitOfWork.ArtistRepository.Query().ProjectTo<ArtistResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {
                return Enumerable.Empty<ArtistResponse>().AsQueryable();
            }
        }

        public async Task Update(Guid id, ArtistRequest artistRequest, UserRequest userRequest, IList<ArtistServiceRequest> artistServiceRequest, IList<ArtistStoreRequest> artistStoreRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var artist = await _unitOfWork.ArtistRepository.GetByIdAsync(id) ?? throw new Exception("This artist not found");

                if (artistStoreRequest.Count != 0)
                    await ValidateStore(artistStoreRequest.Select(c => c.StoreId));

                if (artistServiceRequest.Count != 0)
                    await ValidateService(artistServiceRequest.Select(c => c.ServiceId));

                await UpdateUser(id, userRequest);

                await _unitOfWork.ArtistRepository.UpdateAsync(_mapper.Map(artistRequest, artist));

                await AssignService(artist.ID, artistServiceRequest.Select(c => c.ServiceId).ToList());

                await AssignStore(artist.ID, artistStoreRequest);

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
