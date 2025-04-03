using INBS.Application.DTOs.Feedback;
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

        Task Create(FeedbackRequest request);

        Task Update(Guid id, FeedbackRequest request);

        Task Delete(Guid id);
    }
}
