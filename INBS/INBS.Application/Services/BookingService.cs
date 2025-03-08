using AutoMapper;
using INBS.Application.DTOs.Booking;
using INBS.Application.IService;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;

namespace INBS.Application.Services
{
    public class BookingService(IUnitOfWork _unitOfWork, IMapper _mapper) : IBookingService
    {
        private static long PredictTotalDuration(CustomCombo? customCombo)
        {
            if (customCombo == null) throw new Exception("Custom Combo not found");
            return customCombo.ServiceCustomCombos.Sum(static scc => scc.Service!.AverageDuration);
        }

        private async Task<IEnumerable<Booking>> GetAllBookingInThisDay(DateOnly serviceDate, TimeOnly serviceTime, Guid artistId)
        {
            return await _unitOfWork.BookingRepository.GetAsync(include: query =>
                query.Include(b => b.CustomCombo)
                        .ThenInclude(cc => cc!.ServiceCustomCombos.Where(scc => !scc.Service!.IsDeleted))
                            .ThenInclude(scc => scc.Service)
                    .Include(b => b.Artist)
                        .ThenInclude(a => a!.ArtistAvailabilities.Where(aa => !aa.IsDeleted && aa.AvailableDate == serviceDate))
            .Where(b => b.ServiceDate == serviceDate && b.ServiceTime >= serviceTime && b.ArtistId == artistId));
        }

        private async Task<CustomCombo> GetCustomCombo(Guid customComboId)
        {
            return (await _unitOfWork.CustomComboRepository.GetAsync(include: query =>
                query.Where(x => x.ID == customComboId && !x.IsDeleted)
                    .Include(x => x.ServiceCustomCombos.Where(scc => !scc.Service!.IsDeleted))
                        .ThenInclude(x => x.Service))).FirstOrDefault() ?? throw new Exception("Custom combo not found");
        }

        private async Task<CustomDesign> GetCustomDesign(Guid customDesignId, IEnumerable<Guid> serviceIds)
        {
            return (await _unitOfWork.CustomDesignRepository.GetAsync(include: query =>
                query.Where(x => x.ID == customDesignId && !x.IsDeleted)
                    .Include(x => x.Design)
                        .ThenInclude(x => x!.DesignServices.Where(ds => customDesignId == ds.DesignId && serviceIds.Contains(ds.ServiceId) && !ds.Service!.IsDeleted))
                    .Include(x => x.CustomNailDesigns)
                        .ThenInclude(x => x.AccessoryCustomNailDesigns.Where(acnd => !acnd.Accessory!.IsDeleted))
                            .ThenInclude(x => x.Accessory)
                    )).FirstOrDefault() ?? throw new Exception("Custom design not found");
        }

        private async Task<long> GetTotalAmount(Guid customDesignId, CustomCombo customCombo)
        {
            var services = customCombo.ServiceCustomCombos.Select(scc => scc.Service);

            var result = services.Sum(c => c!.Price);

            var customDesign = await GetCustomDesign(customDesignId, services.Select(c => c!.ID));

            result += customDesign.CustomNailDesigns.Sum(c => c.AccessoryCustomNailDesigns.Sum(acnd => acnd!.Accessory!.Price));

            result += customDesign!.Design!.DesignServices.Sum(c => c.ExtraPrice);

            return result;
        }

        public async Task Create(BookingRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var result = _mapper.Map<Booking>(request);

                var aa = _unitOfWork.ArtistAvailabilityRepository.Get(x => x.ArtistId == request.ArtistId && x.AvailableDate == request.ServiceDate && x.StartTime <= request.ServiceTime && request.ServiceTime <= x.EndTime).FirstOrDefault() ?? throw new Exception("Artist is not available at this time");

                var allBookingInThisDay = await GetAllBookingInThisDay(request.ServiceDate, request.ServiceTime, request.ArtistId);

                var allServicesInThisBooking = await GetCustomCombo(request.CustomComboId);

                result.Duration = PredictTotalDuration(allServicesInThisBooking);

                var endTime = request.ServiceTime.AddMinutes(result.Duration + aa.BreakTime);

                result.Status = (int)BookingStatus.isBooked;

                foreach (var booking in allBookingInThisDay)
                {
                    var otherStartTime = booking.ServiceTime;
                    var otherEndTime = otherStartTime.AddMinutes(PredictTotalDuration(booking.CustomCombo) + aa.BreakTime);

                    if ((otherStartTime <= request.ServiceTime && request.ServiceTime <= otherEndTime) || (otherStartTime <= endTime && endTime <= otherEndTime))
                    {
                        result.Status = (int)BookingStatus.isWating;
                    }
                }

                result.TotalAmount = await GetTotalAmount(request.CustomDesignId, allServicesInThisBooking);

                await _unitOfWork.BookingRepository.InsertAsync(result);

                if(await _unitOfWork.SaveAsync() <= 0)
                {
                    throw new Exception("Something was wrong");
                }

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public Task CancelBooking(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookingResponse>> Get()
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid id, BookingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
