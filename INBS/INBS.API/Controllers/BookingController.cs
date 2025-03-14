﻿using INBS.Application.DTOs.Booking;
using INBS.Application.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing bookings.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BookingController"/> class.
    /// </remarks>
    /// <param name="service">The booking service.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController(IBookingService service) : ControllerBase
    {

        /// <summary>
        /// Gets all bookings.
        /// </summary>
        /// <returns>A list of bookings.</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await service.Get();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="booking">The booking request.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingRequest booking)
        {
            try
            {
                await service.Create(booking);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

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
        /// <returns>An action result.</returns>
        [HttpPatch]
        public async Task<IActionResult> CancelBooking([FromQuery] Guid id)
        {
            try
            {
                await service.CancelBooking(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
