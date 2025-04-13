using INBS.Application.DTOs.Feedback;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.FeedbackImage
{
    public class FeedbackImageResponse
    {
        public int ID { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Guid FeedbackId { get; set; }
        public int NumerialOrder { get; set; }
        public int MediaType { get; set; }
        public virtual FeedbackResponse? Feedback { get; set; }
    }
}
