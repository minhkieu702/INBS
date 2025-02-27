using INBS.Application.DTOs.User.Artist;
using INBS.Application.DTOs.User.User;
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
    public class ArtistController(IArtistService service) : ControllerBase
    {
        /// <summary>
        /// Gets the list of artists.
        /// </summary>
        /// <returns>A list of artists.</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await service.Get();
                return Ok(result.AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new artist.
        /// </summary>
        /// <param name="artistRequest">The artist request.</param>
        /// <param name="userRequest">The user request.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ArtistRequest artistRequest, [FromForm] UserRequest userRequest)
        {
            try
            {
                await service.Create(artistRequest, userRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing artist.
        /// </summary>
        /// <param name="id">The artist ID.</param>
        /// <param name="artistRequest">The artist request.</param>
        /// <param name="userRequest">The user request.</param>
        /// <returns>An action result.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] ArtistRequest artistRequest, [FromForm] UserRequest userRequest)
        {
            try
            {
                await service.Update(id, artistRequest, userRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
        }
    }
}
