using Couchbase;

namespace CouchbaseWrapper.Interfaces
{
  public interface ICouchbaseBucket
  {
    Task<IBucket> GetBucketAsync();
  }
}
