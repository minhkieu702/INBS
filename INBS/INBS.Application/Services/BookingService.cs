using AutoMapper;
using AutoMapper.QueryableExtensions;
using Google.Apis.Http;
using INBS.Application.Common.MyJsonConverters;
using INBS.Application.DTOs.Booking;
using INBS.Application.Interfaces;
using INBS.Application.IService;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class BookingService(IUnitOfWork _unitOfWork, IMapper _mapper, ILogger<BookingService> logger, HttpClient httpClient) : IBookingService
    {
        private async Task<ArtistStore> ValidateArtistStore(BookingRequest request, TimeOnly? predictEndTime)
        {
            var artistStores = await _unitOfWork.ArtistStoreRepository.GetAsync(c => c.Where(c
               => c.StoreId == request.StoreId
               && c.ArtistId == request.ArtistId
               && c.WorkingDate == request.ServiceDate
               && c.StartTime <= request.StartTime
               && !c.IsDeleted
               ));

            return artistStores.FirstOrDefault() ?? throw new Exception("Artist is not at this store");
        }

        private async Task<CustomerSelected> PredictDuration(BookingRequest request)
        {
            var customerSelecteds = await _unitOfWork.CustomerSelectedRepository.GetAsync(c
                => c.Where(c => c.ID == request.CustomerSelectedId)
                .Include(c => c.NailDesignServiceSelecteds
                    .Where(c => !c.NailDesignService!.Service!.IsDeleted && !c.NailDesignService!.IsDeleted))
                    .ThenInclude(c => c.NailDesignService)
                        .ThenInclude(c => c!.Service)
                            .ThenInclude(c => c!.ServicePriceHistories));

            var customerSelected = customerSelecteds.FirstOrDefault() ?? throw new Exception("Your selected nail design and service not found");

            return customerSelected;
        }

        private async Task<IEnumerable<Booking>> ValidateBooking(Booking booking, ArtistStore artistStore)
        {
            var breaktime = artistStore.BreakTime;

            var bookings = await _unitOfWork.BookingRepository.GetAsync(c => c.Where(oldBooking
                => oldBooking.ArtistStoreId == booking.ArtistStoreId

                && oldBooking.ServiceDate == booking.ServiceDate


                && (booking.StartTime < oldBooking.PredictEndTime.AddMinutes(breaktime)
                && oldBooking.StartTime < booking.PredictEndTime.AddMinutes(breaktime))
                && !new[] { (int)BookingStatus.isCanceled, (int)BookingStatus.isCompleted }.Contains(oldBooking.Status)

                && oldBooking.ID != booking.ID
                && !oldBooking.IsDeleted
                ));

            if (bookings.Any()) return bookings;
            return [];
        }

        private static bool IsStuckTime(TimeOnly outerStartTime, TimeOnly outerEndTime, TimeOnly innerStartTime, TimeOnly innerEndTime)
        {
            // Booking start time is between old booking start time and end time
            // Booking end time is between old booking start time and end time
            return
                outerStartTime <= innerStartTime && innerStartTime <= outerEndTime
                || outerStartTime <= innerEndTime && innerEndTime <= outerEndTime;
        }

        private static bool IsOverlapping(TimeOnly outerStartTime, TimeOnly outerEndTime, TimeOnly innerStartTime, TimeOnly innerEndTime)
        {
            return outerStartTime <= innerEndTime && innerStartTime <= outerEndTime;
        }


        private async Task<Booking> AssignBooking(Booking oldBooking, BookingRequest bookingRequest)
        {
            var totalDuration = 0L;
            var totalAmount = 0L;

            var customerSelectedId = oldBooking.CustomerSelectedId;

            //var nailDesignServiceSelecteds = await _unitOfWork.NailDesignServiceSelectedRepository.GetAsync(c
            //    => c.Where(c => c.CustomerSelectedId == bookingRequest.CustomerSelectedId));

            //var nailDesignService = await _unitOfWork.NailDesignServiceRepository.GetAsync(c
            //    => c.Include(c => c.Service)
            //        .ThenInclude(c => c.ServicePriceHistories)
            //        .Where(c => nailDesignServiceSelecteds.Select(c => c.NailDesignServiceId).Contains(c.
            //    );

            //var temp = _unitOfWork.NailDesignServiceRepository.Query().Include(c => c.Service).ThenInclude(c => c!.ServicePriceHistories).Where(c => !c.IsDeleted && c.Service != null && !c.Service.IsDeleted);

            var nailDesignServiceSelecteds = await _unitOfWork.NailDesignServiceSelectedRepository.GetAsync(c => c.Where(c =>
                    c.CustomerSelectedId == bookingRequest.CustomerSelectedId)
                .Include(c => c.NailDesignService)
                .ThenInclude(c => c!.Service)
                .ThenInclude(c => c!.ServicePriceHistories));

            if (!nailDesignServiceSelecteds.Any())
            {
                throw new Exception("Your selected nail design and service not found");
            }

            foreach (var item in nailDesignServiceSelecteds)
            {
                var servicePrice = item.NailDesignService!.Service!.ServicePriceHistories.FirstOrDefault(c => c.EffectiveTo == null) ?? throw new Exception("Some service do not have price");
                totalDuration += item.NailDesignService!.Service!.AverageDuration;
                totalAmount += servicePrice.Price + item.NailDesignService!.ExtraPrice;
            }
            oldBooking.Status = (int)BookingStatus.isConfirmed;
            oldBooking.PredictEndTime = oldBooking.StartTime.AddMinutes(totalDuration);
            oldBooking.TotalAmount = totalAmount;

            var artistStore = await ValidateArtistStore(bookingRequest, oldBooking.PredictEndTime);

            if ((await ValidateBooking(oldBooking, artistStore)).Any())
            {
                oldBooking.Status = (int)BookingStatus.isWaiting;
            }

            oldBooking.ArtistStoreId = artistStore.ID;

            return oldBooking;
        }

        public async Task Create(BookingRequest bookingRequest)
        {
            try
            {
                logger.LogInformation(" BookingRequest: {@BookingRequest}", bookingRequest);

                var booking = _mapper.Map<Booking>(bookingRequest);
                // Assign booking details
                booking = await AssignBooking(booking, bookingRequest);

                logger.LogInformation(" BookingRequest: {@BookingRequest}", bookingRequest);

                // Save booking to the repository
                await _unitOfWork.BookingRepository.InsertAsync(booking);
                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("Your action failed");
                }
                logger.LogInformation("Booking created successfully: {@Booking}", booking);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<BookingResponse> Get()
        {
            try
            {
                return _unitOfWork.BookingRepository.Query().IgnoreQueryFilters().ProjectTo<BookingResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task RecheckStatusBooking(Booking bookingReq)
        {
            var pendingBookings = await _unitOfWork.BookingRepository.GetAsync(c => c.Where(c
                => c.ServiceDate == bookingReq.ServiceDate
                && c.ArtistStoreId == bookingReq.ArtistStoreId
                && c.ID != bookingReq.ID
                && new[] { (int)BookingStatus.isWaiting, (int)BookingStatus.isConfirmed }.Contains(c.Status)
                && c.ID != bookingReq.ID
                && !c.IsDeleted
                ).Include(x => x.ArtistStore));

            if (!pendingBookings.Any()) return;

            var waitbookings = pendingBookings.OrderBy(c => c.LastModifiedAt).Where(c => c.Status == (int)BookingStatus.isWaiting);
            var bookedlist = pendingBookings.OrderBy(c => c.LastModifiedAt).Where(c => c.Status == (int)BookingStatus.isConfirmed);
            var artistStore = pendingBookings.First().ArtistStore ?? throw new Exception("Artist is not at this store");

            var updateList = new List<Booking>();

            foreach (var waitbooking in waitbookings)
            {
                var changeBooking = bookedlist.FirstOrDefault(booking
                    => (
                    // "book with isBooked" start time is between wait booking start time and end time
                    // "book with isBooked" end time is between wait booking start time and end time
                    IsStuckTime(waitbooking.StartTime, waitbooking.PredictEndTime.AddMinutes(artistStore.BreakTime), booking.StartTime, booking.PredictEndTime.AddMinutes(artistStore.BreakTime))
                    ||
                    IsOverlapping(booking.StartTime, booking.PredictEndTime.AddMinutes(artistStore.BreakTime), waitbooking.StartTime, waitbooking.PredictEndTime.AddMinutes(artistStore.BreakTime))
                    ));
                // If there is a booking in wait booking time, next to wait booking
                if (changeBooking == null) continue;

                // Change status of wait booking to isBooked
                waitbooking.Status = (int)BookingStatus.isConfirmed;

#warning do notification with fcm or signalr here

                updateList.Add(waitbooking);
            }

            _unitOfWork.BookingRepository.UpdateRange(updateList);
        }

        public async Task Update(Guid id, BookingRequest bookingRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id) ?? throw new Exception($"The booking with {id} is not existed.");

                _mapper.Map(bookingRequest, booking);

                // Assign booking details
                booking = await AssignBooking(booking, bookingRequest);

                await _unitOfWork.BookingRepository.UpdateAsync(booking);

                await RecheckStatusBooking(booking);

                // Save booking to the repository
                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("Your action failed");
                }

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }


        public async Task CancelBooking(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id) ?? throw new Exception($"The entity with {id} is not existed.");

                booking.Status = (int)BookingStatus.isCanceled;

                // Save booking to the repository
                await _unitOfWork.BookingRepository.UpdateAsync(booking);

                await RecheckStatusBooking(booking);

                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("Your action failed");
                }

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task SetBookingIsServicing(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id) ?? throw new Exception($"The entity with {id} is not existed.");

                booking.Status = (int)BookingStatus.isServing;

                // Save booking to the repository
                await _unitOfWork.BookingRepository.UpdateAsync(booking);

                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("Your action failed");
                }

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<List<Booking>> GetBookingsWithinNextMinutes(int minutes)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var now = TimeOnly.FromDateTime(DateTime.Now);
            var targetTime = now.AddMinutes(minutes);

            var bookings = await _unitOfWork.BookingRepository.GetAsync(query => query
                .Where(b => b.ServiceDate == today && b.StartTime >= now && b.StartTime <= targetTime)
                .Include(b => b.CustomerSelected)
                .ThenInclude(b => b!.Customer)
                .ThenInclude(b => b!.User));

            return bookings.ToList();
        }
    } 
}