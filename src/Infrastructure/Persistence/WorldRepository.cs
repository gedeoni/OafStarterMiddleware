using Application.Common.Interfaces;
using Domain.Entities;
using System;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Query;
using System.Collections.Generic;

namespace Infrastructure.Persistence
{
    public class WorldRepository : CouchbaseRepository<World>, IWorldRepository
    {
        public WorldRepository(ICouchbaseContext couchbaseContext) : base(couchbaseContext)
        {
        }

        public async Task<IAsyncEnumerable<int>> GetTotalWorlds(string entityName)
        {
            var cluster = _couchbaseContext.Bucket.Cluster;
            string query = "SELECT RAW count(*) from World where entity = $entityName";
            var results =  await cluster.QueryAsync<int>( query, options =>
                    {
                        options.Parameter("entityName", entityName);
                    });

            return results.Rows;
        }
    }
}
