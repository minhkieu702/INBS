using INBS.Application.DTOs.Booking;
using INBS.Application.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.Booking
{
    /// <summary>
    /// Get Booking
    /// </summary>
    /// <param name="service"></param>
    public class BookingController(IBookingService service) : ODataController
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
    }
}
