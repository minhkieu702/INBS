using INBS.Application.DTOs.NailDesignService;
using INBS.Application.DTOs.Service;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.NailDesignService
{
    /// <summary>
    /// Controller for handling Nail Design Service related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NailDesignServiceController : ODataController
    {
        private readonly INailDesignServiceService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="NailDesignServiceController"/> class.
        /// </summary>
        /// <param name="service">The service to handle Nail Design Service operations.</param>
        public NailDesignServiceController(INailDesignServiceService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the list of Nail Design Services.
        /// </summary>
        /// <returns>An <see cref="IQueryable{NailDesignServiceResponse}"/> representing the list of Nail Design Services.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<NailDesignServiceResponse> Get()
        {
            return _service.Get();
        }
    }
}
