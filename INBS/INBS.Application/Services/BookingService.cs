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
        public Task CancelBooking(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Create(BookingRequest request)
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
    //{
    //    private static long PredictTotalDuration(CustomerSelected? customCombo)
    //    {
    //        if (customCombo == null) throw new Exception("Custom Combo not found");
    //        return customCombo.ServiceCustomCombos.Sum(static scc => scc.Service!.AverageDuration);
    //    }

    //    private async Task<IEnumerable<Booking>> GetAllBookingInThisDay(DateOnly serviceDate, TimeOnly serviceTime, Guid artistId)
    //    {
    //        return await _unitOfWork.BookingRepository.GetAsync( query =>
    //            query.Include(b => b.CustomCombo)
    //                    .ThenInclude(cc => cc!.ServiceCustomCombos.Where(scc => !scc.Service!.IsDeleted))
    //                        .ThenInclude(scc => scc.Service)
    //                .Include(b => b.Artist)
    //                    .ThenInclude(a => a!.ArtistAvailabilities.Where(aa => !aa.IsDeleted && aa.AvailableDate == serviceDate))
    //        .Where(b => b.ServiceDate == serviceDate && b.ServiceTime >= serviceTime && b.ArtistId == artistId));
    //    }

    //    private async Task<CustomerSelected> GetCustomCombo(Guid customComboId)
    //    {
    //        return (await _unitOfWork.CustomerSelectedRepository.GetAsync( query =>
    //            query.Where(x => x.ID == customComboId && !x.IsDeleted)
    //                .Include(x => x.ServiceCustomCombos.Where(scc => !scc.Service!.IsDeleted))
    //                    .ThenInclude(x => x.Service))).FirstOrDefault() ?? throw new Exception("Custom combo not found");
    //    }

    //    private async Task<CustomDesign> GetCustomDesign(Guid customDesignId, IEnumerable<Guid> serviceIds)
    //    {
    //        return (await _unitOfWork.CustomDesignRepository.GetAsync( query =>
    //            query.Where(x => x.ID == customDesignId && !x.IsDeleted)
    //                .Include(x => x.Design)
    //                    .ThenInclude(x => x!.DesignServices.Where(ds => customDesignId == ds.DesignId && serviceIds.Contains(ds.ServiceId) && !ds.Service!.IsDeleted))
    //                .Include(x => x.CustomNailDesigns)
    //                    .ThenInclude(x => x.AccessoryCustomNailDesigns.Where(acnd => !acnd.Accessory!.IsDeleted))
    //                        .ThenInclude(x => x.Accessory)
    //                )).FirstOrDefault() ?? throw new Exception("Custom design not found");
    //    }

    //    private async Task<long> GetTotalAmount(Guid customDesignId, CustomerSelected customCombo)
    //    {
    //        var services = customCombo.ServiceCustomCombos.Select(scc => scc.Service);

    //        var result = services.Sum(c => c!.Price);

    //        var customDesign = await GetCustomDesign(customDesignId, services.Select(c => c!.ID));

    //        result += customDesign.Price;

    //        result += customDesign!.Design!.DesignServices.Sum(c => c.ExtraPrice);

    //        return result;
    //    }

    //    public async Task Create(BookingRequest request)
    //    {
    //        try
    //        {
    //            _unitOfWork.BeginTransaction();
    //            var currentBooking = _mapper.Map<Booking>(request);

    //            var aa = _unitOfWork.ArtistStoreRepository.Get(x => x.ArtistId == request.ArtistId && x.AvailableDate == request.ServiceDate && x.StartTime <= request.ServiceTime && request.ServiceTime <= x.EndTime).FirstOrDefault() ?? throw new Exception("Artist is not available at this time");

    //            var allBookingInThisDay = await GetAllBookingInThisDay(request.ServiceDate, request.ServiceTime, request.ArtistId);

    //            var allServicesInThisBooking = await GetCustomCombo(request.CustomComboId);

    //            currentBooking.PredictCompleteTime = request.ServiceTime.AddMinutes(PredictTotalDuration(allServicesInThisBooking));

    //            var endTime = currentBooking.StartTime.AddMinutes(aa.BreakTime);

    //            currentBooking.Status = (int)BookingStatus.isBooked;

    //            foreach (var otherBooking in allBookingInThisDay)
    //            {
    //                var otherEndTime = otherBooking.PredictCompleteTime.AddMinutes(aa.BreakTime);

    //                if ((otherBooking.StartTime <= request.ServiceTime && request.ServiceTime <= otherEndTime) || 
    //                    (otherBooking.StartTime <= endTime && endTime <= otherEndTime))
    //                {
    //                    currentBooking.Status = (int)BookingStatus.isWating;
    //                }
    //            }

    //            currentBooking.TotalAmount = await GetTotalAmount(request.CustomDesignId, allServicesInThisBooking);

    //            await _unitOfWork.BookingRepository.InsertAsync(currentBooking);

    //            if(await _unitOfWork.SaveAsync() <= 0)
    //            {
    //                throw new Exception("Something was wrong");
    //            }

    //            _unitOfWork.CommitTransaction();
    //        }
    //        catch (Exception)
    //        {
    //            _unitOfWork.RollBack();
    //            throw;
    //        }
    //    }

    //    public async Task CancelBooking(Guid id)
    //    {
    //        throw new NotImplementedException();
    //        //try
    //        //{
    //        //    _unitOfWork.BeginTransaction();

    //        //    var book = (await _unitOfWork.BookingRepository.GetAsync( query => query
    //        //    .Where(c => !c.IsDeleted && c.ID == id)
    //        //    .Include(c => c.Artist)
    //        //        .ThenInclude(c => c!.ArtistAvailabilities.Where(c => !c.IsDeleted))
    //        //    )).FirstOrDefault() ?? throw new Exception("Booking not found");

    //        //    book.Status = (int)BookingStatus.isCanceled;

    //        //    var breaktime = book.Artist!.ArtistAvailabilities.FirstOrDefault(c => c.AvailableDate == book.ServiceDate)?.BreakTime ?? 0;

    //        //    var otherBookings = await GetAllBookingInThisDay(book.ServiceDate, book.ServiceTime, book.ArtistId);

    //        //    var endTimeOfBook = book.ServiceTime.AddMinutes(book.Duration + breaktime);

    //        //    foreach (var otherBooking in otherBookings.OrderBy(c => c.CreatedAt))
    //        //    {
    //        //        var otherEndTime = otherBooking.ServiceTime.AddMinutes(otherBooking.Duration + breaktime);

    //        //        if (otherBooking.Status == (int)BookingStatus.isWating && 
    //        //            (book.ServiceTime >= otherBooking.ServiceTime && otherBooking.ServiceTime >= endTimeOfBook) ||
    //        //            (book.ServiceTime >= otherEndTime && otherEndTime >= endTimeOfBook))
    //        //        {
    //        //            #warning Push notification to client
    //        //            book.
    //        //        }

    //        //    }

    //        //    if (await _unitOfWork.SaveAsync() == 0)
    //        //    {
    //        //        throw new Exception("Delete booking failed");
    //        //    }
    //        //}
    //        //catch (Exception)
    //        //{

    //        //    throw;
    //        //}
    //    }

    //    public Task<IEnumerable<BookingResponse>> Get()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task Update(Guid id, BookingRequest request)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
