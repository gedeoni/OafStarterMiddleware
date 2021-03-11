using Couchbase;
using Couchbase.KeyValue;

namespace Application.Common.Interfaces
{
    public interface ICouchbaseContext
    {
        IBucket Bucket { get; }
        ICouchbaseCollection Collection { get; }
    }
}
