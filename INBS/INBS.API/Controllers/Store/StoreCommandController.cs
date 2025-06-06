﻿using INBS.Application.DTOs.Store;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers.Store
{
    /// <summary>
    /// Controller for managing store operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="StoreCommandController"/> class.
    /// </remarks>
    /// <param name="service">The store service.</param>
    [ApiController]
    [Route("api/Store")]
    public class StoreCommandController(IStoreService service) : ControllerBase
    {
        ///// <summary>
        ///// Gets the list of stores.
        ///// </summary>
        ///// <returns>A list of stores.</returns>
        //[HttpGet]
        //[EnableQuery(MaxExpansionDepth = 100)]
        //public IQueryable<StoreResponse> Get()
        //{
        //    return service.Get();
        //}

        /// <summary>
        /// Creates a new store.
        /// </summary>
        /// <param name="store">The store creation request.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StoreRequest store)
        {
            try
            {
                await service.Create(store);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing store.
        /// </summary>
        /// <param name="id">The store ID.</param>
        /// <param name="store">The store update request.</param>
        /// <returns>An action result.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] StoreRequest store)
        {
            try
            {
                await service.Update(id, store);
                return Ok();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a store by ID.
        /// </summary>
        /// <param name="id">The store ID.</param>
        /// <returns>An action result.</returns>
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
