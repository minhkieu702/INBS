using AutoMapper;
using INBS.Application.Common;
using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.User.Customer;
using INBS.Application.DTOs.User.User;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
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
    public class CustomerService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication _authentication, IHttpContextAccessor _contextAccessor) : ICustomerService
    {
        public async Task UpdatePreferencesAsync(PreferencesRequest request)
        {
            try
            {
                unitOfWork.BeginTransaction();

                var id = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                var preferences = await unitOfWork.CustomerPreferenceRepository.GetAsync(c => c.CustomerId == id);

                if (preferences.Any()) unitOfWork.CustomerPreferenceRepository.DeleteRange(preferences);

                unitOfWork.CustomerPreferenceRepository.InsertRange(await Mapping(id, request));

                if (await unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                unitOfWork.RollBack();
                throw;
            }
        }

        private async Task<IList<CustomerPreference>> Mapping(Guid cusId, PreferencesRequest request)
        {
            var (colors, occasions, paintTypes, skintones) = await Utils.GetPreferenceAsync();

            var preferences = new List<CustomerPreference>();

            preferences.AddRange(request.ColorIds.Where(c => colors.Select(c => c.ID).Contains(c))
                .Select(c => new CustomerPreference
                    {
                        CustomerId = cusId,
                        PreferenceId = c,
                        PreferenceType = "Color"
                    }
                ));

            preferences.AddRange(request.OccasionIds.Where(c => occasions.Select(c => c.ID).Contains(c)).Select(c => new CustomerPreference
            {
                CustomerId = cusId,
                PreferenceId = c,
                PreferenceType = "Occasion"
            }));

            preferences.AddRange(request.PaintTypeIds.Where(c => paintTypes.Select(c => c.ID).Contains(c)).Select(c => new CustomerPreference
            {
                CustomerId = cusId,
                PreferenceId = c,
                PreferenceType = "PaintType"
            }));

            preferences.AddRange(request.SkintoneIds.Where(c => skintones.Select(c => c.ID).Contains(c)).Select(c => new CustomerPreference
            {
                CustomerId = cusId,
                PreferenceId = c,
                PreferenceType = "Skintone"
            }));

            return preferences;
        }

        public async Task<IEnumerable<CustomerResponse>> Get()
        {
            try
            {
                var (colors, occasions, paintTypes, skintones) = await Utils.GetPreferenceAsync();

                var result = await unitOfWork.CustomerRepository.GetAsync(include: query => query
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
                    .Include(c => c.CustomerPreferences)
                    .Include(c => c.DeviceTokens)
                );

                var responses = mapper.Map<IEnumerable<CustomerResponse>>(result);
                foreach (var response in responses)
                {
                    foreach (var preference in response.CustomerPreferences)
                    {
                        switch (preference.PreferenceType)
                        {
                            case "Color":
                                preference.Data = colors.FirstOrDefault(c => c.ID == preference.PreferenceId);
                                break;

                            case "Occasion":
                                preference.Data = occasions.FirstOrDefault(c => c.ID == preference.PreferenceId);
                                break;

                            case "Skintone":
                                preference.Data = skintones.FirstOrDefault(c => c.ID == preference.PreferenceId);
                                break;

                            case "PaintType":
                                preference.Data = paintTypes.FirstOrDefault(c => c.ID == preference.PreferenceId);
                                break;

                            default:
                                break;
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
