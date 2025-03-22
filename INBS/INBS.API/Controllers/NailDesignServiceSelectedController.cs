using INBS.Application.DTOs.NailDesignService;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for handling Nail Design Service Selected operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NailDesignServiceSelectedController : ODataController
    {
        private readonly INailDesignServiceSelectedService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="NailDesignServiceSelectedController"/> class.
        /// </summary>
        /// <param name="service">The service to handle Nail Design Service Selected operations.</param>
        public NailDesignServiceSelectedController(INailDesignServiceSelectedService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the list of Nail Design Service Selected.
        /// </summary>
        /// <returns>An <see cref="IQueryable{NailDesignServiceSelectedResponse}"/> of Nail Design Service Selected.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<NailDesignServiceSelectedResponse> Get()
        {
            return _service.Get();
        }
    }
}
