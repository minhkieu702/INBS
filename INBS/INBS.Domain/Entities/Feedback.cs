using INBS.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace INBS.Domain.Entities
{
    public class Feedback : BaseEntity
    {
        public Feedback() : base()
        {
            Rating = 5;
            Content = string.Empty;
        }
        public int FeedbackType { get; set; }
        
        public int Rating { get; set; }

        public string Content { get; set; }

        public Guid TypeId { get; set; }

        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.Feedbacks))]
        public virtual Customer? Customer { get; set; }
    }
}
