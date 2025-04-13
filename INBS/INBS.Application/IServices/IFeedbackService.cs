using INBS.Application.DTOs.Feedback;
using INBS.Application.DTOs.FeedbackImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IFeedbackService
    {
        IQueryable<FeedbackResponse> Get();

        Task Create(FeedbackRequest request, IList<FeedbackImageRequest> feedbackImageRequests);

        Task Update(Guid id, FeedbackRequest request, IList<FeedbackImageRequest> feedbackImageRequests);

        Task Delete(Guid id);
    }
}
