using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.Common;
using INBS.Application.DTOs.Feedback;
using INBS.Application.DTOs.FeedbackImage;
using INBS.Application.DTOs.Image;
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
    public class FeedbackService(IUnitOfWork _unitOfWork, IAuthentication _authentication, IHttpContextAccessor _contextAccessor, IMapper _mapper, IFirebaseCloudMessageService _firebaseCloudMessageService, INotificationHubService _notificationHubService, IFirebaseStorageService _firebaseService) : IFeedbackService
    {
        private async Task NotifyArtist(Guid artistId, Guid bookingId)
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
                    WebHref = $"/booking/:{bookingId}",
                };

                await _unitOfWork.NotificationRepository.InsertAsync(notification);

                await _firebaseCloudMessageService.SendToMultipleDevices(deviceTokens.ToList(), notification.Title, notification.Content);

                await _notificationHubService.NotifyFeedback(artistId, notification.Title, notification.Content);
            }
            catch (Exception)
            {
                return;
            }
        }
        private async Task HandleAritst(Guid artistId, Guid bookingId, int newRating, int count, int? oldRating = null, bool isDelete = false)
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

                await NotifyArtist(artistId, bookingId);
            }

            artist.AverageRating = averageRating;

            await _unitOfWork.ArtistRepository.UpdateAsync(artist);
        }
        
        private async Task HandleStore(Guid storeId, Guid bookingId, int newRating, int count, int? oldRating = null, bool isDelete = false)
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

        private async Task HandleDesign(Guid designId, Guid bookingId, int newRating, int count, int? oldRating = null, bool isDelete = false)
        {
            var design = await _unitOfWork.DesignRepository.GetByIdAsync(designId) ?? throw new Exception("Design not found");

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

        private static void ValidateFeedbackImage(IEnumerable<FeedbackImageRequest> imageReqs)
        {
            var seenOrders = new HashSet<int>();

            foreach (var img in imageReqs)
            {
                if (!seenOrders.Add(img.NumerialOrder)) // Nếu đã tồn tại, Add() trả về false
                {
                    throw new Exception($"Duplicate NumerialOrder found: {img.NumerialOrder}");
                }
            }
        }

        private async Task HandleFeedbackImages(Guid feedbackId, IList<FeedbackImageRequest> newList)
        {
            ValidateFeedbackImage(newList);

            var oldList = await _unitOfWork.FeedbackImageRepository.GetAsync(c => c.Where(m => m.FeedbackId == feedbackId));

            var newIdsSet = newList.Select(c => c.NumerialOrder).ToHashSet();
            var processedItems = new HashSet<FeedbackImageRequest>();

            var updatingList = new List<FeedbackImage>();
            var insertingList = new List<FeedbackImage>();
            var deletingList = new List<FeedbackImage>();
            var uploadTasks = new List<Task>();  // 🔥 Chạy upload ảnh song song để tối ưu tốc độ

            // 1️⃣ Duyệt danh sách cũ để cập nhật hoặc xóa
            foreach (var oldItem in oldList)
            {
                if (newIdsSet.Contains(oldItem.NumerialOrder))
                {
                    var newItem = newList.First(c => c.NumerialOrder == oldItem.NumerialOrder);

                    _mapper.Map(newItem, oldItem);

                    if (newItem.NewImage != null)
                    {
                        var uploadTask = UploadImageAsync(newItem.NewImage, oldItem);  // 🔥 Tạo Task thay vì `await` ngay
                        uploadTasks.Add(uploadTask);
                    }

                    updatingList.Add(oldItem);
                    processedItems.Add(newItem);  // ✅ Thêm vào danh sách đã xử lý
                }
                else
                {
                    deletingList.Add(oldItem);
                }
            }

            // 2️⃣ Xóa tất cả phần tử đã xử lý khỏi danh sách mới
            newList = newList.Except(processedItems).ToList();

            // 3️⃣ Duyệt danh sách mới để thêm Media mới
            foreach (var newItem in newList)
            {
                var media = _mapper.Map<FeedbackImage>(newItem);
                media.FeedbackId = feedbackId;

                if (newItem.NewImage != null)
                {
                    var uploadTask = UploadImageAsync(newItem.NewImage, media);
                    uploadTasks.Add(uploadTask);
                }

                insertingList.Add(media);
            }


            // 🔥 Chạy upload ảnh song song để tối ưu tốc độ
            if (uploadTasks.Count != 0) await Task.WhenAll(uploadTasks);

            // 4️⃣ Xử lý batch cập nhật vào DB
            if (deletingList.Count != 0) _unitOfWork.FeedbackImageRepository.DeleteRange(deletingList);
            if (updatingList.Count != 0) _unitOfWork.FeedbackImageRepository.UpdateRange(updatingList);
            if (insertingList.Count != 0) _unitOfWork.FeedbackImageRepository.InsertRange(insertingList);
        }

        private async Task UploadImageAsync(IFormFile newImage, FeedbackImage media)
        {
            media.ImageUrl = await _firebaseService.UploadFileAsync(newImage);
        }

        public async Task Create(FeedbackRequest request, IList<FeedbackImageRequest> feedbackImages)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                
                var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                var count = _unitOfWork.FeedbackRepository.Query().Where(c => c.TypeId == request.TypeId && c.FeedbackType == (int)request.FeedbackType).Count();

                var feedback = _mapper.Map<Feedback>(request);

                feedback.CustomerId = userId;
                feedback.FeedbackType = (int)request.FeedbackType;

                await HandleFeedbackImages(feedback.ID, feedbackImages);
                
                switch (request.FeedbackType)
                {
                    case FeedbackType.Artist:
                        await HandleAritst(request.TypeId, request.BookingId, request.Rating, count);
                        break;
                    case FeedbackType.Design:
                        await HandleDesign(request.TypeId, request.BookingId, request.Rating, count); 
                        break;
                    case FeedbackType.Store:
                        await HandleStore(request.TypeId, request.BookingId, request.Rating, count);
                        break;
                    default:
                        break;
                }

                await _unitOfWork.FeedbackRepository.InsertAsync(feedback);

                await HandleFavoriteBooking(request);

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

        private async Task HandleFavoriteBooking(FeedbackRequest request)
        {
            var booking = await _unitOfWork.BookingRepository.Query()
                .Include(c => c.ArtistStore)
                    .ThenInclude(c => c!.Artist)
                .Include(c => c.ArtistStore)
                    .ThenInclude(c => c!.Store)
                .Include(c => c.CustomerSelected)
                    .ThenInclude(c => c!.NailDesignServiceSelecteds)
                        .ThenInclude(c => c!.NailDesignService)
                            .ThenInclude(c => c!.NailDesign)
                                .ThenInclude(c => c!.Design)
                .FirstOrDefaultAsync(c => c.ID == request.BookingId);

            var customerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

            if (booking == null) return;

            var selectedDesignIds = booking.CustomerSelected!.NailDesignServiceSelecteds
                .Select(c => c.NailDesignService!.NailDesign!.DesignId).Distinct().ToList();

            var feedbacks = await _unitOfWork.FeedbackRepository.Query()
                .Where(c
                => (c.BookingId == request.BookingId && c.CustomerId == customerId && c.FeedbackType == (int)FeedbackType.Artist && c.TypeId == booking.ArtistStore!.ArtistId)
                || (c.BookingId == request.BookingId && c.CustomerId == customerId && c.FeedbackType == (int)FeedbackType.Store && c.TypeId == booking.ArtistStore!.StoreId)
                || (c.BookingId == request.BookingId && c.CustomerId == customerId && c.FeedbackType == (int)FeedbackType.Design && selectedDesignIds.Contains(c.TypeId))
                )
                .ToListAsync();

            var designFeedbacks = new List<Feedback>();
            var isFavoriteArtist = false;
            var isFavoriteStore = false;

            feedbacks.ForEach(c =>
            {
                if (c.FeedbackType == (int)FeedbackType.Artist && c.Rating == 5)
                {
                    isFavoriteArtist = true;
                }
                else if (c.FeedbackType == (int)FeedbackType.Store && c.Rating == 5)
                {
                    isFavoriteStore = true;
                }
                else if (c.FeedbackType == (int)FeedbackType.Design)
                {
                    designFeedbacks.Add(c);
                }
            });

            var isFavoriteDesign = selectedDesignIds.All(designId =>
                designFeedbacks.Any(f => f.TypeId == designId && f.Rating == 5));

            if (isFavoriteStore && isFavoriteDesign && isFavoriteArtist)
            {
                booking.IsFavorite = true;
                _unitOfWork.BookingRepository.Update(booking);
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
                        await HandleAritst(feedback.TypeId, feedback.BookingId, feedback.Rating, count, feedback.Rating, true);
                        break;
                    case (int)FeedbackType.Design:
                        await HandleDesign(feedback.TypeId, feedback.BookingId, feedback.Rating, count, feedback.Rating, true);
                        break;
                    case (int)FeedbackType.Store:
                        await HandleStore(feedback.TypeId, feedback.BookingId, feedback.Rating, count, feedback.Rating, true);
                        break;
                    default:
                        break;
                }

                await _unitOfWork.FeedbackRepository.DeleteAsync(feedback.ID);

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

        public async Task Update(Guid id, FeedbackRequest request, IList<FeedbackImageRequest> feedbackImages)
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
                        await HandleAritst(request.TypeId, request.BookingId, request.Rating, count, feedback.Rating);
                        break;
                    case FeedbackType.Design:
                        await HandleDesign(request.TypeId, request.BookingId, request.Rating, count, feedback.Rating);
                        break;
                    case FeedbackType.Store:
                        await HandleStore(request.TypeId, request.BookingId, request.Rating, count, feedback.Rating);
                        break;
                    default:
                        break;
                }

                _mapper.Map(request, feedback);

                await HandleFeedbackImages(feedback.ID, feedbackImages);

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
