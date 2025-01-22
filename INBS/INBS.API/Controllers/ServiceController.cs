using INBS.Application.DTOs.Service.Service;
using INBS.Application.IService;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.ComponentModel.Design;

namespace INBS.API.Controllers
{
    /// <summary>
    /// Controller for managing services.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ServiceController"/> class.
    /// </remarks>
    /// <param name="service">The service service.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController(IServiceService service) : ControllerBase
    {
        private readonly IServiceService _service = service;

        /// <summary>
        /// Gets the list of services.
        /// </summary>
        /// <returns>A list of services.</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var services = await _service.Get();
                return Ok(services.AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Creates a new service, bắt buộc để imageUrl là null, còn nếu để Image là null thì sẽ tự động lấy ảnh mặc định có sẵn
        /// </summary>
        /// <param name="service">The service creation request.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ServiceRequest service)
        {
            try
            {
                await _service.Create(service);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing service.
        /// </summary>
        /// <param name="id">The service ID.</param>
        /// <param name="service">The service update request.</param>
        /// <returns>An action result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] ServiceRequest service)
        {
            try
            {
                await _service.Update(id, service);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a service by ID.
        /// </summary>
        /// <param name="id">The service ID.</param>
        /// <returns>An action result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
