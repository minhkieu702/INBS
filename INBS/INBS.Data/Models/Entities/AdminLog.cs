using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBS.Data.Models.Entities
{
    public class AdminLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        public string? Action { get; set; }
        
        public string? ActionDetail { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public Guid AdminId { get; set; }
        [ForeignKey(nameof(AdminId))]
        [InverseProperty(nameof(Admin.AdminLogs))]
        public virtual User? Admin { get; set; }
    }
}
