using AutoMapper;
using INBS.Application.Common;
using INBS.Application.DTOs.Design.Design;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Design.NailDesign;
using INBS.Application.Interfaces;
using INBS.Application.IService;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace INBS.Application.Services
{
    public class DesignService(IUnitOfWork _unitOfWork, IMapper _mapper, IFirebaseService _firebaseService) : IDesignService
    {
        public async Task Create(DesignRequest modelRequest, IList<ImageRequest> images, IList<NailDesignRequest> nailDesigns)
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

                if (images.Any())
                {
                    await HandleUpdateImage(images.OrderBy(c => c.NumerialOrder), newEntity.ID, []);
                }

                if (nailDesigns.Any())
                {
                    await HandleUpdateNailDesign(nailDesigns.OrderBy(c => (c.NailPosition, c.IsLeft)), newEntity.ID, []);
                }

                await HandleUpdatePreference(modelRequest, newEntity.ID);

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

        private async Task HandleUpdatePreference(DesignRequest modelRequest, Guid designID)
        {
            var requestList = await PreferencesList(designID, modelRequest);
            
            var designPreferences = await _unitOfWork.DesignPreferenceRepository.GetAsync(filter: dp => dp.DesignId == designID);

            if (designPreferences.Any())
                _unitOfWork.DesignPreferenceRepository.DeleteRange(designPreferences);
            
            if (requestList.Any())
                await _unitOfWork.DesignPreferenceRepository.InsertRangeAsync(requestList);
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
                .Include(d => d.NailDesigns)
                .Include(d => d.CustomDesigns.Where(cd => !cd.IsDeleted && !cd.Customer!.User!.IsDeleted))
                    .ThenInclude(cd => cd.Customer)
                        .ThenInclude(c => c!.User)
                .Include(d => d.CustomDesigns)
                    .ThenInclude(cd => cd.CustomNailDesigns)
                        .ThenInclude(cnd => cnd.AccessoryCustomNailDesigns)
                            .ThenInclude(acnd => acnd.Accessory)
                .Include(d => d.ArtistDesigns.Where(ad => !ad.Artist!.User!.IsDeleted))
                    .ThenInclude(ad => ad.Artist)
                        .ThenInclude(a => a!.User)
                .AsNoTracking()
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

        public async Task Update(Guid designId, DesignRequest modelRequest, IList<ImageRequest> images, IList<NailDesignRequest> nailDesigns)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.DesignRepository.GetByIdAsync(designId) ?? throw new Exception(designId + " is not found");
                
                _mapper.Map(modelRequest, existedEntity);

                var currentImagesList = await _unitOfWork.ImageRepository.GetAsync(filter: i => i.DesignId == designId);

                var currentNailDesignList = await _unitOfWork.NailDesignRepository.GetAsync(filter: nd => nd.DesignId == designId);

                await HandleUpdateImage(
                    images.OrderBy(c => c.NumerialOrder), 
                    designId,
                    currentImagesList.OrderBy(c => c.NumerialOrder).ToDictionary(c => (c.NumerialOrder)));

                await HandleUpdatePreference(modelRequest, designId);

                await HandleUpdateNailDesign(
                    nailDesigns.OrderBy(c => c.NailPosition), 
                    designId,
                    currentNailDesignList.OrderBy(c => c.NailPosition).ToDictionary(c => (c.NailPosition, c.IsLeft)));

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

        private async Task HandleUpdateNailDesign(IEnumerable<NailDesignRequest> nailDesignsReq, Guid
             designId, Dictionary<(int, bool), NailDesign> nailDesignDict)
        {
            ValidateNailDesigns(nailDesignsReq);

            var updatinglist = new List<NailDesign>();
            var insertingList = new List<NailDesign>();
            var uploadTasks = new List<Task>(); // Danh sách các task upload ảnh (chạy đồng thời)

            foreach (var nailDesignRq in nailDesignsReq)
            {
                // Lấy NailDesign hiện tại từ dictionary, kiểm tra nếu tồn tại
                if (nailDesignDict.TryGetValue((nailDesignRq.NailPosition, nailDesignRq.IsLeft), out var nailDesign))
                {
                    // Nếu có ảnh mới, upload ảnh đồng thời
                    if (nailDesignRq.NewImage != null)
                    {
                        var uploadTask = _firebaseService
                            .UploadFileAsync(nailDesignRq.NewImage)
                            .ContinueWith(t => nailDesign.ImageUrl = t.Result);

                        uploadTasks.Add(uploadTask);
                    }
                    else
                    {
                        nailDesign.ImageUrl = nailDesignRq.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                    }

                    updatinglist.Add(nailDesign);
                }
                else
                {
                    nailDesign = _mapper.Map<NailDesign>(nailDesignRq);
                    nailDesign.DesignId = designId;

                    if (nailDesignRq.NewImage != null)
                    {
                        var uploadTask = _firebaseService
                            .UploadFileAsync(nailDesignRq.NewImage)
                            .ContinueWith(t => nailDesign.ImageUrl = t.Result);
                        
                        uploadTasks.Add(uploadTask);
                    }
                    else
                    {
                        nailDesign.ImageUrl = nailDesignRq.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                    }

                    insertingList.Add(nailDesign);
                }
            }

            // Đợi tất cả ảnh upload xong
            await Task.WhenAll(uploadTasks);

            if(insertingList.Count != 0) _unitOfWork.NailDesignRepository.InsertRange(insertingList);
            
            if(updatinglist.Count != 0) _unitOfWork.NailDesignRepository.UpdateRange(updatinglist);
        }

        private void ValidateImages(IEnumerable<ImageRequest> imageReqs)
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

        private async Task HandleUpdateImage(IEnumerable<ImageRequest> imageReqs,Guid designId, Dictionary<int, Image> currentImageList)
        {

            ValidateImages(imageReqs);

            var updatingList = new List<Image>();
            var insertingList = new List<Image>();
            var uploadTasks = new List<Task>(); // Danh sách task upload ảnh

            foreach (var imgReq in imageReqs)
            {
                if (currentImageList.TryGetValue(imgReq.NumerialOrder, out var image)) // Ảnh đã tồn tại
                {
                    if (imgReq.NewImage != null)
                    {
                        // Upload ảnh đồng thời
                        var uploadTask = _firebaseService.UploadFileAsync(imgReq.NewImage)
                            .ContinueWith(t => image.ImageUrl = t.Result);
                        uploadTasks.Add(uploadTask);
                    }
                    else
                    {
                        image.ImageUrl = imgReq.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                    }

                    updatingList.Add(image);
                }
                else // Ảnh mới cần thêm vào
                {
                    image = _mapper.Map<Image>(imgReq);
                    image.DesignId = designId;

                    if (imgReq.NewImage != null)
                    {
                        // Upload ảnh đồng thời
                        var uploadTask = _firebaseService.UploadFileAsync(imgReq.NewImage)
                            .ContinueWith(t => image.ImageUrl = t.Result);
                        uploadTasks.Add(uploadTask);
                    }
                    else
                    {
                        image.ImageUrl = imgReq.ImageUrl ?? Constants.DEFAULT_IMAGE_URL;
                    }

                    insertingList.Add(image);
                }
            }

            // Đợi tất cả ảnh upload hoàn thành
            await Task.WhenAll(uploadTasks);

            // Xóa ảnh không còn tồn tại trong danh sách đầu vào
            var deleteList = currentImageList.Values
                .Where(img => !imageReqs.Select(req => req.NumerialOrder).Contains(img.NumerialOrder))
                .ToList();

            if(deleteList.Count != 0) _unitOfWork.ImageRepository.DeleteRange(deleteList);
            if (updatingList.Count != 0) _unitOfWork.ImageRepository.UpdateRange(updatingList);
            if (insertingList.Count != 0) _unitOfWork.ImageRepository.InsertRange(insertingList);
        }
    }
}
