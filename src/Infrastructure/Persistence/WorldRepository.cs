using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Couchbase;
using Couchbase.Query;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    public class WorldRepository : CouchbaseRepository<World>, IWorldRepository
    {
        public WorldRepository(ICouchbaseContext couchbaseContext, ILogger<CouchbaseRepository<World>> logger, IConfiguration config) : base(couchbaseContext, logger, config)
        {
        }
    }
}
