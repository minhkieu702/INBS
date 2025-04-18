using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.Common;
using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.Preference;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace INBS.Application.Services
{
    public class CustomerService(IUnitOfWork _unitOfWork, IMapper _mapper, IAuthentication _authentication, IHttpContextAccessor _contextAccessor) : ICustomerService
    {
        public async Task UpdatePreferencesAsync(CustomerPreferenceRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var id = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                var preferences = await _unitOfWork.PreferenceRepository.GetAsync(c => c.Where(c => c.CustomerId == id));

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

        private async Task<IList<Preference>> Mapping(Guid cusId, CustomerPreferenceRequest request)
        {
            var (colors, occasions, paintTypes, skintones) = await Utils.GetPreferenceAsync();

            var occasionIds = occasions.Select(c => c.ID).ToHashSet();
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
            AddPreferences(request.OccasionIds, PreferenceType.Occasion, occasionIds);
            AddPreferences(request.SkintoneIds, PreferenceType.SkinTone, skintoneIds);

            return preferences;
        }

        public IQueryable<CustomerResponse> Get()
        {
            try
            {
                return _unitOfWork.CustomerRepository.Query().ProjectTo<CustomerResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
