using INBS.Application.DTOs.Customer;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.Customer
{

    /// <summary>
    /// Get With Odata.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController(ICustomerService service) : ODataController
    {
        /// <summary>
        /// Gets customers.
        /// </summary>
        /// <returns>Customer preferences.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<CustomerResponse> Get()
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
