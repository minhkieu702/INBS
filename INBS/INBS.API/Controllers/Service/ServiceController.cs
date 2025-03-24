using INBS.Application.DTOs.Service;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.Service
{
    /// <summary>
    /// Controller for managing services.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ServiceCommandController"/> class.
    /// </remarks>
    /// <param name="service">The service service.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController(IServiceService service) : ODataController
    {
        private readonly IServiceService _service = service;

        /// <summary>
        /// Gets the list of services.
        /// </summary>
        /// <returns>A list of services.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<ServiceResponse> Get()
        {
            try
            {
                return _service.Get();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}