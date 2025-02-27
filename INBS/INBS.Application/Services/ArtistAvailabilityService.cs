using AutoMapper;
using INBS.Application.DTOs.User.Artist.ArtistAvailability;
using INBS.Application.IServices;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class ArtistAvailabilityService(IUnitOfWork _unitOfWork, IMapper _mapper) : IArtistAvailabilityService
    {
        public Task Create(ArtistAvailabilityRequest requestModel)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ArtistAvailabilityResponse>> Get()
        {
			try
			{
                var result = await _unitOfWork.ArtistAvailabilityRepository.GetAsync(include: query => 
                    query
                    .Where(aa => !aa.IsDeleted && aa.Artist!.User!.IsDeleted)
                    .Include(aa => aa.Artist).ThenInclude(a => a!.User)
                    .Include(aa => aa.Bookings)
                    );
                return _mapper.Map<IEnumerable<ArtistAvailabilityResponse>>(result);
            }
			catch (Exception)
			{

				throw;
			}
        }

        public Task Update(Guid id, ArtistAvailabilityRequest requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
