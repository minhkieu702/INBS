using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBS.Data.Models.Entities
{
    public class UserWaitList
    {
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.UserWaitLists))]
        public virtual User? User { get; set; }

        public Guid WaitListId { get; set; }
        [ForeignKey(nameof(WaitListId))]
        [InverseProperty(nameof(WaitList.UserWaitLists))]
        public virtual WaitList? WaitList { get; set; }

        public bool IsArtist { get; set; }
    }
}
