using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.ArtistStore;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class ArtistStoreService(IUnitOfWork _unitOfWork, IMapper _mapper, IAuthentication _authentication, IHttpContextAccessor _contextAccessor, IFirebaseCloudMessageService _fcmService, INotificationHubService _notificationHubService) : IArtistStoreService
    {
        public IQueryable<ArtistStoreResponse> GetAll()
        {
            var role = _authentication.GetUserRoleFromHttpContext(_contextAccessor.HttpContext);
            if (role == 2)
            {
                return _unitOfWork.ArtistStoreRepository.Query().ProjectTo<ArtistStoreResponse>(_mapper.ConfigurationProvider);
            }
            if (role == 1)
            {
                return _unitOfWork.ArtistStoreRepository.Query().Where(c => !c.IsDeleted).ProjectTo<ArtistStoreResponse>(_mapper.ConfigurationProvider);

            }
            return _unitOfWork.ArtistStoreRepository.Query().Where(c => !c.IsDeleted && c.Status == (int)ArtistStoreStatus.Approved).ProjectTo<ArtistStoreResponse>(_mapper.ConfigurationProvider);
        }

        public async Task Update(Guid id, ArtistStoreStatus artistStoreStatus)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var artistStore = await _unitOfWork.ArtistStoreRepository.GetByIdAsync(id);
                if (artistStore == null)
                {
                    throw new Exception("Artist store not found");
                }
                artistStore.Status = (int)artistStoreStatus;
                _unitOfWork.ArtistStoreRepository.Update(artistStore);

                switch (artistStoreStatus)
                {
                    case ArtistStoreStatus.Rejected:
                        await SendNotificationWorkingDateToArtist(artistStore, "Your shift has been rejected", "Your shift has been rejected. Please check your profile for more details.");
                        break;
                    case ArtistStoreStatus.Approved:
                        await SendNotificationWorkingDateToArtist(artistStore, "Your shift has been approved", "Your shift has been approved. Please check your profile for more details.");
                        break;
                    default:
                        break;
                }

                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("This action failed");
                }
                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private async Task SendNotificationWorkingDateToArtist(ArtistStore artistStore, string title, string body)
        {
            var deviceTokens = await _unitOfWork.DeviceTokenRepository.Query().Where(c => c.UserId == artistStore.ArtistId && c.Platform == (int)DevicePlatformType.Web).ToListAsync();
            
            var notification = new Notification
            {
                Title = title,
                Content = body,
                NotificationType = (int)NotificationType.Alert,
                CreatedAt = DateTime.UtcNow,
                UserId = artistStore.ArtistId
            };

            _unitOfWork.NotificationRepository.Insert(notification);

            if (deviceTokens.Count == 0) return /*throw new Exception("Can't send notification to artist, because artist does not have device")*/;

            await _fcmService.SendToMultipleDevices(deviceTokens.Select(c => c.Token).ToList(), title, body);

            await _notificationHubService.NotifyArtistStoreUpdated(artistStore.ArtistId, title, body);
        }
    }
}
