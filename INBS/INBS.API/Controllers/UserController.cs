using INBS.Application.DTOs.User.User;
using INBS.Application.IServices;
using INBS.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing user accounts.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </remarks>
    /// <param name="service">The user service.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService service) : ControllerBase
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The user registration request model.</param>
        /// <returns>The registered user details.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            try
            {
                var response = await service.Register(request);
                return CreatedAtAction(nameof(Register), new { id = response.ID }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await service.Login(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
