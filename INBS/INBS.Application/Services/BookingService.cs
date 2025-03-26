using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.Booking;
using INBS.Application.Interfaces;
using INBS.Application.IService;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace INBS.Application.Services
{
    public class BookingService(IUnitOfWork _unitOfWork, IMapper _mapper, IFirebaseCloudMessageService _fcmService) : IBookingService
    {
        private async Task<ArtistStore> ValidateArtistStore(BookingRequest request, TimeOnly predictEndTime)
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

            await SendNotificationBookingToArtist(bookingRequest.ArtistId, "YOU ARE CHOSEN ONE", oldBooking.Status == (int)BookingStatus.isWaiting ? $"You got a overlapping booking that start at {bookingRequest.StartTime} on {bookingRequest.ServiceDate}" : $"You have new booking that start at {bookingRequest.StartTime} on {bookingRequest.ServiceDate}");

            oldBooking.ArtistStoreId = artistStore.ID;

            return oldBooking;
        }

        public async Task Create(BookingRequest bookingRequest)
        {
            try
            {
                logger.LogInformation(" BookingRequest: {@BookingRequest}", bookingRequest);

                var booking = _mapper.Map<Booking>(bookingRequest);

                int estimatedMinutes = await PredictCompletionTime(bookingRequest);

                // Tính toán `PredictEndTime`
                TimeOnly endTime = bookingRequest.StartTime.Add(TimeSpan.FromMinutes(estimatedMinutes));
                booking.PredictEndTime = endTime;

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
        public async Task<int> PredictCompletionTime(BookingRequest bookingRequest)
        {
            using var httpClient = new NetHttpClient();

            var fastApiUrl = "http://100.115.78.81:8001/api/booking"; // URL FastAPI

            var jsonContent = JsonConvert.SerializeObject(new
            {
                ServiceDate = bookingRequest.ServiceDate.ToString("yyyy-MM-dd"),
                StartTime = bookingRequest.StartTime.ToString(@"HH\:mm"),
                CustomerSelectedId = bookingRequest.CustomerSelectedId.ToString(),
                ArtistId = bookingRequest.ArtistId.ToString(),
                StoreId = bookingRequest.StoreId.ToString()
            });
            Console.WriteLine($"📤 JSON Sent to FastAPI: {jsonContent}");

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(fastApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ FastAPI Error: {response.StatusCode}, Response: {errorResponse}");
                throw new Exception($"FastAPI Error: {response.StatusCode} - {errorResponse}");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"✅ FastAPI Response Data: {responseData}");

            var result = JsonConvert.DeserializeObject<BookingResponse>(responseData);

            return result.EstimatedCompletionMinutes;
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

                updateList.Add(waitbooking);
            }

            _unitOfWork.BookingRepository.UpdateRange(updateList);
        }

        private async Task SendNotificationBookingToArtist(Guid artistId, string title, string body)
        {
            var deviceTokenOfArtist = await _unitOfWork.DeviceTokenRepository.GetAsync(query => query.Where(c => c.UserId == artistId));

            if (!deviceTokenOfArtist.Any()) throw new Exception("Can't send notification to artist, because artist does not have device");

            var notification = new Notification
            {
                CreatedAt = DateTime.Now,
                Status = (int)NotificationStatus.Send,
                NotificationType = (int)NotificationType.Notification,
                UserId = artistId,
            };

            await _unitOfWork.NotificationRepository.InsertAsync(notification);

            await _fcmService.SendToMultipleDevices(deviceTokenOfArtist.Select(c => c.Token).ToList(), title, body);
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

                var booking = (await _unitOfWork.BookingRepository.GetAsync(query 
                    => query.Where(c => c.ID == id)
                    .Include(c => c.ArtistStore))
                    ).FirstOrDefault() ?? throw new Exception($"The entity with {id} is not existed.");

                booking.Status = (int)BookingStatus.isCanceled;

                // Save booking to the repository
                await _unitOfWork.BookingRepository.UpdateAsync(booking);

                await RecheckStatusBooking(booking);

                await SendNotificationBookingToArtist(booking.ArtistStore!.ArtistId, "YOUR APPOINTMENT IS CANCELED", $"A booking at {booking.StartTime} on {booking.ServiceDate} is canceled");

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