using INBS.Application.IServices;
using INBS.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers.ArtistStore
{

    /// <summary>
    /// Controller for handling artist store commands.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistStoreCommandController(IArtistStoreService _service) : ControllerBase
    {
        /// <summary>
        /// Updates the status of an artist store.
        /// </summary>
        /// <param name="artistStoreId">The ID of the artist store.</param>
        /// <param name="status">The new status of the artist store.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Update([FromQuery] Guid artistStoreId, [FromForm] ArtistStoreStatus status)
        {
            try
            {
                await _service.Update(artistStoreId, status);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
