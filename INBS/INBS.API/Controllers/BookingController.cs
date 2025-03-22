using INBS.Application.DTOs.Booking;
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
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<BookingResponse> Get()
        {
            try
            {
                return service.Get();
            }
            catch (Exception)
            {

                throw;
            }
        }

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
                await service.Create(booking);
                return Ok();
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
    }
}
