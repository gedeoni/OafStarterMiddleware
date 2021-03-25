using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Couchbase;
using Couchbase.Query;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class WorldRepository : CouchbaseRepository<World>, IWorldRepository
    {
        public WorldRepository(ICouchbaseContext couchbaseContext) : base(couchbaseContext)
        {
        }
    }
}
