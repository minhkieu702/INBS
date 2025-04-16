using INBS.Application.DTOs.Service;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers.NailDesignService
{
    [ApiController]
    [Route("api/NailDesignService")]
    public class NailDServiceCommandController(INailDesignServiceService _service) : ControllerBase
    {
        /// <summary>
        /// Updates the duration of a service.
        /// </summary>
        /// <param name="serviceDuration">The service duration request.</param>
        /// <returns>An action result.</returns>
        [HttpPost("Time")]
        public async Task<IActionResult> UpdateTime([FromForm] ServiceDurationRequest serviceDuration)
        {
            try
            {
                await _service.UpdateTime(serviceDuration);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
