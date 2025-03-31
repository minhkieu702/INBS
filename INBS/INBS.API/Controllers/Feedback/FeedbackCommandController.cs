using INBS.Application.DTOs.Feedback;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace INBS.API.Controllers.Feedback
{
    /// <summary>
    /// Controller for handling feedback-related operations.
    /// </summary>
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackCommandController : ControllerBase
    {
        private readonly IFeedbackService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackCommandController"/> class.
        /// </summary>
        /// <param name="service">The feedback service.</param>
        public FeedbackCommandController(IFeedbackService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create the feedback.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] FeedbackRequest feedback)
        {
            try
            {
                await _service.Create(feedback);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update the feedback.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromForm] FeedbackRequest feedback)
        {
            try
            {
                await _service.Update(id, feedback);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete the feedback.
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            try
            {
                await _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
