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
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;


namespace INBS.Application.Services
{
    public class BookingService(IUnitOfWork _unitOfWork, IMapper _mapper, IFirebaseCloudMessageService _fcmService, IExpoNotification _expoNotification, HttpClient _httpClient, IAuthentication _authentication, IHttpContextAccessor _contextAccessor, INotificationHubService _notificationHubService) : IBookingService
    {

        private const string ApiKey = "469acea901a9fff8210792874151eaa2582149dbf8fa1a28db48ebb4c5901382";
        private const string TogetherAIUrl = "https://api.together.xyz/v1/chat/completions";


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
                totalAmount += servicePrice.Price;
            }
            oldBooking.Status = (int)BookingStatus.isConfirmed;
            oldBooking.TotalAmount = totalAmount;

            var artistStore = await ValidateArtistStore(bookingRequest, oldBooking.PredictEndTime);

            if ((await ValidateBooking(oldBooking, artistStore)).Any())
            {
                oldBooking.Status = (int)BookingStatus.isWaiting;
            }

            await SendNotificationBookingToArtist(bookingRequest.ArtistId, "YOU ARE CHOSEN ONE", oldBooking.Status == (int)BookingStatus.isWaiting ? $"You got a overlapping booking that start at {bookingRequest.StartTime} on {bookingRequest.ServiceDate}" : $"You have new booking that start at {bookingRequest.StartTime} on {bookingRequest.ServiceDate}", $"/booking/:{oldBooking.ID}");

            oldBooking.ArtistStoreId = artistStore.ID;

            return oldBooking;
        }

        //

        public async Task<Guid> Create(BookingRequest bookingRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Validate booking request
                if (bookingRequest == null)
                    throw new Exception("Booking request cannot be null");

                // Validate booking time
                if (bookingRequest.ServiceDate < DateOnly.FromDateTime(DateTime.Now) ||
                    (bookingRequest.ServiceDate == DateOnly.FromDateTime(DateTime.Now) && 
                     bookingRequest.StartTime < TimeOnly.FromDateTime(DateTime.Now)))
                {
                    throw new Exception("Cannot create booking in the past");
                }

                var booking = _mapper.Map<Booking>(bookingRequest);
                booking.PredictEndTime = booking.StartTime.AddMinutes(bookingRequest.EstimateDuration);
                
                // Assign booking details
                booking = await AssignBooking(booking, bookingRequest);

                // Save booking to the repository
                await _unitOfWork.BookingRepository.InsertAsync(booking);

                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("Failed to create booking");
                }

                // Send notification to artist
                await _notificationHubService.NotifyBookingCreated(
                    bookingRequest.ArtistId,
                    "New Booking",
                    $"You have a new booking at {booking.StartTime} on {booking.ServiceDate}",
                    new { booking.ID, booking.StartTime, booking.ServiceDate }
                    );

                // Send notification to customer
                if (booking.CustomerSelected?.CustomerID != null)
                {
                    await NotificationToCustomer(
                        booking.CustomerSelected.CustomerID,
                        "Booking Confirmation",
                        $"Your booking has been created successfully for {booking.StartTime} on {booking.ServiceDate}"
                    );
                }

                _unitOfWork.CommitTransaction();
                return booking.ID;
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                throw new Exception($"Error creating booking: {ex.Message}");
            }
        }

        public IQueryable<BookingResponse> Get()
        {
            try
            {
                var role = _authentication.GetUserRoleFromHttpContext(_contextAccessor.HttpContext);
                if (role == 2)
                {
                    return _unitOfWork.BookingRepository.Query().ProjectTo<BookingResponse>(_mapper.ConfigurationProvider);
                }
                return _unitOfWork.BookingRepository.Query().Where(c => !c.IsDeleted).ProjectTo<BookingResponse>(_mapper.ConfigurationProvider);
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

        private async Task SendNotificationBookingToArtist(Guid artistId, string title, string body, string webHref)
        {
            var deviceTokenOfArtist = await _unitOfWork.DeviceTokenRepository.GetAsync(query => query.Where(c => c.UserId == artistId && c.Platform == (int)DevicePlatformType.Web));

            var notification = new Notification
            {
                WebHref = webHref,
                CreatedAt = DateTime.Now,
                NotificationType = (int)NotificationType.Notification,
                Content = body,
                Title = title,
                UserId = artistId,
            };

            await _unitOfWork.NotificationRepository.InsertAsync(notification);

            if (!deviceTokenOfArtist.Any()) return /*throw new Exception("Can't send notification to artist, because artist does not have device")*/;

            await _fcmService.SendToMultipleDevices(deviceTokenOfArtist.Select(c => c.Token).ToList(), title, body);
        }

        public async Task Update(Guid id, BookingRequest bookingRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Get and validate booking with artist store information
                var booking = await _unitOfWork.BookingRepository.Query()
                    .Include(c => c.ArtistStore)
                    .FirstOrDefaultAsync(c => c.ID == id) ?? throw new Exception($"The booking with {id} is not existed.");

                // Validate booking status
                if (booking.Status == (int)BookingStatus.isCompleted || 
                    booking.Status == (int)BookingStatus.isCanceled)
                {
                    throw new Exception("Cannot update completed or canceled booking");
                }

                // Validate new booking time
                if (bookingRequest.ServiceDate < DateOnly.FromDateTime(DateTime.Now) ||
                    (bookingRequest.ServiceDate == DateOnly.FromDateTime(DateTime.Now) && 
                     bookingRequest.StartTime < TimeOnly.FromDateTime(DateTime.Now)))
                {
                    throw new Exception("Cannot update booking to past time");
                }

                // Store old data for notification
                var oldStartTime = booking.StartTime;
                var oldServiceDate = booking.ServiceDate;
                var oldArtistId = booking.ArtistStore?.ArtistId;

                // Update booking
                _mapper.Map(bookingRequest, booking);
                booking.PredictEndTime = booking.StartTime.AddMinutes(bookingRequest.EstimateDuration);
                booking = await AssignBooking(booking, bookingRequest);

                // If artist has changed, notify both old and new artists
                if (oldArtistId.HasValue && oldArtistId != bookingRequest.ArtistId)
                {
                    // Notify old artist about cancellation
                    await _notificationHubService.NotifyBookingUpdated(
                        oldArtistId.Value,
                        "Booking Reassigned",
                        $"A booking at {oldStartTime} on {oldServiceDate} has been reassigned",
                        new { booking.ID, oldStartTime, oldServiceDate }
                    );

                    // Notify new artist about new booking
                    await _notificationHubService.NotifyBookingUpdated(
                        bookingRequest.ArtistId,
                        "Booking Updated",
                        $"You have been assigned a new booking at {booking.StartTime} on {booking.ServiceDate}",
                        new { booking.ID, booking.StartTime, booking.ServiceDate }
                    );
                }
                // If only time changed but same artist, notify the artist about the change
                else if (oldStartTime != booking.StartTime || oldServiceDate != booking.ServiceDate)
                {
                    await SendNotificationBookingToArtist(
                        booking.ArtistStore!.ArtistId,
                        "BOOKING TIME CHANGED",
                        $"Your booking has been rescheduled from {oldStartTime} {oldServiceDate} to {booking.StartTime} {booking.ServiceDate}",
                        $"/booking/:{booking.ID}"
                    );
                }

                await _unitOfWork.BookingRepository.UpdateAsync(booking);
                await RecheckStatusBooking(booking);

                // Save changes
                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("Failed to update booking");
                }

                // Send notification to customer about booking changes
                if (booking.CustomerSelected?.CustomerID != null)
                {
                    var notificationMessage = oldArtistId != bookingRequest.ArtistId
                        ? $"Your booking has been updated with a new artist and scheduled for {booking.StartTime} {booking.ServiceDate}"
                        : $"Your booking has been rescheduled from {oldStartTime} {oldServiceDate} to {booking.StartTime} {booking.ServiceDate}";

                    await NotificationToCustomer(
                        booking.CustomerSelected.CustomerID,
                        "You got new information",
                        notificationMessage
                    );
                }

                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                // Log error
                throw new Exception($"Error updating booking {id}: {ex.Message}");
            }
        }

        private async Task HandleCancel(Guid bookingId, string cancelReason)
        {
            var cancelation = new Cancellation
            {
                BookingId = bookingId,
                CancelledAt = DateTime.UtcNow,
                Reason = cancelReason,
                Percent = 1
            };
            await _unitOfWork.CancellationRepository.InsertAsync(cancelation);
        }

        public async Task CancelBooking(Guid id, string cancelReason)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var booking = await _unitOfWork.BookingRepository.Query()
                    .Include(c => c.ArtistStore)
                    .Include(c => c.CustomerSelected)
                    .FirstOrDefaultAsync(c => c.ID == id) ?? throw new Exception($"Booking with ID {id} not found");

                // Validate booking status
                if (booking.Status == (int)BookingStatus.isCompleted)
                {
                    throw new Exception("Cannot cancel completed booking");
                }

                if (booking.Status == (int)BookingStatus.isCanceled)
                {
                    throw new Exception("Booking is already canceled");
                }

                // Update booking status
                booking.Status = (int)BookingStatus.isCanceled;

                await HandleCancel(booking.ID, cancelReason);

                // Save booking to the repository
                await _unitOfWork.BookingRepository.UpdateAsync(booking);

                // Recheck other bookings' status
                await RecheckStatusBooking(booking);

                // Notify artist
                await _notificationHubService.NotifyBookingCanceled(
                    booking.ArtistStore!.ArtistId,
                    "Booking Canceled",
                    $"Booking at {booking.StartTime} on {booking.ServiceDate} has been canceled" +
                    (cancelReason != null ? $". Reason: {cancelReason}" : ""),
                    booking.ID
                );

                // Notify customer
                if (booking.CustomerSelected?.CustomerID != null)
                {
                    await NotificationToCustomer(
                        booking.CustomerSelected.CustomerID,
                        "Your booking has been canceled",
                        $"Your booking at {booking.StartTime} on {booking.ServiceDate} has been canceled" +
                        (cancelReason != null ? $". Reason: {cancelReason}" : "")
                    );
                }

                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("Failed to cancel booking");
                }

                _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                throw new Exception($"Error canceling booking: {ex.Message}");
            }
        }

        private async Task NotificationToCustomer(Guid userId, string title, string content)
        {
            var devieTokens = await _unitOfWork.DeviceTokenRepository.GetAsync(query => query.Where(c => c.UserId == userId && c.Platform == (int)DevicePlatformType.App));

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
                if (!(await _expoNotification.SendPushNotificationAsync(deviceToken.Token, title, content, "BookingDetail")))
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

                if (customerSelected != null) await NotificationToCustomer(customerSelected.CustomerID, "YOUR BOOKING IS SERVING", "Your booking is serving");

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

        public async Task<int> PredictBookingCancel(Guid BookingId)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                var booking = _unitOfWork.BookingRepository.Query()
                    .Where(b => b.ID == BookingId).FirstOrDefault();
                if (booking == null)
                {
                    throw new Exception("Booking not found");
                }

                var today = DateOnly.FromDateTime(DateTime.Now);
                int daysBeforeService = (booking.ServiceDate.DayNumber - today.DayNumber);

                _unitOfWork.CommitTransaction();

                return await PredictCancellationProbability(
                    daysBeforeService,
                    booking.Status,
                    booking.TotalAmount
                );
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<int> PredictCancellationProbability(int daysBeforeService, int status, long totalAmount)
        {
            var requestBody = new
            {
                model = "meta-llama/Llama-Vision-Free",
                messages = new[]
                {
                new
                {
                    role = "system",
                    content = "Bạn là một hệ thống dự đoán khả năng hủy Booking dựa trên các thông tin khách hàng cung cấp."
                },
                new
                {
                    role = "user",
                    content = $"Dự đoán phần trăm hủy của Booking với các thông tin sau: " +
                      $"Số ngày trước khi sử dụng dịch vụ: {daysBeforeService}, " +
                      $"Trạng thái Booking: {status}, " +
                      $"Tổng tiền Booking: {totalAmount}, " +
                      $"Trả về chỉ một số từ 0 đến 100, không giải thích."
                }
                },
                temperature = 0.7
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
            }

            try
            {
                var response = await _httpClient.PostAsync(TogetherAIUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Response Body: " + responseBody);

                var responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);
                if (responseData?.choices?.Count > 0)
                {
                    string aiResponse = responseData.choices[0].message.content;
                    return int.TryParse(aiResponse, out int probability) ? probability : -1;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API error: {ex.Message}");
            }

            return -1;
        }

        public async Task<string?> SuggestOffPeakTimeAsync(Guid bookingId)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                var booking = _unitOfWork.BookingRepository.Query()
                    .Include(b => b.ArtistStore)
                    .ThenInclude(a => a.Store)
                    .FirstOrDefault(b => b.ID == bookingId);

                if (booking == null || booking.ArtistStore == null || booking.ArtistStore.Store == null)
                {
                    throw new Exception("Thông tin booking không hợp lệ.");
                }

                var dto = new SuggestBooking
                {
                    StoreName = booking.ArtistStore.Store.Address,
                    WorkingDate = booking.ArtistStore.WorkingDate,
                    StartTime = booking.ArtistStore.StartTime,
                    EndTime = booking.ArtistStore.EndTime,
                    BreakTime = booking.ArtistStore.BreakTime,
                    TotalAmount = booking.TotalAmount
                };

                _unitOfWork.CommitTransaction();

                return await SuggestOffPeakTimeFromAI(dto);
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private async Task<float> PercentOfCanceled()
        {
            var customerId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var oldBooking = await _unitOfWork.BookingRepository.Query()
                .Where(c => c.CustomerSelected!.CustomerID == customerId && (c.Status == (int)BookingStatus.isCanceled || c.Status == (int)BookingStatus.isCompleted))
                .ToListAsync();
            if (oldBooking.Count == 0)
            {
                return 0;
            }
            var percent = oldBooking.Count(c => c.Status == (int)BookingStatus.isCanceled) / (float)oldBooking.Count;
            return percent;
        }
        private bool IsInPeakTimes((TimeOnly, TimeOnly) slot, List<(TimeOnly, TimeOnly)> peakTimes)
        {
            foreach (var peak in peakTimes)
            {
                if (slot.Item1 < peak.Item2 && slot.Item2 > peak.Item1)
                    return true;
            }
            return false;
        }

        private bool IsOverlappingWithExistingBooking((TimeOnly, TimeOnly) slot, List<Booking> bookings)
        {
            foreach (var booking in bookings)
            {
                if (slot.Item1 < booking.PredictEndTime && slot.Item2 > booking.StartTime)
                    return true;
            }
            return false;
        }

        private List<(TimeOnly, TimeOnly)> GenerateSlots(TimeOnly start, TimeOnly end, TimeSpan duration)
        {
            var result = new List<(TimeOnly, TimeOnly)>();
            var current = start;

            while (current.Add(duration) <= end)
            {
                result.Add((current, current.Add(duration)));
                current = current.Add(duration);
            }

            return result;
        }
    
        public async Task<List<SuggestSlot>> SuggestTimeSlots(DateOnly date, Guid storeId)
        {
            var peakTimeRanges = ExtractPeakHours(await CallTogetherAIToGetPeakTime(date));

            var artistStores = await _unitOfWork.ArtistStoreRepository.Query()
                .Where(c => c.StoreId == storeId && c.WorkingDate == date)
                .Include(c => c.Bookings.Where(b => !b.IsDeleted && (b.Status == (int)BookingStatus.isWaiting || b.Status == (int)BookingStatus.isConfirmed))) // Waiting or Confirmed
                .ToListAsync();

            var slotDuration = TimeSpan.FromMinutes(60); // hoặc dùng dynamic từ service duration
            var allSlots = new List<(TimeOnly start, TimeOnly end)>();

            // Tạo tất cả các slot có thể trong ngày dựa trên giờ làm của artist
            foreach (var artistStore in artistStores)
            {
                var slots = GenerateSlots(artistStore.StartTime, artistStore.EndTime, slotDuration);

                foreach (var slot in slots)
                {
                    if (IsInPeakTimes(slot, peakTimeRanges)) continue;
                    if (IsOverlappingWithExistingBooking(slot, artistStore.Bookings.ToList())) continue;

                    allSlots.Add(slot);
                }
            }

            // Chỉ giữ lại những slot mà có ít nhất 1 artist rảnh
            var groupedSlots = allSlots
                .GroupBy(s => $"{s.start}-{s.end}")
                .Where(g => g.Count() >= 1) // Có ít nhất 1 artist rảnh
                .Select(g => new SuggestSlot
                {
                    Start = g.First().start.ToString("HH:mm"),
                    End = g.First().end.ToString("HH:mm")
                })
                .Distinct()
                .ToList();

            return groupedSlots;
        }

        public async Task<string> CallTogetherAIToGetPeakTime(DateOnly date)
        {
            var prompt = $"Identify peak time on {date:dd/MM/yyyy}, based on booking history of nail salon store in Vietnam.\n " +
             $"Peak hours are time ranges with high booking frequency, long waiting time, or high cancellation rate.\n " +
             $"Your output must in this format:\n " +
             $"[ \"\"hh:mm - hh:mm\"\", \"\"hh:mm - hh:mm\"\",... ]\n" + 
             $"No explanation or code block, just the array.";
            var requestBody = new
            {
                model = "meta-llama/Llama-Vision-Free",
                messages = new[]
                {
            new
            {
                role = "data analyst",
                content = "You are a data analyst about booking at nail salon"
            },
            new
            {
                role = "user",
                content = prompt
            }
                },
                temperature = 0.7
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
            }

            try
            {
                var response = await _httpClient.PostAsync(TogetherAIUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine("AI Suggest Response: " + responseBody);

                var responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);

                response.EnsureSuccessStatusCode();

                Console.WriteLine("AI Peak Time Response: " + responseBody);
                
                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"AI error: {ex.Message}");
            }
            return "Không tìm thấy khung giờ";
        }

        public async Task<string?> SuggestOffPeakTimeFromAI(SuggestBooking dto)
        {
            var prompt = $"Tôi muốn đặt lịch vào ngày {dto.WorkingDate:dd/MM/yyyy} " +
             $"tại chi nhánh {dto.StoreName}, với thời gian làm việc từ {dto.StartTime} đến {dto.EndTime}. " +
             $"Thời gian nghỉ trưa là {dto.BreakTime} phút. " +
             $"Tổng giá trị booking là {dto.TotalAmount} VND. " +
             $"Xin hãy đề xuất một khung giờ ít cao điểm nhất trong khoảng thời gian làm việc, tránh giờ nghỉ trưa, " +
             $"và không trùng với giờ cao điểm thông thường. " +
             $"Vui lòng chỉ trả về một giờ duy nhất dạng HH:mm, không giải thích gì thêm.";

            var requestBody = new
            {
                model = "meta-llama/Llama-Vision-Free",
                messages = new[]
                {
            new
            {
                role = "system",
                content = "Bạn là một hệ thống AI đề xuất giờ đặt lịch ít cao điểm trong ngày."
            },
            new
            {
                role = "user",
                content = prompt
            }
                },
                temperature = 0.7
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
            }

            try
            {
                var response = await _httpClient.PostAsync(TogetherAIUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine("AI Suggest Response: " + responseBody);

                var responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);
                if (responseData?.choices?.Count > 0)
                {
                    string aiResponse = responseData.choices[0].message.content;
                    return aiResponse.Trim();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"AI error: {ex.Message}");
            }

            return "Không tìm thấy khung giờ";
        }


        public List<(TimeOnly Start, TimeOnly End)> ExtractPeakHours(string text)
        {
            var timeRanges = new List<(TimeOnly Start, TimeOnly End)>();
            var regex = new Regex(@"(\d{1,2}:\d{2})\s*-\s*(\d{1,2}:\d{2})");
            var matches = regex.Matches(text);

            foreach (Match match in matches)
            {
                if (match.Groups.Count == 3)
                {
                    var startTime = TimeOnly.Parse(match.Groups[1].Value);
                    var endTime = TimeOnly.Parse(match.Groups[2].Value);
                    timeRanges.Add((startTime, endTime));
                }
            }

            return timeRanges;
        }
    }
}