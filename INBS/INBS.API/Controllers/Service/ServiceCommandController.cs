﻿using INBS.Application.DTOs.Service;
using INBS.Application.IService;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.ComponentModel.Design;

namespace INBS.API.Controllers.Service
{
    /// <summary>
    /// Controller for managing services.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ServiceCommandController"/> class.
    /// </remarks>
    /// <param name="service">The service service.</param>
    [ApiController]
    [Route("api/Service")]
    public class ServiceCommandController(IServiceService service) : ControllerBase
    {
        private readonly IServiceService _service = service;

        /// <summary>
        /// Creates a new service, bắt buộc để imageUrl là null, còn nếu để Image là null thì sẽ tự động lấy ảnh mặc định có sẵn
        /// </summary>
        /// <param name="service">The service creation request.</param>
        /// <param name="serviceNailDesigns">The list of service nail design requests.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ServiceRequest service, [FromForm] IList<ServiceNailDesignRequest> serviceNailDesigns)
        {
            try
            {
                await _service.Create(service, serviceNailDesigns);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing service.
        /// </summary>
        /// <param name="id">The service ID.</param>
        /// <param name="service">The service update request.</param>
        /// <param name="serviceNailDesigns">The list of service nail design requests.</param>
        /// <returns>An action result.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] ServiceRequest service, [FromForm] IList<ServiceNailDesignRequest> serviceNailDesigns)
        {
            try
            {
                await _service.Update(id, service, serviceNailDesigns);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a service by ID.
        /// </summary>
        /// <param name="id">The service ID.</param>
        /// <returns>An action result.</returns>
        [HttpPatch]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            try
            {
                await _service.DeleteById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
