using INBS.Application.DTOs.Artist;
using INBS.Application.DTOs.ArtistService;
using INBS.Application.DTOs.ArtistStore;
using INBS.Application.DTOs.User;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Newtonsoft.Json;
using System.Diagnostics;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing artists.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController(IArtistService service) : ControllerBase
    {
        /// <summary>
        /// Gets the list of artists.
        /// </summary>
        /// <returns>A list of artists.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<ArtistResponse> Get()
        {
            return service.Get();
        }

        /// <summary>
        /// Creates a new artist.
        /// </summary>
        /// <param name="artist">The artist request.</param>
        /// <param name="user">The user request.</param>
        /// <param name="artistStores"></param>
        /// <param name="artistServices"></param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ArtistRequest artist, [FromForm] UserRequest user, [FromForm] IList<ArtistServiceRequest> artistServices, [FromForm] IList<ArtistStoreRequest> artistStores)
        {
            try
            {
                await service.Create(artist, user, artistServices, artistStores);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing artist.
        /// </summary>
        /// <param name="id">The artist ID.</param>
        /// <param name="artist">The artist request.</param>
        /// <param name="user">The user request.</param>
        /// <param name="artistServices"></param>
        /// <param name="artistStores"></param>
        /// <returns>An action result.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] ArtistRequest artist, [FromForm] UserRequest user,  [FromForm] IList<ArtistServiceRequest> artistServices, [FromForm] IList<ArtistStoreRequest> artistStores)
        {
            try
            {
                await service.Update(id, artist, user, artistServices, artistStores);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an artist.
        /// </summary>
        /// <param name="id">The artist ID.</param>
        /// <returns>An action result.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            try
            {
                await service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
