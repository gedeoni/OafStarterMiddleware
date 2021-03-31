using System;

namespace Domain.Common.Interfaces
{
    public abstract class AuditableEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}