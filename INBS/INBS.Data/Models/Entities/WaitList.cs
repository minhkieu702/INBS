using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBS.Data.Models.Entities
{
    public class WaitList
    {
        public WaitList()
        {
            ID = Guid.NewGuid();
            AddedAt = DateTime.Now;
            UserWaitLists = [];
        }

        [Key]
        public Guid ID { get; set; }

        public DateOnly RequestedDate { get; set; }

        public TimeOnly RequestedTime { get; set; }

        public int Status { get; set; }

        public DateTime AddedAt { get; set; }

        [InverseProperty(nameof(UserWaitList.WaitList))]
        public virtual ICollection<UserWaitList> UserWaitLists { get; set; }
    }
}
