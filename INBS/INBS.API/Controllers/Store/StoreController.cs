using INBS.Application.DTOs.Store;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.Store
{
    /// <summary>
    /// Get Store 
    /// </summary>
    /// <param name="service"></param>
    public class StoreController(IStoreService service) : ODataController
    {
        /// <summary>
        /// Gets the list of stores.
        /// </summary>
        /// <returns>A list of stores.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<StoreResponse> Get()
        {
            try
            {
                return service.Get();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
