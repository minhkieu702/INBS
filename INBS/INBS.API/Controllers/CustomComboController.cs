using INBS.Application.DTOs.Service.CustomCombo;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomComboController(ICustomComboService service) : ControllerBase
    {
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var cc = await service.Get();
                return Ok(cc.AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CustomComboRequest customCombo, [FromForm] IList<ServiceCustomComboRequest> serviceCustomCombos)
        {
            try
            {
                await service.Create(customCombo, serviceCustomCombos, User);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] CustomComboRequest customCombo, [FromForm] IList<ServiceCustomComboRequest> serviceCustomCombos)
        {
            try
            {
                await service.Update(id, customCombo, serviceCustomCombos, User);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            try
            {
                await service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


    }
}
