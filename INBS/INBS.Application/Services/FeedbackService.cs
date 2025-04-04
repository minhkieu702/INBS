using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.Common;
using INBS.Application.DTOs.Feedback;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class FeedbackService(IUnitOfWork _unitOfWork, IAuthentication _authentication, IHttpContextAccessor _contextAccessor, IMapper _mapper, IFirebaseCloudMessageService _firebaseCloudMessageService) : IFeedbackService
    {
        private async Task NotifyArtist(Guid artistId)
        {
            try
            {
                var deviceTokens = _unitOfWork.DeviceTokenRepository.Query().Where(c => c.UserId == artistId).Select(c => c.Token);

                var notification = new Notification
                {
                    UserId = artistId,
                    Content = "You have a feedback",
                    Title = "YOU ARE RATED",
                    CreatedAt = DateTime.Now,
                    NotificationType = (int)NotificationType.Alert,
                };

                await _unitOfWork.NotificationRepository.InsertAsync(notification);

                await _firebaseCloudMessageService.SendToMultipleDevices(deviceTokens.ToList(), notification.Title, notification.Content);
            }
            catch (Exception)
            {
                return;
            }
        }
        private async Task HandleAritst(Guid artistId, int newRating, int count, int? oldRating = null, bool isDelete = false)
        {
            var artist = await _unitOfWork.ArtistRepository.GetByIdAsync(artistId) ?? throw new Exception("Artist not found");

            var averageRating = artist.AverageRating;

            if (isDelete && int.TryParse(oldRating.ToString(), out int result))
            {
                Utils.DeleteRating(result, ref count, ref averageRating);
            }
            else
            {
                Utils.UpdateRating(newRating, ref count, ref averageRating, oldRating);

                await NotifyArtist(artistId);
            }

            artist.AverageRating = averageRating;

            await _unitOfWork.ArtistRepository.UpdateAsync(artist);
        }
        
        private async Task HandleStore(Guid storeId, int newRating, int count, int? oldRating = null, bool isDelete = false)
        {
            var store = await _unitOfWork.StoreRepository.GetByIdAsync(storeId) ?? throw new Exception("Store not found");

            var averageRating = store.AverageRating;

            if (isDelete && int.TryParse(oldRating.ToString(), out int result))
            {
                Utils.DeleteRating(result, ref count, ref averageRating);
            }
            else
            {
                Utils.UpdateRating(newRating, ref count, ref averageRating, oldRating);
            }

            store.AverageRating = averageRating;

            await _unitOfWork.StoreRepository.UpdateAsync(store);
        }

        private async Task HandleDesign(Guid designId, int newRating, int count, int? oldRating = null, bool isDelete = false)
        {
            var design = await _unitOfWork.DesignRepository.GetByIdAsync(designId) ?? throw new Exception("Store not found");

            var averageRating = design.AverageRating;

            if (isDelete && int.TryParse(oldRating.ToString(), out int result))
            {
                Utils.DeleteRating(result, ref count, ref averageRating);
            }
            else
            {
                Utils.UpdateRating(newRating, ref count, ref averageRating, oldRating);
            }

            design.AverageRating = averageRating;

            await _unitOfWork.DesignRepository.UpdateAsync(design);
        }

        public async Task Create(FeedbackRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                
                var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                var count = _unitOfWork.FeedbackRepository.Query().Where(c => c.TypeId == request.TypeId && c.FeedbackType == (int)request.FeedbackType).Count();

                var feedback = _mapper.Map<Feedback>(request);

                feedback.CustomerId = userId;
                feedback.FeedbackType = (int)request.FeedbackType;

                switch (request.FeedbackType)
                {
                    case FeedbackType.Artist:
                        await HandleAritst(request.TypeId, request.Rating, count);
                        break;
                    case FeedbackType.Design:
                        await HandleDesign(request.TypeId, request.Rating, count); 
                        break;
                    case FeedbackType.Store:
                        await HandleStore(request.TypeId, request.Rating, count);
                        break;
                    default:
                        break;
                }

                await _unitOfWork.FeedbackRepository.InsertAsync(feedback);

                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("This action failed");
                }
                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                throw new Exception(ex.Message);
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(id) ?? throw new Exception("Feedback not found");

                var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                var count = _unitOfWork.FeedbackRepository.Query().Where(c => c.TypeId == feedback.TypeId && c.FeedbackType == feedback.FeedbackType).Count();

                switch (feedback.FeedbackType)
                {
                    case (int)FeedbackType.Artist:
                        await HandleAritst(feedback.TypeId, feedback.Rating, count, feedback.Rating, true);
                        break;
                    case (int)FeedbackType.Design:
                        await HandleDesign(feedback.TypeId, feedback.Rating, count, feedback.Rating, true);
                        break;
                    case (int)FeedbackType.Store:
                        await HandleStore(feedback.TypeId, feedback.Rating, count, feedback.Rating, true);
                        break;
                    default:
                        break;
                }

                await _unitOfWork.FeedbackRepository.DeleteAsync(feedback);

                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("This action failed");
                }
                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                throw new Exception(ex.Message);
            }
        }

        public IQueryable<FeedbackResponse> Get()
        {
            try
            {
                return _unitOfWork.FeedbackRepository.Query().ProjectTo<FeedbackResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Update(Guid id, FeedbackRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(id) ?? throw new Exception("Feedback not found");

                var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                var count = _unitOfWork.FeedbackRepository.Query().Where(c => c.TypeId == request.TypeId && c.FeedbackType == (int)request.FeedbackType).Count();

                switch (request.FeedbackType)
                {
                    case FeedbackType.Artist:
                        await HandleAritst(request.TypeId, request.Rating, count, feedback.Rating);
                        break;
                    case FeedbackType.Design:
                        await HandleDesign(request.TypeId, request.Rating, count, feedback.Rating);
                        break;
                    case FeedbackType.Store:
                        await HandleStore(request.TypeId, request.Rating, count, feedback.Rating);
                        break;
                    default:
                        break;
                }

                _mapper.Map(request, feedback);

                await _unitOfWork.FeedbackRepository.UpdateAsync(feedback);

                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("This action failed");
                }
                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                throw new Exception(ex.Message);
            }
        }
    }
}
