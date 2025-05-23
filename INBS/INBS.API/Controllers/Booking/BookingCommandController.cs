﻿using INBS.Application.DTOs.Booking;
using INBS.Application.IService;
using INBS.Application.Services;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers.Booking
{
    /// <summary>
    /// Controller for managing bookings.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BookingCommandController"/> class.
    /// </remarks>
    /// <param name="service">The booking service.</param>
    [ApiController]
    [Route("api/Booking")]
    public class BookingCommandController(IBookingService service) : ControllerBase
    {

        ///// <summary>
        ///// Gets all bookings.
        ///// </summary>
        ///// <returns>A list of bookings.</returns>
        //[HttpGet]
        //[EnableQuery(MaxExpansionDepth = 100)]
        //public IQueryable<BookingResponse> Get()
        //{
        //    try
        //    {
        //        return service.Get();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="booking">The booking request.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BookingRequest booking)
        {
            try
            {
                return Ok(await service.Create(booking));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        //[HttpPost("create")]
        //public async Task<IActionResult> CreateBooking([FromBody] BookingRequest bookingRequest)
        //{
        //    if (bookingRequest == null)
        //    {
        //        return BadRequest("Invalid booking request");
        //    }

        //    try
        //    {
        //        await service.Create(bookingRequest);
        //        return Ok(new { Message = "Booking created successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "An error occurred while creating the booking");
        //    }
        //}

        /// <summary>
        /// Updates an existing booking.
        /// </summary>
        /// <param name="id">The booking ID.</param>
        /// <param name="booking">The booking request.</param>
        /// <returns>An action result.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] BookingRequest booking)
        {
            try
            {
                await service.Update(id, booking);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a booking.
        /// </summary>
        /// <param name="id">The booking ID.</param>
        /// <param name="reason">The reason of cancelation</param>
        /// <returns>An action result.</returns>
        [HttpPatch]
        public async Task<IActionResult> CancelBooking([FromQuery] Guid id, [FromForm] string reason)
        {
            try
            {
                await service.CancelBooking(id, reason);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Sets the booking status to servicing.
        /// </summary>
        /// <param name="id">The booking ID.</param>
        /// <returns>An action result.</returns>
        [HttpPost("Serving")]
        public async Task<IActionResult> Serving([FromQuery] Guid id)
        {
            try
            {
                await service.SetBookingIsServicing(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet("PredictCancel")]
        public async Task<IActionResult> PredictCancellation([FromQuery] Guid bookingId)
        {
            try
            {
                int probability = await service.PredictBookingCancel(bookingId);
                return Ok(new { CancellationProbability = probability });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("SuggestOffPeakTime")]
        public async Task<IActionResult> SuggestOffPeakTime([FromQuery] Guid bookingId)
        {
            try
            {
                var offPeakTime = await service.SuggestOffPeakTimeAsync(bookingId);
                if (string.IsNullOrEmpty(offPeakTime))
                {
                    return NotFound(new { Message = "Không tìm thấy khung giờ ít cao điểm." });
                }
                return Ok(new { SuggestedOffPeakTime = offPeakTime });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("SuggestArtist")]
        public async Task<IActionResult> SuggestArtist([FromForm] Guid storeId, [FromForm] DateOnly date, [FromForm] TimeOnly time, [FromForm] Guid customerSelectedId)
        {
            try
            {
                var suggestedArtists = await service.SuggestArtist(storeId, date, time, customerSelectedId);
                return Ok(suggestedArtists);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("SuggestTimeSlots")]
        public async Task<IActionResult> SuggestTimeSlots([FromForm] DateOnly date, [FromForm] Guid storeId)
        {
            try
            {
                var suggestedSlots = await service.SuggestTimeSlots(date, storeId);
                return Ok(suggestedSlots);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
