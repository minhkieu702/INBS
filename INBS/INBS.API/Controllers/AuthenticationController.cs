﻿using INBS.Application.DTOs.User.User;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for handling authentication.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IAuthenticationService service) : ControllerBase
    {
        /// <summary>
        /// Changes the profile of a user.
        /// </summary>
        /// <param name="request">The user request model containing profile details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("changeProfile")]
        public async Task<IActionResult> ChangeProfile([FromForm] UserRequest request)
        {
            try
            {
                await service.ChangeProfile(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Registers a new customer.
        /// </summary>
        /// <param name="request">The user registration request model.</param>
        /// <param name="confirmPassword">The confirmation of the password.</param>
        /// <param name="password">The password.</param>
        /// <returns>The registered user details.</returns>
        [HttpPost("customer/register")]
        public async Task<IActionResult> Register([FromForm] UserRequest request, [FromForm] string password, [FromForm] string confirmPassword)
        {
            try
            {
                await service.RegisterCustomer(request, password, confirmPassword);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
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
                return new BadRequestObjectResult(ex.Message);
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
                return new BadRequestObjectResult(ex.Message);
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

                return new BadRequestObjectResult(ex.Message);

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
                return new BadRequestObjectResult(ex.Message);
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
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid? id)
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
