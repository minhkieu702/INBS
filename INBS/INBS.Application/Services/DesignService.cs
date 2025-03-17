using AutoMapper;
using INBS.Application.Common;
using INBS.Domain.Enums;
using INBS.Application.Interfaces;
using INBS.Application.IService;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using INBS.Application.DTOs.Preference;
using INBS.Application.DTOs.Image;
using INBS.Application.DTOs.NailDesign;
using INBS.Application.DTOs.Design;
using Microsoft.AspNetCore.Http;
using AutoMapper.QueryableExtensions;

namespace INBS.Application.Services
{
    public class DesignService(IUnitOfWork _unitOfWork, IMapper _mapper, IFirebaseService _firebaseService) : IDesignService
    {
        private async Task HandleNailDesign(Guid designId, IList<NailDesignRequest> newList)
        {
            ValidateNailDesigns(newList);

            var oldList = await _unitOfWork.NailDesignRepository.GetAsync(query => query.Where(c => c.DesignId == designId));

            var updateList = new List<NailDesign>();
            var insertList = new List<NailDesign>();

            foreach (var newItem in newList)
            {
                var item = oldList.FirstOrDefault(c => c.NailPosition == newItem.NailPosition && c.IsLeft == newItem.IsLeft);

                if (item != null)
                {
                    _mapper.Map(newItem, item);

                    if (newItem.NewImage != null)
                    {
                        item.ImageUrl = await _firebaseService.UploadFileAsync(newItem.NewImage);
                    }

                    updateList.Add(item);

                    if (newItem.NailDesignServices.Any())
                    {
                        await HandleNailDesignService(item.ID, newItem.NailDesignServices);
                    }
                }
                else
                {
                    var newNailDesign = _mapper.Map<NailDesign>(newItem);

                    if (newItem.NewImage != null)
                    {
                        newNailDesign.ImageUrl = await _firebaseService.UploadFileAsync(newItem.NewImage);
                    }

                    newNailDesign.DesignId = designId;

                    insertList.Add(newNailDesign);

                    if (newItem.NailDesignServices.Any())
                    {
                        await HandleNailDesignService(newNailDesign.ID, newItem.NailDesignServices);
                    }
                }
            }
            if (insertList.Count != 0) _unitOfWork.NailDesignRepository.InsertRange(insertList);
            if (updateList.Count != 0) _unitOfWork.NailDesignRepository.UpdateRange(updateList);
        }

        private async Task HandleNailDesignService(Guid nailDesignId, IList<NailDesignServiceRequest> nailDesignServiceRequests)
        {
            await ValidateService(nailDesignServiceRequests.Select(c => c.ServiceId));

            var oldList = await _unitOfWork.NailDesignServiceRepository.GetAsync(
                query => query.Where(c => c.NailDesignId == nailDesignId));

            var updateList = new List<NailDesignService>();
            var insertList = new List<NailDesignService>();
            var processedItems = new HashSet<NailDesignServiceRequest>();
            var newServiceIds = nailDesignServiceRequests.Select(c => c.ServiceId).ToHashSet(); // Tạo HashSet để tối ưu tìm kiếm

            // 1️⃣ Duyệt danh sách cũ để cập nhật hoặc đánh dấu xóa
            foreach (var oldNDS in oldList)
            {
                if (newServiceIds.Contains(oldNDS.ServiceId))
                {
                    var newItem = nailDesignServiceRequests.First(c => c.ServiceId == oldNDS.ServiceId);
                    _mapper.Map(newItem, oldNDS);
                    updateList.Add(oldNDS);

                    // ✅ Thêm vào danh sách đã xử lý để tránh xóa trong vòng lặp
                    processedItems.Add(newItem);
                }
                else
                {
                    // Nếu serviceId cũ không có trong danh sách mới -> Đánh dấu xóa mềm
                    oldNDS.IsDeleted = true;
                    updateList.Add(oldNDS);
                }
            }

            // 2️⃣ Loại bỏ các phần tử đã xử lý khỏi danh sách mới
            nailDesignServiceRequests = nailDesignServiceRequests.Except(processedItems).ToList();

            // 3️⃣ Thêm mới các service chưa tồn tại
            if (nailDesignServiceRequests.Count != 0)
            {
                var newEntities = _mapper.Map<List<NailDesignService>>(nailDesignServiceRequests);
                newEntities.ForEach(n => n.NailDesignId = nailDesignId);
                insertList.AddRange(newEntities);
            }

            // 4️⃣ Thực hiện batch insert/update
            if (updateList.Count != 0) _unitOfWork.NailDesignServiceRepository.UpdateRange(updateList);
            if (insertList.Count != 0) _unitOfWork.NailDesignServiceRepository.InsertRange(insertList);
        }


        private async Task ValidateService(IEnumerable<Guid> serviceIds)
        {
            var service = await _unitOfWork.ServiceRepository.GetAsync(query => query.Where(c => !c.IsDeleted && serviceIds.Contains(c.ID)));
            if (service.Count() != serviceIds.Count())
            {
                throw new Exception("Some service is not existed");
            }
        }

        public async Task Create(DesignRequest modelRequest, PreferenceRequest preferenceRequest, IList<MediaRequest> images, IList<NailDesignRequest> nailDesigns)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.DesignRepository.GetAsync(c => c.Where(x => x.Name.Equals(modelRequest.Name)));

                if (existedEntity.Any())
                    throw new Exception("This design's name has been already used");

                var newEntity = _mapper.Map<Design>(modelRequest);
                newEntity.CreatedAt = DateTime.Now;

                await _unitOfWork.DesignRepository.InsertAsync(newEntity);

                if (images.Count != 0)
                {
                    await HandleMedias(newEntity.ID, images);
                }

                if (nailDesigns.Count != 0)
                {
                    await HandleNailDesign(newEntity.ID, nailDesigns);
                }

                await HandleUpdatePreference(preferenceRequest, newEntity.ID);

                if (await _unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private static async Task<List<Preference>> PreferencesList(Guid cusId, PreferenceRequest request)
        {
            var (colors, occasions, paintTypes, skintones) = await Utils.GetPreferenceAsync();

            var colorIds = colors.Select(c => c.ID).ToHashSet();
            var occasionIds = occasions.Select(c => c.ID).ToHashSet();
            var paintTypeIds = paintTypes.Select(c => c.ID).ToHashSet();
            var skintoneIds = skintones.Select(c => c.ID).ToHashSet();

            var preferences = new List<Preference>();

            // Hàm helper để thêm dữ liệu tránh lặp code
            void AddPreferences(IEnumerable<int> ids, PreferenceType type, HashSet<int> validIds)
            {
                preferences.AddRange(
                    ids.Distinct()
                       .Where(validIds.Contains) // Kiểm tra hợp lệ nhanh hơn
                .Select(id => new Preference
                       {
                           DesignId = cusId,
                           PreferenceId = id,
                           PreferenceType = (int)type
                       })
                );
            }

            // Áp dụng cho từng loại preference
            AddPreferences(request.ColorIds, PreferenceType.Color, colorIds);
            AddPreferences(request.OccasionIds, PreferenceType.Occasion, occasionIds);
            AddPreferences(request.PaintTypeIds, PreferenceType.PaintType, paintTypeIds);
            AddPreferences(request.SkintoneIds, PreferenceType.SkinTone, skintoneIds);

            return preferences;
        }

        private async Task HandleUpdatePreference(PreferenceRequest modelRequest, Guid designID)
        {
            var requestList = await PreferencesList(designID, modelRequest);
            
            var designPreferences = await _unitOfWork.PreferenceRepository.GetAsync(c => c.Where(dp => dp.DesignId == designID));

            if (designPreferences.Any())
                _unitOfWork.PreferenceRepository.DeleteRange(designPreferences);
            
            if (requestList.Count != 0)
                await _unitOfWork.PreferenceRepository.InsertRangeAsync(requestList);
        }

        public async Task Delete(Guid designId)
        {
            try
            {
                var existedEntity = await _unitOfWork.DesignRepository.GetByIdAsync(designId) ?? throw new Exception("Design " + designId + " is not found");

                existedEntity.IsDeleted = true;

                await _unitOfWork.DesignRepository.UpdateAsync(existedEntity);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("Delete failed");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<DesignResponse> Get()
        {
            try
            {
                return _unitOfWork.DesignRepository.Query().ProjectTo<DesignResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {

                throw;
            }
            //var (colors, occasions, paintTypes, skintones) = await Utils.GetPreferenceAsync();

            //var designs = await _unitOfWork.DesignRepository.GetAsync( 
            //    query => query
            //    .Include(d => d.Medias)
            //    .Include(d => d.Preferences)
            //    .Include(d => d.NailDesigns)
            //        .ThenInclude(nd => nd.NailDesignServices.Where(c => !c.Service!.IsDeleted))
            //            .ThenInclude(nds => nds.Service)
            //    .Include(d => d.NailDesigns)
            //        .ThenInclude(nd => nd.NailDesignServices)
            //            .ThenInclude(nds => nds.NailDesignServiceSelecteds)
            //                .ThenInclude(nds => nds.CustomerSelected)
            //                    .ThenInclude(cs => cs!.Customer)
            //                        .ThenInclude(c => c!.User)
            //    .AsNoTracking()
            //    );

            //var responses = _mapper.Map<IEnumerable<DesignResponse>>(designs);

            //foreach (var response in responses)
            //{
            //    var preferenceActions = new Dictionary<PreferenceType, Action<PreferenceResponse>>()
            //    {
            //        [PreferenceType.Color] = prefer => prefer.Data = colors.FirstOrDefault(c => c.ID == prefer.PreferenceId),

            //        [PreferenceType.Occasion] = prefer => prefer.Data = occasions.FirstOrDefault(c => c.ID == prefer.PreferenceId),

            //        [PreferenceType.PaintType] = prefer => prefer.Data = paintTypes.FirstOrDefault(c => c.ID == prefer.PreferenceId),

            //        [PreferenceType.SkinTone] = prefer => prefer.Data = skintones.FirstOrDefault(c => c.ID == prefer.PreferenceId)
            //    };

            //    foreach (var preference in response.Preferences)
            //    {
            //        if (Enum.TryParse(preference.PreferenceType, out PreferenceType type) && preferenceActions.TryGetValue(type, out var action))
            //        {
            //            action(preference);
            //        }
            //    }
            //}

            //return responses;
        }

        public async Task Update(Guid designId, DesignRequest modelRequest, PreferenceRequest preferenceRequest, IList<MediaRequest> images, IList<NailDesignRequest> nailDesigns)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.DesignRepository.GetByIdAsync(designId) ?? throw new Exception(designId + " is not found");
                
                _mapper.Map(modelRequest, existedEntity);

                await _unitOfWork.DesignRepository.UpdateAsync(existedEntity);

                await HandleMedias(designId, images);

                await HandleUpdatePreference(preferenceRequest, designId);

                await HandleNailDesign(designId, nailDesigns);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private static void ValidateNailDesigns(IEnumerable<NailDesignRequest> nailDesignsReq)
        {
            var seenPositions = new HashSet<(int, bool)>();

            foreach (var naildesign in nailDesignsReq)
            {
                if (!seenPositions.Add((naildesign.NailPosition, naildesign.IsLeft)))
                {
                    var hand = naildesign.IsLeft ? "left" : "right";
                    throw new Exception($"Invalid nail design: NailPosition {naildesign.NailPosition} on hand {hand}");
                }
            }
        }

        private static void ValidateMedia(IEnumerable<MediaRequest> imageReqs)
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

        private async Task HandleMedias(Guid designId, IList<MediaRequest> newList)
        {
            ValidateMedia(newList);

            var oldList = await _unitOfWork.MediaRepository.GetAsync(c => c.Where(m => m.DesignId == designId));

            var newIdsSet = newList.Select(c => c.NumerialOrder).ToHashSet();
            var processedItems = new HashSet<MediaRequest>();

            var updatingList = new List<Media>();
            var insertingList = new List<Media>();
            var deletingList = new List<Media>();
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
                var media = _mapper.Map<Media>(newItem);
                media.DesignId = designId;

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
            if (deletingList.Count != 0) _unitOfWork.MediaRepository.DeleteRange(deletingList);
            if (updatingList.Count != 0) _unitOfWork.MediaRepository.UpdateRange(updatingList);
            if (insertingList.Count != 0) _unitOfWork.MediaRepository.InsertRange(insertingList);
        }

        private async Task UploadImageAsync(IFormFile newImage, Media media)
        {
            media.ImageUrl = await _firebaseService.UploadFileAsync(newImage);
        }
    }
}
