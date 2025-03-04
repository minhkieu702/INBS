using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class FeedbackService
    {
        public Guid FeedbackId { get; set; }
        [ForeignKey(nameof(FeedbackId))]
        [InverseProperty(nameof(Feedback.FeedbackServices))]
        public virtual Feedback? Feedback { get; set; }

        public Guid ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        [InverseProperty(nameof(Service.FeedbackServices))]
        public virtual Service? Service { get; set; }
    }
}
