using INBS.Application.DTOs.User.Artist.ArtistAvailability;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing artist availability.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistAvailabilityController : ControllerBase
    {
        private readonly IArtistAvailabilityService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistAvailabilityController"/> class.
        /// </summary>
        /// <param name="service">The artist availability service.</param>
        public ArtistAvailabilityController(IArtistAvailabilityService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the list of artist availabilities.
        /// </summary>
        /// <returns>A list of artist availabilities.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _service.Get();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new artist availability.
        /// </summary>
        /// <param name="request">The artist availability request.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ArtistAvailabilityRequest request)
        {
            try
            {
                await _service.Create(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing artist availability.
        /// </summary>
        /// <param name="id">The ID of the artist availability to update.</param>
        /// <param name="request">The artist availability request.</param>
        /// <returns>An action result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] ArtistAvailabilityRequest request)
        {
            try
            {
                await _service.Update(id, request);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an existing artist availability.
        /// </summary>
        /// <param name="id">The ID of the artist availability to delete.</param>
        /// <returns>An action result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
