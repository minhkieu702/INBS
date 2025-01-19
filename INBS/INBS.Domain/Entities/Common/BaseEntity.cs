using System.ComponentModel.DataAnnotations;

namespace INBS.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            ID = Guid.NewGuid();
            IsDeleted = false;
        }

        [Key]
        public Guid ID { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
} 