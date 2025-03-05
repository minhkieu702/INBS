using AutoMapper;
using INBS.Application.Common;
using INBS.Domain.Enums;
using INBS.Application.DTOs.Common.Preference;
using INBS.Application.DTOs.User.Customer;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace INBS.Application.Services
{
    public class CustomerService(IUnitOfWork _unitOfWork, IMapper _mapper, IAuthentication _authentication, IHttpContextAccessor _contextAccessor) : ICustomerService
    {
        public async Task UpdatePreferencesAsync(PreferenceRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var id = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                var preferences = await _unitOfWork.PreferenceRepository.GetAsync(c => c.CustomerId == id);

                if (preferences.Any()) _unitOfWork.PreferenceRepository.DeleteRange(preferences);

                _unitOfWork.PreferenceRepository.InsertRange(await Mapping(id, request));

                if (await _unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private async Task<IList<Preference>> Mapping(Guid cusId, PreferenceRequest request)
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
                           CustomerId = cusId,
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

        public async Task<IEnumerable<CustomerResponse>> Get()
        {
            try
            {
                var (colors, occasions, paintTypes, skintones) = await Utils.GetPreferenceAsync();

                var result = await _unitOfWork.CustomerRepository.GetAsync(include: query => query
                    .Where(u => !u.User!.IsDeleted)
                    .Include(c => c.User)
                        .ThenInclude(c => c!.Notifications.Where(n => !n.IsDeleted))
                    .Include(c => c.CustomDesigns.Where(n => !n.IsDeleted))
                        .ThenInclude(c => c.CustomNailDesigns)
                            .ThenInclude(c => c.AccessoryCustomNailDesigns)
                                .ThenInclude(c => c.Accessory)
                    .Include(c => c.CustomCombos.Where(n => !n.IsDeleted))
                        .ThenInclude(c => c.ServiceCustomCombos)
                            .ThenInclude(c => c.Service)
                    .Include(c => c.Preferences)
                    .Include(c => c.DeviceTokens)
                );

                var responses = _mapper.Map<IEnumerable<CustomerResponse>>(result);
                foreach (var response in responses)
                {
                    var preferenceActions = new Dictionary<PreferenceType, Action<PreferenceResponse>>()
                    {
                        [PreferenceType.Color] = prefer => prefer.Data = colors.FirstOrDefault(c => c.ID == prefer.PreferenceId),

                        [PreferenceType.Occasion] = prefer => prefer.Data = occasions.FirstOrDefault(c => c.ID == prefer.PreferenceId),

                        [PreferenceType.PaintType] = prefer => prefer.Data = paintTypes.FirstOrDefault(c => c.ID == prefer.PreferenceId),

                        [PreferenceType.SkinTone] = prefer => prefer.Data = skintones.FirstOrDefault(c => c.ID == prefer.PreferenceId)
                    };

                    foreach (var preference in response.CustomerPreferences)
                    {
                        if (Enum.TryParse(preference.PreferenceType, out PreferenceType type) && preferenceActions.TryGetValue(type, out var action))
                        {
                            action(preference);
                        }
                    }
                }

                return responses;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
