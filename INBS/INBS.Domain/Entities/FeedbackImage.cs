using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class FeedbackImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Guid FeedbackId { get; set; }
        [ForeignKey(nameof(FeedbackId))]
        [InverseProperty(nameof(Feedback.FeedbackImages))]
        public virtual Feedback? Feedback { get; set; }
    }
}
