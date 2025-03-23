using INBS.Application.DTOs.Artist;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OpenApi.Validations.Rules;

namespace INBS.API.Controllers.Artist
{
    /// <summary>
    /// Get Artist
    /// </summary>
    /// <param name="service"></param>
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController(IArtistService service) : ODataController
    {
        /// <summary>
        /// Gets the list of artists.
        /// </summary>
        /// <returns>A list of artists.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<ArtistResponse> Get()
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
