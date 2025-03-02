using AutoMapper;
using INBS.Application.DTOs.User.Artist.ArtistAvailability;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace INBS.Application.Services
{
    public class ArtistAvailabilityService(IUnitOfWork _unitOfWork, IMapper _mapper) : IArtistAvailabilityService
    {
        private static void IsLessThanMaximumBreakTime(ArtistAvailabilityRequest requestModel)
        {
            if (requestModel.MaximumBreakTime < requestModel.BreakTime)
            {
                throw new Exception("Break time must be less than maximum break time");
            }
        }

        public async Task Create(ArtistAvailabilityRequest requestModel)
        {
            try
            {
                IsLessThanMaximumBreakTime(requestModel);

                var result = await _unitOfWork.ArtistAvailabilityRepository.GetAsync(include: query => query.Where(aa => aa.ArtistId == requestModel.ArtistId && aa.AvailableDate == requestModel.AvailableDate));
                
                if (result.Any())
                {
                    throw new Exception("This artist availability already exists");
                }

                var artistAvailability = _mapper.Map<ArtistAvailability>(requestModel);

                artistAvailability.CreatedAt = DateTime.Now;

                await _unitOfWork.ArtistAvailabilityRepository.InsertAsync(artistAvailability);

                if (await _unitOfWork.SaveAsync() <= 0)
                    throw new Exception("This action failed");
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
                var result = await _unitOfWork.ArtistAvailabilityRepository.GetByIdAsync(id) ?? throw new Exception("This artist availability not found");

                result.IsDeleted = true;

                await _unitOfWork.ArtistAvailabilityRepository.UpdateAsync(result);

                if (await _unitOfWork.SaveAsync() <= 0)
                    throw new Exception("This action failed");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ArtistAvailabilityResponse>> Get()
        {
			try
			{
                var result = await _unitOfWork.ArtistAvailabilityRepository.GetAsync(include: query => 
                    query
                    .Include(aa => aa.Artist).ThenInclude(a => a!.User)
                    .Include(aa => aa.Bookings)
                    .Where(aa => !aa.IsDeleted && !aa.Artist!.User!.IsDeleted)
                    );
                return _mapper.Map<IEnumerable<ArtistAvailabilityResponse>>(result);
            }
			catch (Exception)
			{

				throw;
			}
        }

        public async Task Update(Guid id, ArtistAvailabilityRequest requestModel)
        {
            try
            {
                IsLessThanMaximumBreakTime(requestModel);

                var result = await _unitOfWork.ArtistAvailabilityRepository.GetByIdAsync(id) ?? throw new Exception("This artist availability not found");

                _mapper.Map(requestModel, result);

                await _unitOfWork.ArtistAvailabilityRepository.UpdateAsync(result);

                if (await _unitOfWork.SaveAsync() <= 0)
                    throw new Exception("This action failed");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
