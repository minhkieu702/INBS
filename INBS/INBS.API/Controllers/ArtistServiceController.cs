using INBS.Application.DTOs.ArtistService;
using INBS.Application.DTOs.NailDesignService;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for handling artist services.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistServiceController : ODataController
    {
        private readonly IArtistServiceService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistServiceController"/> class.
        /// </summary>
        /// <param name="service">The artist service service.</param>
        public ArtistServiceController(IArtistServiceService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the list of artist services.
        /// </summary>
        /// <returns>An <see cref="IQueryable{ArtistServiceResponse}"/> of artist services.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<ArtistServiceResponse> Get()
        {
            return _service.Get();
        }
    }
}
