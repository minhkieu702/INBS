using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Common
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            ID = Guid.NewGuid();
            IsDeleted = false;
            LastModifiedAt = DateTime.Now;
        }

        [Key]
        public Guid ID { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}
