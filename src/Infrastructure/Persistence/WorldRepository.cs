using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using Domain.Entities;
using Domain.Common.Interfaces;
using Application.Common.Interfaces;

namespace Infrastructure.Persistence
{
    public class WorldRepository : CouchbaseRepository<World>, IWorldRepository
    {
        public WorldRepository(ICouchbaseContext couchbaseContext, ILogger<CouchbaseRepository<World>> logger, IConfiguration config)
            : base(couchbaseContext, logger, config)
        {
        }
    }
}
