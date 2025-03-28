using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.Cart;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class CartService(IUnitOfWork _unitOfWork, IMapper _mapper, IAuthentication _authentication, IHttpContextAccessor _contextAccessor) : ICartService
    {
        public IQueryable<CartResponse> Get()
        {
			try
			{
				return _unitOfWork.CartRepository.Query().ProjectTo<CartResponse>(_mapper.ConfigurationProvider);
			}
			catch (Exception)
			{

				throw;
			}
        }

        public async Task Create(CartRequest cartRequest)
        {
            try
            {
                var customerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                var isExist = await _unitOfWork.CartRepository.GetAsync(c => c.Where(c => c.NailDesignServiceId == cartRequest.NailDesignServiceId && customerId == c.CustomerId));

                if (isExist.Any())
                {
                    throw new Exception("This nail design service already exist in cart");
                }

                var isValidate = await _unitOfWork.NailDesignServiceRepository.GetByIdAsync(cartRequest.NailDesignServiceId) ?? throw new Exception("This nail design service not found");

                var cart = _mapper.Map<Cart>(cartRequest);

                cart.CustomerId = customerId;

                await _unitOfWork.CartRepository.InsertAsync(cart);

                if (await _unitOfWork.SaveAsync() ==0)
                {
                    throw new Exception("This action failed");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var isExist = (await _unitOfWork.CartRepository.GetAsync(c => c.Where(c => c.NailDesignServiceId == id))).FirstOrDefault();

                if (isExist == null)
                {
                    throw new Exception("This nail design service not found in cart");
                }

                _unitOfWork.CartRepository.Delete(isExist);

                if (await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("This action failed");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
