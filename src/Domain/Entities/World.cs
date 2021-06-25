using System;
using Domain.Common.Attributes;
using Domain.Common.Interfaces;

namespace Domain.Entities
{
    [EntityName("world")]
    public class World : AuditableEntity, ICouchbaseEntity
    {
        public string Name { get; set; }
        public bool HasLife { get; set; }
        public string Id { get; set; }
        public string Entity { get; set; }
    }
}
