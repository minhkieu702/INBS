using INBS.Application.DTOs.Authentication.Customer;
using Microsoft.AspNetCore.Mvc;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for handling authenticaion.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        //[HttpPost("Customer/SignIn")]
        //public async Task<IActionResult> CustomerSignIn([FromBody] SignInRequest request)
        //{
        //    try
        //    {
        //        //var result = await _service.CustomerSignIn(request);
        //        //return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
    }
}
