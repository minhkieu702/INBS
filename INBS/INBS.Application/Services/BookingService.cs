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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using NetHttpClient = System.Net.Http.HttpClient;


namespace INBS.Application.Services
{
    public class BookingService(IUnitOfWork _unitOfWork, IMapper _mapper, IFirebaseCloudMessageService _fcmService, IExpoNotification _expoNotification) : IBookingService
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

            var bookings = await _unitOfWork.BookingRepository.GetAsync(c => c.Where(otherBooking
                => otherBooking.ArtistStoreId == booking.ArtistStoreId

                && otherBooking.ServiceDate == booking.ServiceDate


                && (booking.StartTime < otherBooking.PredictEndTime.AddMinutes(breaktime)
                && otherBooking.StartTime < booking.PredictEndTime.AddMinutes(breaktime))
                && !new[] { (int)BookingStatus.isCanceled, (int)BookingStatus.isCompleted }.Contains(otherBooking.Status)

                && otherBooking.ID != booking.ID
                && !otherBooking.IsDeleted
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
            //var totalDuration = 0L;
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
                //totalDuration += item.NailDesignService!.Service!.AverageDuration;
                totalAmount += servicePrice.Price + item.NailDesignService!.ExtraPrice;
            }
            oldBooking.Status = (int)BookingStatus.isConfirmed;
            //oldBooking.PredictEndTime = oldBooking.StartTime.AddMinutes(totalDuration);
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

        //

        public async Task<Guid> Create(BookingRequest bookingRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();          
                var booking = _mapper.Map<Booking>(bookingRequest);

                booking.StartTime.AddMinutes(bookingRequest.EstimateDuration);
                // Assign booking details
                booking = await AssignBooking(booking, bookingRequest);             

                // Save booking to the repository
                await _unitOfWork.BookingRepository.InsertAsync(booking);

                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("Your action failed");
                }
                _unitOfWork.CommitTransaction();
                return booking.ID;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
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
                && !c.IsDeleted
                ).Include(x => x.ArtistStore)
                .Include(x => x.CustomerSelected));

            if (!pendingBookings.Any()) return;

            // Sắp xếp lại các booking chờ và đã xác nhận theo thời gian bắt đầu
            var waitbookings = pendingBookings.Where(c => c.Status == (int)BookingStatus.isWaiting)
                                              .OrderBy(c => c.StartTime)
                                              .ToList();

            var confirmBookings = pendingBookings.Where(c => c.Status == (int)BookingStatus.isConfirmed)
                                            .OrderBy(c => c.StartTime)
                                            .ToList();

            var artistStore = pendingBookings.First().ArtistStore ?? throw new Exception("Artist is not at this store");
            var updateList = new List<Booking>();

            foreach (var waitbooking in waitbookings)
            {
                // Tìm các booking đã xác nhận có thời gian giao nhau
                var conflictingBooking = confirmBookings.FirstOrDefault(
                    confirmBooking => waitbooking.StartTime < confirmBooking.PredictEndTime.AddMinutes(artistStore.BreakTime)
                                && confirmBooking.StartTime < waitbooking.PredictEndTime.AddMinutes(artistStore.BreakTime));

                // Nếu không có booking nào bị xung đột, chuyển trạng thái booking chờ thành đã xác nhận
                if (conflictingBooking == null)
                {
                    waitbooking.Status = (int)BookingStatus.isConfirmed;
                    updateList.Add(waitbooking);

                    // Thêm vào danh sách đã xác nhận để tránh xung đột với các booking chờ tiếp theo
                    confirmBookings.Add(waitbooking);
                    confirmBookings = confirmBookings.OrderBy(c => c.StartTime).ToList();
                }
            }

            // Cập nhật danh sách booking
            _unitOfWork.BookingRepository.UpdateRange(updateList);
        }

        private async Task HandleNotification(Guid userId)
        {
            var deviceTokens = await _unitOfWork.DeviceTokenRepository.GetAsync(query => query.Where(c => c.Platform == (int)DevicePlatformType.App && c.UserId == userId));
            if (deviceTokens.Count() == 0)
            {
#warning Set Thrown
                return;
            }

            var tasks = new List<Task>();
            var content = "Your booking is approved";
            
            var notifications = new List<Notification>();

            foreach (var deviceToken in deviceTokens)
            {
                
            }
        }

        private async Task SendNotificationBookingToArtist(Guid artistId, string title, string body)
        {
            var deviceTokenOfArtist = await _unitOfWork.DeviceTokenRepository.GetAsync(query => query.Where(c => c.UserId == artistId));

#warning ADD THROWN EXCEPTION
            if (!deviceTokenOfArtist.Any()) return /*throw new Exception("Can't send notification to artist, because artist does not have device")*/;

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

        private async Task NotificationToCustomer(Guid userId, string content)
        {
            var devieTokens = await _unitOfWork.DeviceTokenRepository.GetAsync(query => query.Where(c => c.UserId == userId));
            
            var notification = new Notification
            {
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                NotificationType = (int)NotificationType.Reminder,
                Status = (int)NotificationStatus.Send,
            };
            var check = true;
            foreach (var deviceToken in devieTokens)
            {
                if (!(await _expoNotification.SendPushNotificationAsync(deviceToken.Token,"YOUR BOOKING IS SERVING", content, "BookingDetail")))
            {
                    check = false; break;
                }
            }

            if (check)
            {
                await _unitOfWork.NotificationRepository.InsertAsync(notification);
            }
        }

        public async Task SetBookingIsServicing(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var booking = (await _unitOfWork.BookingRepository.GetAsync(
                    query => query
                    .Where(c => c.ID == id)
                    .Include(c => c.CustomerSelected)
                    )).FirstOrDefault() ?? throw new Exception($"The entity with {id} is not existed.");

                booking.Status = (int)BookingStatus.isServing;

                var customerSelected = booking.CustomerSelected;

                if (customerSelected != null) await NotificationToCustomer(customerSelected.CustomerID, "Your booking is serving");

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