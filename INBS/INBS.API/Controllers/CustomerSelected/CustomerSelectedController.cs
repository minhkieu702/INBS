using INBS.Application.DTOs.Booking;
using INBS.Application.DTOs.CustomerSelected;
using INBS.Application.IService;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.CustomerSelected
{
    /// <summary>
    /// Get CustomerSelected
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerSelectedController(ICustomerSelectedService service) : ODataController
    {

        /// <summary>
        /// Gets all bookings.
        /// </summary>
        /// <returns>A list of bookings.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<CustomerSelectedResponse> Get()
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
