using Couchbase;

namespace CouchbaseWrapper.Interfaces
{
  public interface ICouchbaseBucket : IAsyncDisposable
  {
    Task<IBucket> GetBucketAsync();
  }
}
