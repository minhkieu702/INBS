using AutoMapper;
using INBS.Application.DTOs.Customer;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
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

                return customer.Preferences;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
