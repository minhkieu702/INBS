using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.DTOs.PaymentDetail;
using INBS.Application.IServices;
using INBS.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class PaymentDetailService(IUnitOfWork _unitOfWork, IMapper _mapper) : IPaymentDetailService
    {
        public IQueryable<PaymentDetailResponse> Get()
        {
            try
            {
                return _unitOfWork.PaymentDetailRepository.Query().ProjectTo<PaymentDetailResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
