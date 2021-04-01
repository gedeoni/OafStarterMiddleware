using Infrastructure.Persistence;
using Couchbase;
using Couchbase.KeyValue;
using Domain.Entities;

namespace Integration
{
    public class CouchbaseFixture
    {
        public TestWorldRepository worldRepository {get;}

        public CouchbaseFixture()
        {
            worldRepository = new TestWorldRepository();

        }
    }

    public class TestCouchbaseContext : ICouchbaseContext
    {
        public IBucket Bucket { get; }
        public ICouchbaseCollection Collection { get; }

        public TestCouchbaseContext()
        {
            ICluster cluster  = Cluster.ConnectAsync("couchbase://localhost:11210", "Administrator", "password").Result;
            Bucket =  cluster.BucketAsync("WorldTest").Result;
            Collection = Bucket.DefaultCollectionAsync().Result;
        }
    }

    public class TestWorldRepository : TestEntityRepository<World>
    {
        public TestWorldRepository()
        {
        }
    }
}