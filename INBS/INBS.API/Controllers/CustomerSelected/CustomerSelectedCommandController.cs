﻿using INBS.Application.DTOs.CustomerSelected;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers.CustomerSelected

{
    /// <summary>
    /// Controller for managing customer selected.
    /// </summary>
    [ApiController]
    [Route("api/CustomerSelected")]
    public class CustomerSelectedCommandController(ICustomerSelectedService service) : ControllerBase
    {
        ///// <summary>
        ///// Gets the list of customer selected.
        ///// </summary>
        ///// <returns>A list of customer selected.</returns>
        //[HttpGet]
        //[EnableQuery(MaxExpansionDepth = 100)]
        //public IQueryable<CustomerSelectedResponse> Get()
        //{
        //    return service.Get();
        //}

        [HttpPost("Design")]
        public async Task<IActionResult> BookingWithDesign([FromQuery] Guid designId, [FromQuery] Guid serviceId)
        {
            try
            {
                return Ok(await service.BookingWithDesign(designId, serviceId));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new customer selected.
        /// </summary>
        /// <param name="customerSelected">The customer selected request.</param>
        /// <param name="nailDesignServiceSelecteds">The list of service customer selected requests.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CustomerSelectedRequest customerSelected, [FromForm] IList<NailDesignServiceSelectedRequest> nailDesignServiceSelecteds)
        {
            try
            {
                var result = await service.Create(customerSelected, nailDesignServiceSelecteds);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing customer selected.
        /// </summary>
        /// <param name="id">The ID of the customer selected to update.</param>
        /// <param name="customerSelected">The customer selected request.</param>
        /// <param name="nailDesignServiceSelecteds">The list of service customer selected requests.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] CustomerSelectedRequest customerSelected, [FromForm] IList<NailDesignServiceSelectedRequest> nailDesignServiceSelecteds)
        {
            try
            {
                await service.Update(id, customerSelected, nailDesignServiceSelecteds);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a customer selected.
        /// </summary>
        /// <param name="id">The ID of the customer selected to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPatch]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
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