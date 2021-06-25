using System;
using Domain.Common.Extensions;

namespace Domain.Common.Interfaces
{
    public abstract class CouchbaseEntity<TEntity>
    {
        protected CouchbaseEntity()
        {
            Entity = typeof(TEntity).GetEntityName();
        }
        public string Id { get; set; }
        public string Entity { get; }
    }
}