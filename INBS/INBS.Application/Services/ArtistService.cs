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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace INBS.Application.Services
{
    public class ArtistService(IUnitOfWork _unitOfWork, IFirebaseStorageService _firebaseService, IMapper _mapper, IAuthentication _authentication, IEmailSender _emailSender, IHttpContextAccessor _contextAccessor, INotificationHubService _notificationHubService, IFirebaseCloudMessageService _firebaseCloudMessageService) : IArtistService
    {

        private async Task IsUniquePhoneNumber(string phoneNumber, Guid? userId = null)
        {
            var artist = await _unitOfWork.UserRepository.GetAsync(query => query.Where(c => c.ID != userId && c.PhoneNumber == phoneNumber));
            if (artist.Any())
            {
                throw new Exception("Phone number already exists");
            }
        }

        private async Task AssignService(Guid artistId, IList<ArtistServiceRequest> services)
        {
            var oldServices = await _unitOfWork.ArtistServiceRepository.GetAsync(query => query.Where(c => c.ArtistId == artistId));
            
            if(oldServices.Any()) _unitOfWork.ArtistServiceRepository.DeleteRange(oldServices);

            var newServices = new List<Domain.Entities.ArtistService>();

            foreach (var service in services)
            {
                newServices.Add(new Domain.Entities.ArtistService
                {
                    ArtistId = artistId,
                    ServiceId = service.ServiceId
                });
            }

            await _unitOfWork.ArtistServiceRepository.InsertRangeAsync(newServices);
        }

        private async Task AssignStore(Guid artistId, IList<ArtistStoreRequest> aSRs)
        {
            var tomorrow = DateOnly.FromDateTime(DateTime.Now).AddDays(1);

            // Lấy danh sách lịch làm việc hiện tại từ DB
            var currentASs = await _unitOfWork.ArtistStoreRepository.GetAsync(query =>
                query.Where(c => c.ArtistId == artistId && c.WorkingDate >= tomorrow)
                     .Include(c => c.Bookings)
            );

            var holdList = new List<ArtistStore>();
            var deleteList = new List<ArtistStore>();
            var updateList = new List<ArtistStore>();
            var insertList = new List<ArtistStore>();

            foreach (var currAS in currentASs)
            {
                // Tìm ASR tương ứng trong danh sách mới
                var matchingASR = aSRs.FirstOrDefault(asr =>
                    asr.StoreId == currAS.StoreId &&
                    asr.WorkingDate == currAS.WorkingDate &&
                    asr.StartTime == currAS.StartTime &&
                    asr.EndTime == currAS.EndTime);

                if (matchingASR != null)
                {
                    // Trùng hoàn toàn => Giữ nguyên
                    holdList.Add(currAS);
                    continue;
                }

                // Nếu không có booking => Đưa vào danh sách xóa
                if (!currAS.Bookings.Any())
                {
                    deleteList.Add(currAS);
                    continue;
                }

                // Nếu có booking => Kiểm tra trùng lặp thời gian
                var conflictingASR = aSRs.FirstOrDefault(asr =>
                    asr.StoreId == currAS.StoreId &&
                    asr.WorkingDate == currAS.WorkingDate);

                if (conflictingASR != null)
                {
                    var isBookingInsideNewTime = currAS.Bookings.All(booking =>
                        booking.StartTime >= conflictingASR.StartTime &&
                        booking.PredictEndTime <= conflictingASR.EndTime);

                    if (isBookingInsideNewTime)
                    {
                        // Nếu booking nằm trong giờ mới => Đưa vào updateList
                        updateList.Add(currAS);
                    }
                    else
                    {
                        throw new Exception($"Conflict booking at store {currAS.StoreId} on {currAS.WorkingDate}.");
                    }
                }
            }

            // Các ASR không nằm trong holdList, updateList => Đưa vào insertList
            foreach (var asr in aSRs)
            {
                var exists = holdList.Any(h => h.StoreId == asr.StoreId && h.WorkingDate == asr.WorkingDate) ||
                             updateList.Any(u => u.StoreId == asr.StoreId && u.WorkingDate == asr.WorkingDate);

                if (!exists)
                {
                    var newAS = _mapper.Map<ArtistStore>(asr);
                    newAS.ArtistId = artistId;
                    insertList.Add(newAS);
                }
            }

            // Thực hiện cập nhật trong DB
            _unitOfWork.ArtistStoreRepository.UpdateRange(updateList);
            _unitOfWork.ArtistStoreRepository.DeleteRange(deleteList);
            _unitOfWork.ArtistStoreRepository.InsertRange(insertList);

            var role = _authentication.GetUserRoleFromHttpContext(_contextAccessor.HttpContext);
            if (role == (int)Role.Admin)
            {
                await HandleNotify(artistId, "You got new information", "Admin already assigned shift for you. Please check your shifts");
            }
            else
            {
                var admin = await _unitOfWork.UserRepository.GetAsync(c => c.Where(c => c.Role == (int)Role.Admin && !c.IsDeleted));
                if (!admin.Any())
                    throw new Exception("Admin not found");

                var artist = await _unitOfWork.UserRepository
                    .Query()
                    .Include(c => c.Artist)
                    .FirstOrDefaultAsync(c => c.ID == artistId)
                    ?? throw new Exception("Artist not found");

                await HandleNotify(admin.FirstOrDefault()!.ID, "You got new information", $"Artist {artist.FullName} - ${artist.Artist!.Username} assigned new shift");
            }
        }

        private async Task HandleNotify(Guid adminId, string title, string message)
        {
            var notification = new Notification
            {
                Content = message,
                Title = title,
                UserId = adminId,
                NotificationType = (int)NotificationType.Alert,
            };

            await _unitOfWork.NotificationRepository.InsertAsync(notification);

            var deviceTokens = await _unitOfWork.DeviceTokenRepository.Query()
                .Where(c => c.UserId == adminId && c.Platform == (int)DevicePlatformType.Web)
                .ToListAsync();

            await _firebaseCloudMessageService.SendToMultipleDevices(deviceTokens.Select(c => c.Token).ToList(), title, message);

            await _notificationHubService.NotifyArtistStoreIsCreated(adminId, title, message);

        }

        private async Task ValidateStore(IEnumerable<Guid> ids)
        {
            // Lấy danh sách ID từ database
            var existingIds = (await _unitOfWork.StoreRepository.GetAsync(
                query => query
                .Where(c => !c.IsDeleted && ids.Contains(c.ID)))).Select(c => c.ID);

            // Tìm các ID không tồn tại
            var missingIds = ids.Except(existingIds);

            if (missingIds.Any())
            {
                var missingIdsString = string.Join(", ", missingIds);
                throw new Exception($"The following stores do not exist: {missingIdsString}");
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
            
            var users = await _unitOfWork.ArtistRepository.GetAsync(c => c.Where(c => c.Username.Contains(username)));

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

                if (artistStoreRequest.Count != 0) 
                    await AssignStore(artist.ID, artistStoreRequest);

                if (artistServiceRequest.Count != 0)
                    await AssignService(artist.ID, artistServiceRequest);

                await _unitOfWork.ArtistRepository.InsertAsync(artist);

                if (await _unitOfWork.SaveAsync() <= 0)
                    throw new Exception("This action failed");

                await _emailSender.Send(null, userRequest.Email, "ACCOUNT TO LOG IN TO ARTIST PORTAL", Utils.GetHTMLForNewArtistAccount(artist.Username, "password123!@#"));
                
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
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id) ?? throw new Exception("This artist not found");

                user.IsDeleted = !user.IsDeleted;

                await _unitOfWork.UserRepository.UpdateAsync(user);

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
                var role = _authentication.GetUserRoleFromHttpContext(_contextAccessor.HttpContext);
                if (role == (int)Role.Admin)
                {
                    return _unitOfWork.ArtistRepository.Query().ProjectTo<ArtistResponse>(_mapper.ConfigurationProvider);
                }
                return _unitOfWork.ArtistRepository.Query().Include(c => c.User).Where(c => !c.User!.IsDeleted).ProjectTo<ArtistResponse>(_mapper.ConfigurationProvider);
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

                if (artistServiceRequest.Count != 0) 
                    await AssignService(artist.ID, artistServiceRequest);

                //if (artistStoreRequest.Count != 0) 
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
