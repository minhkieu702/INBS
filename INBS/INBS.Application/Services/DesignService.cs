using AutoMapper;
using INBS.Application.Common;
using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.Interfaces;
using INBS.Application.IService;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class DesignService(IUnitOfWork _unitOfWork, IMapper _mapper, IFirebaseService _firebaseService) : IDesignService
    {
        public async Task Create(DesignRequest modelRequest, IList<NewImageRequest> newImages)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.DesignRepository.GetAsync(x => x.Name.Equals(modelRequest.Name));

                if (existedEntity.Any())
                    throw new Exception("This design's name has been already used");

                var newEntity = _mapper.Map<Design>(modelRequest);
                newEntity.CreatedAt = DateTime.Now;

                await _unitOfWork.DesignRepository.InsertAsync(newEntity);

                await HandleInsertImage(newImages, newEntity.ID);
                await HandleInsertPreference(modelRequest, newEntity.ID);

                if (await _unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private static async Task<List<DesignPreference>> PreferencesList(Guid designID, DesignRequest modelRequest)
        {
            var (colors, occasions, paintTypes, skintones) = await Utils.GetPreferenceAsync();
            var preferenceEntities = new List<DesignPreference>();
            //color
            foreach (var id in modelRequest.ColorIds.Distinct())
            {
                if (!colors.Select(c => c.ID).Contains(id)) continue;

                var entity = new DesignPreference
                {
                    DesignId = designID,
                    PreferenceId = id,
                    PreferenceType = "Color"
                };
                preferenceEntities.Add(entity);
            }
            //occasion
            foreach (var id in modelRequest.OccasionIds.Distinct())
            {
                if (!occasions.Select(c => c.ID).Contains(id)) continue;

                var entity = new DesignPreference
                {
                    DesignId = designID,
                    PreferenceId = id,
                    PreferenceType = "Occasion"
                };
                preferenceEntities.Add(entity);
            }
            //skintone
            foreach (var id in modelRequest.SkintoneIds.Distinct())
            {
                if (!skintones.Select(c => c.ID).Contains(id)) continue;

                var entity = new DesignPreference
                {
                    DesignId = designID,
                    PreferenceId = id,
                    PreferenceType = "Skintone"
                };
                preferenceEntities.Add(entity);
            }
            //paint type
            foreach (var id in modelRequest.PaintTypeIds.Distinct())
            {
                if (!paintTypes.Select(c => c.ID).Contains(id)) continue;

                var entity = new DesignPreference
                {
                    DesignId = designID,
                    PreferenceId = id,
                    PreferenceType = "PaintType"
                };
                preferenceEntities.Add(entity);
            }
            return preferenceEntities;
        }

        private async Task HandleInsertPreference(DesignRequest modelRequest, Guid designID)
        {
            var requestList = await PreferencesList(designID, modelRequest);
            
            var designPreferences = await _unitOfWork.DesignPreferenceRepository.GetAsync(filter: dp => dp.DesignId == designID);

            if (designPreferences.Any())
                _unitOfWork.DesignPreferenceRepository.DeleteRange(designPreferences);
            
            if (requestList.Any())
                await _unitOfWork.DesignPreferenceRepository.InsertRangeAsync(requestList);
        }

        private async Task HandleInsertImage(IList<NewImageRequest> images, Guid designId)
        {
            if (!images.Any()) return;

            var list = new List<Image>();
            var semaphore = new SemaphoreSlim(5); // Giới hạn 5 tác vụ đồng thời (có thể thay đổi theo nhu cầu)

            var uploadTasks = images.Select(async image =>
            {
                await semaphore.WaitAsync(); // Chờ cho đến khi có slot trống
                try
                {
                    var entity = _mapper.Map<Image>(image);

                    entity.ImageUrl = image.Image != null ? await _firebaseService.UploadFileAsync(image.Image) : Constants.DEFAULT_IMAGE_URL; // Upload ảnh
                    entity.DesignId = designId;
                    entity.CreatedAt = DateTime.Now;

                    lock (list)
                    {
                        list.Add(entity); // Thêm ảnh vào danh sách (tránh xung đột)
                    }
                }
                finally
                {
                    semaphore.Release(); // Giải phóng slot
                }
            });

            await Task.WhenAll(uploadTasks); // Chờ tất cả tác vụ hoàn thành

            _unitOfWork.ImageRepository.InsertRange(list); // Thêm tất cả ảnh vào cơ sở dữ liệu
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

        public async Task<IEnumerable<DesignResponse>> Get()
        {
            var (colors, occasions, paintTypes, skintones) = await Utils.GetPreferenceAsync();

            var designs = await _unitOfWork.DesignRepository.GetAsync(include: 
                query => query
                .Include(d => d.Images.OrderBy(i => i.NumerialOrder))
                .Include(d => d.DesignPreferences)
                .Include(d => d.StoreDesigns.Where(sd => sd.Store != null && !sd.Store.IsDeleted))
                    .ThenInclude(sd => sd.Store)
                //.Include(d => d.CustomDesigns.Where(cd => cd.Design != null && !cd.Design.IsDeleted))
                //    .ThenInclude(cd => cd.AccessoryCustomDesigns.Where(cd => cd.Accessory != null && !cd.Accessory.IsDeleted))
                //        .ThenInclude(acd => acd.Accessory)
                //.Include(d => d.CustomDesigns.Where(cd => cd.Design != null && !cd.Design.IsDeleted))
                //    .ThenInclude(cd => cd.Customer)
                );

            var response = _mapper.Map<IEnumerable<DesignResponse>>(designs);

            foreach (var design in response)
            {
                foreach (var preference in design.DesignPreferences)
                {
                    switch (preference.PreferenceType)
                    {
                        case "Color": 
                            preference.Data = colors.FirstOrDefault(c => c.ID == preference.PreferenceId); 
                            break;

                        case "Occasion": preference.Data = occasions.FirstOrDefault(c => c.ID == preference.PreferenceId);
                            break;

                        case "Skintone": preference.Data = skintones.FirstOrDefault(c => c.ID == preference.PreferenceId);
                            break;

                        case "PaintType": preference.Data = paintTypes.FirstOrDefault(c => c.ID == preference.PreferenceId);
                            break;

                        default:
                            break;
                    }
                }
            }

            return response;
        }

        public async Task Update(Guid designId, DesignRequest modelRequest, IList<NewImageRequest> newImages, IList<ImageRequest> currentImages)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.DesignRepository.GetByIdAsync(designId) ?? throw new Exception(designId + " is not found");
                
                _mapper.Map(modelRequest, existedEntity);

                var currentList = await _unitOfWork.ImageRepository.GetAsync(filter: i => i.DesignId == designId);

                HandleDeleteImage(currentImages, designId, currentList);
                HandleUpdateImage(currentImages, designId, currentList);
                await HandleInsertImage(newImages, designId);
                await HandleInsertPreference(modelRequest, designId);
                await _unitOfWork.DesignRepository.UpdateAsync(existedEntity);

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

        private void HandleUpdateImage(IList<ImageRequest> currentImagesRequest, Guid designId, IEnumerable<Image> currentList)
        {
            var imageList = new List<Image>();
            foreach (var currentImage in currentList)
            {
                var updateEntity = currentImagesRequest.FirstOrDefault(c => c.ID == currentImage.ID);

                if (updateEntity == null) continue;

                if (updateEntity.NumerialOrder != currentImage.NumerialOrder ||
                    updateEntity.ImageUrl != currentImage.ImageUrl ||
                    updateEntity.Description != currentImage.Description)
                {
                    imageList.Add(_mapper.Map(updateEntity, currentImage));
                }
            }
            _unitOfWork.ImageRepository.UpdateRangeAsync(imageList);
        }

        private void HandleDeleteImage(IList<ImageRequest> currentImagesRequest, Guid designId, IEnumerable<Image> currentList)
        {
            var imageList = new List<Image>();
            foreach (var currentImage in currentList)
            {
                if (!currentImagesRequest.Select(c => c.ID).Contains(currentImage.ID))
                {
                    imageList.Add(currentImage);
                }
            }
            _unitOfWork.ImageRepository.DeleteRange(imageList);
        }
    }
}
