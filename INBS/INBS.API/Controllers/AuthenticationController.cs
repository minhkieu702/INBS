using INBS.Application.DTOs.Authentication.Customer;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for handling authenticaion.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IAuthenticationService service) : ControllerBase
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The user registration request model.</param>
        /// <returns>The registered user details.</returns>
        [HttpPost("customer/register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            try
            {
                await service.RegisterCustomer(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        [HttpPost("customer/login")]
        public async Task<IActionResult> Login([FromForm] string phoneNumber, [FromForm] string password)
        {
            try
            {
                var response = await service.LoginCustomer(phoneNumber, password);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Verify OTP before login
        /// </summary>
        /// <returns></returns>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtpForCustomer([FromBody] string phoneNumber, [FromForm] string otp)
        {
            try
            {
                var response = await service.VerifyOtpAsync(phoneNumber, otp);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Login for staff members.
        /// </summary>
        /// <param name="username">The username of the staff member.</param>
        /// <param name="password">The password of the staff member.</param>
        /// <returns>The login response containing access and refresh tokens.</returns>
        [HttpPost("staff/login")]
        public async Task<IActionResult> LoginStaff([FromForm] string username, [FromForm] string password)
        {

            try

            {

                var response = await service.LoginStaff(username, password);

                return Ok(response);

            }

            catch (Exception ex)

            {

                return BadRequest(new { message = ex.Message });

            }

        }
        /// <summary>
        /// Resets the password for a artist.
        /// </summary>
        /// <param name="username">The username of the artist.</param>
        /// <param name="newPassword">The new password for the artist.</param>
        /// <param name="confirmPassword">The confirmation of the new password.</param>
        /// <returns>The result of the password reset operation.</returns>
        [HttpPost("artist/reset-password")]
        public async Task<IActionResult> ResetPasswordStaff([FromForm] string username, [FromForm] string newPassword, [FromForm] string confirmPassword)
        {
            try
            {
                var response = await service.ResetPasswordStaff(username, newPassword, confirmPassword);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Resets the password for a customer.
        /// </summary>
        /// <param name="phoneNumber">The phone number of the customer.</param>
        /// <param name="newPassword">The new password for the customer.</param>
        /// <param name="confirmPassword">The confirmation of the new password.</param>
        /// <returns>The result of the password reset operation.</returns>
        [HttpPost("customer/reset-password")]
        public async Task<IActionResult> ResetPasswordCustomer([FromForm] string phoneNumber, [FromForm] string newPassword, [FromForm] string confirmPassword)
        {
            try
            {
                var response = await service.ResetPasswordCustomer(phoneNumber, newPassword, confirmPassword);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
