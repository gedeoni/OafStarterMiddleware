using System;
using Domain.Common.Interfaces;

namespace Domain.Entities
{
    public class World : CouchbaseEntity<World>, IAuditableEntity
    {
        public string Name { get; set; }
        public bool HasLife { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
