using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers.Admin
{
    /// <summary>
    /// Controller for admin-related actions.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AdminController"/> class.
    /// </remarks>
    /// <param name="service">The admin service.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController(IAdminService service) : ControllerBase
    {

        /// <summary>
        /// Creates a new admin.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing the result of the action.</returns>
        [HttpPost("/NewAdmin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var resukt = await service.Create();
                return Ok(resukt);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Logs in an admin.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the action.</returns>
        [HttpPost("/Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var response = await service.Login(username, password);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
