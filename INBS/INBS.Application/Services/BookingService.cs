using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.Booking;
using INBS.Application.Interfaces;
using INBS.Application.IService;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
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
        private async Task<ArtistStore> ValidateArtistStore(BookingRequest request, TimeOnly predictEndTime)
        {
            var artistStores = await _unitOfWork.ArtistStoreRepository.GetAsync(c => c.Where(c
               => c.StoreId == request.StoreId
               && c.ArtistId == request.ArtistId
               && c.WorkingDate == request.ServiceDate
               && c.StartTime <= request.StartTime
               && c.EndTime >= predictEndTime
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
                        .ThenInclude(c => c!.Service));

            var customerSelected = customerSelecteds.FirstOrDefault() ?? throw new Exception("Your selected nail design and service not found");

            return customerSelected;
        }

        private async Task<IEnumerable<Booking>> ValidateBooking(Booking booking, ArtistStore artistStore)
        {
            var breaktime = artistStore.BreakTime;

            var bookings = await _unitOfWork.BookingRepository.GetAsync(c => c.Where(oldBooking
                => oldBooking.ArtistStoreId == booking.ArtistStoreId
                
                && oldBooking.ServiceDate == booking.ServiceDate
                
                && ( IsStuckTime(booking.StartTime, booking.PredictEndTime.AddMinutes(breaktime), oldBooking.StartTime, oldBooking.PredictEndTime.AddMinutes(breaktime))
                
                || IsOverlapping(oldBooking.StartTime, oldBooking.PredictEndTime.AddMinutes(breaktime), booking.StartTime, booking.PredictEndTime.AddMinutes(breaktime)))

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


        private static void AssignBooking(ref Booking booking, CustomerSelected customerSelected, ArtistStore artistStore)
        {
            var totalDuration = 0L;
            var totalAmount = 0L;
            foreach (var item in customerSelected.NailDesignServiceSelecteds)
            {
                totalDuration += item.NailDesignService!.Service!.AverageDuration;
                totalAmount += item.NailDesignService!.Service!.Price + item.NailDesignService!.ExtraPrice;
            }
            booking.Status = (int)BookingStatus.isBooked;
            booking.PredictEndTime = booking.StartTime.AddMinutes(totalDuration);
            booking.TotalAmount = totalAmount;
            booking.ArtistStoreId = artistStore.ID;
        }

        public async Task Create(BookingRequest bookingRequest)
        {
            try
            {
                var booking = _mapper.Map<Booking>(bookingRequest);

                var customerSelected = await PredictDuration(bookingRequest);

                var artistStore = await ValidateArtistStore(bookingRequest, booking.PredictEndTime);
                booking.ArtistStoreId = artistStore.ID;

                // Assign booking details
                AssignBooking(ref booking, customerSelected, artistStore);

                if ((await ValidateBooking(booking, artistStore)).Any())
                {
                    booking.Status = (int)BookingStatus.isWating;
                }

                // Save booking to the repository
                await _unitOfWork.BookingRepository.InsertAsync(booking);
                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("Your action failed");
                }
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
                return _unitOfWork.BookingRepository.Query().ProjectTo<BookingResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {
                return (new List<BookingResponse>()).AsQueryable();
            }
        }

        private async Task RecheckStatusBooking(Booking bookingReq)
        {
            var pendingBookings = await _unitOfWork.BookingRepository.GetAsync(c => c.Where(c 
                => c.ServiceDate == bookingReq.ServiceDate 
                && c.ArtistStoreId == bookingReq.ArtistStoreId
                && c.ID != bookingReq.ID
                && new[] { (int)BookingStatus.isWating, (int)BookingStatus.isBooked }.Contains(c.Status)
                && c.ID != bookingReq.ID
                && !c.IsDeleted
                ).Include(x => x.ArtistStore));

            if (!pendingBookings.Any()) return;

            var waitbookings = pendingBookings.OrderBy(c => c.LastModifiedAt).Where(c => c.Status == (int)BookingStatus.isWating);
            var bookedlist = pendingBookings.OrderBy(c => c.LastModifiedAt).Where(c => c.Status == (int)BookingStatus.isBooked);
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
                waitbooking.Status = (int)BookingStatus.isBooked;

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

                var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id) ?? throw new Exception($"The entity with {id} is not existed.");

                _mapper.Map(bookingRequest, booking);

                var customerSelected = await PredictDuration(bookingRequest);

                var artistStore = await ValidateArtistStore(bookingRequest, booking.PredictEndTime);

                // Assign booking details
                AssignBooking(ref booking, customerSelected, artistStore);

                if ((await ValidateBooking(booking, artistStore)).Any())
                {
                    booking.Status = (int)BookingStatus.isWating;
                }

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

        public async Task CompleteBooking(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id) ?? throw new Exception($"The entity with {id} is not existed.");

                booking.Status = (int)BookingStatus.isCompleted;

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
                .ThenInclude(b => b.Customer)
                .ThenInclude(b => b.User));

            return bookings.ToList();
        }
    }
}
