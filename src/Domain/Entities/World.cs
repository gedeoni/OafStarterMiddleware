using System;
using Domain.Common.Interfaces;

namespace Domain.Entities
{
    public class World : AuditableEntity, ICouchbaseEntity
    {
        public string Name { get; set; }
        public bool HasLife { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Entity { get; set; }
    }
}
