using INBS.Application.DTOs.ArtistStore;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing artists.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistStoreController(IArtistStoreService service) : ControllerBase
    {
        /// <summary>
        /// Retrieves all artist store records.
        /// </summary>
        /// <returns>An IQueryable of ArtistStoreResponse.</returns>
        [EnableQuery(MaxExpansionDepth = 10)]
        [HttpGet]
        public IQueryable<ArtistStoreResponse> Get()
        {
            try
            {
                return service.GetAll();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
