using AutoMapper;
using INBS.Application.Common;
using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.Interfaces;
using INBS.Application.IService;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class DesignService(IUnitOfWork _unitOfWork, IMapper _mapper, IFirebaseService _firebaseService) : IDesignService
    {
        public async Task Create(DesignRequest modelRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.DesignRepository.GetAsync(x => x.Name.Equals(modelRequest.Name));

                if (existedEntity.Any())
                    throw new Exception("This design's name has been already used");

                var newEntity = _mapper.Map<Design>(modelRequest);

                await _unitOfWork.DesignRepository.InsertAsync(newEntity);

                await HandleInsertImage(modelRequest.Images, newEntity.ID);
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

            await _unitOfWork.DesignPreferenceRepository.InsertRangeAsync(requestList);
        }

        private async Task HandleInsertImage(IList<ImageRequest> images, Guid iD)
        {
            if (!images.Any()) return;

            var list = new List<Image>();

            foreach (var image in images)
            {
                var entity = _mapper.Map<Image>(image);

                entity.ImageUrl = await _firebaseService.UploadFileAsync(image.Image);

                entity.DesignId = iD;

                list.Add(entity);
            }

            _unitOfWork.ImageRepository.InsertRange(list);
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

        public async Task Update(Guid designId, DesignRequest modelRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.DesignRepository.GetByIdAsync(designId) ?? throw new Exception(designId + " is not found");
                
                _mapper.Map(modelRequest, existedEntity);

                await HandleDeleteImage(modelRequest, designId);
                await HandleInsertImage(modelRequest.Images, designId);
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

        private async Task HandleDeleteImage(DesignRequest modelRequest, Guid designId)
        {
            var currentList = await _unitOfWork.ImageRepository.GetAsync(filter: i => i.ID == designId);

            var deleteList = currentList.Where(i => !modelRequest.ImageUrls.Contains(i.ImageUrl));

            _unitOfWork.ImageRepository.DeleteRange(deleteList);
        }
    }
}
