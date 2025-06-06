﻿using INBS.Application.DTOs.Artist;
using INBS.Application.DTOs.Booking;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IService
{
    public interface IBookingService
    {
        Task<Guid> Create(BookingRequest request);

        Task CancelBooking(Guid id, string cancelation);

        Task Update(Guid id, BookingRequest request);

        IQueryable<BookingResponse> Get();

        Task SetBookingIsServicing(Guid id);

        Task<List<Booking>> GetBookingsWithinNextMinutes(int minutes);

        Task<int> PredictBookingCancel(Guid BookingId);

        Task<string?> SuggestOffPeakTimeAsync(Guid bookingId);

        Task<List<SuggestSlot>> SuggestTimeSlots(DateOnly date, Guid storeId);

        Task<List<ArtistResponse>> SuggestArtist(Guid storeId, DateOnly date, TimeOnly time, Guid customerSelectedId);
    }
}
