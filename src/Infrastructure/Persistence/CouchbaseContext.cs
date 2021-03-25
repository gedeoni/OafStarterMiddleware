using Application.Common.Interfaces;
using Couchbase;
using Couchbase.KeyValue;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    public interface ICouchbaseContext
    {
        IBucket Bucket { get; }
        ICouchbaseCollection Collection { get; }
    }

    public class CouchbaseContext : ICouchbaseContext
    {
        public IBucket Bucket { get; }
        public ICouchbaseCollection Collection { get; }

        public CouchbaseContext(IRepaymentsBucket bucketProvider, ILogger<CouchbaseContext> logger)
        {
            Bucket = bucketProvider.GetBucketAsync().Result;
            logger.LogInformation($"successfully got a bucket, { Bucket}");

            Collection = Bucket.DefaultCollection();
            logger.LogInformation($"successfully got a default collection, { Collection}");
        }
    }
}
