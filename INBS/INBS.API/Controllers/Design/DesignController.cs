using INBS.Application.DTOs.Design;
using INBS.Application.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.Design
{
    /// <summary>
    /// Get Design With Odata
    /// </summary>
    /// <param name="service"></param>
    public class DesignController(IDesignService service) : ODataController
    {

        /// <summary>
        /// Gets the list of designs.
        /// </summary>
        /// <returns>A list of designs.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 100)]
        public IQueryable<DesignResponse> Get()
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