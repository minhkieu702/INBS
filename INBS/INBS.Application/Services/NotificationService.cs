using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.DeviceToken;
using INBS.Application.DTOs.Notification;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task SendNotificationAsync(string phoneNumber, string message)
        {
            // Giả lập gửi tin nhắn (có thể thay bằng Twilio, Firebase, v.v.)
            _logger.LogInformation($"Sending SMS to {phoneNumber}: {message}");
            await Task.CompletedTask;
        }

        public IQueryable<NotificationResponse> Get()
        {
            try
            {
                return _unitOfWork.NotificationRepository.Query().ProjectTo<NotificationResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task MarkSeenNotification(Guid notificationId)
        {
            try
            {
                var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(notificationId) ?? throw new Exception("Notification not found");

                notification.Status = (int)NotificationStatus.Read;

                await _unitOfWork.NotificationRepository.UpdateAsync(notification);

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
    }
}
