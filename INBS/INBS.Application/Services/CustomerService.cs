using AutoMapper;
using INBS.Application.Common;
using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.User.Customer;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Customer> UpdatePreferencesAsync(Guid customerId, PreferencesRequest request)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);

                if (customer == null)
                    throw new KeyNotFoundException("Customer not found");

                customer.Preferences = request.Preferences;

                await _unitOfWork.CustomerRepository.UpdateAsync(customer);
                await _unitOfWork.SaveAsync();

                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string> GetPreferencesAsync(Guid customerId)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
                if (customer == null)
                    throw new KeyNotFoundException("Customer not found");

                if (customer.Preferences == null)
                {
                    return string.Empty;
                }

                return customer.Preferences;
            }
            catch (Exception)
            {
                throw;
            }
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
                    .Include(c => c.CustomerPreferences)
                    .Include(c => c.DeviceTokens)
                );

                var responses = _mapper.Map<IEnumerable<CustomerResponse>>(result);
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
