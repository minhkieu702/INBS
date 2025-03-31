using INBS.Application.DTOs.Feedback;
using INBS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace INBS.API.Controllers.Feedback
{
    /// <summary>
    /// Call Feedback With odata
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ODataController
    {
        private readonly IFeedbackService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackController"/> class.
        /// </summary>
        /// <param name="service">The feedback service.</param>
        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the feedback responses.
        /// </summary>
        /// <returns>An <see cref="IQueryable{FeedbackResponse}"/> of feedback responses.</returns>
        [HttpGet]
        [EnableQuery]
        public IQueryable<FeedbackResponse> Get()
        {
            return _service.Get();
        }
    }
}
