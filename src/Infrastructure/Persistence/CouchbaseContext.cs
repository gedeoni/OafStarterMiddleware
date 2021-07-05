using Application.Common.Interfaces;
using Couchbase;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.KeyValue;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    public interface IWorldsBucket : INamedBucketProvider { }
    public class CouchbaseConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string[] Servers { get; set; }
        public bool UseSsl { get; set; }
        public string BucketName { get; set; }
    }
    public interface ICouchbaseContext
    {
        IBucket Bucket { get; }
        ICouchbaseCollection Collection { get; }
    }

    public class CouchbaseContext : ICouchbaseContext
    {
        public IBucket Bucket { get; }
        public ICouchbaseCollection Collection { get; }

        public CouchbaseContext(IWorldsBucket bucketProvider, ILogger<CouchbaseContext> logger)
        {
            Bucket = bucketProvider.GetBucketAsync().Result;
            logger.LogInformation($"successfully got a bucket, { Bucket}");

            Collection = Bucket.DefaultCollection();
            logger.LogInformation($"successfully got a default collection, { Collection}");
        }
    }
}
