using Couchbase;
using CouchbaseWrapper.Interfaces;

namespace CouchbaseWrapper
{
  internal sealed class CouchbaseBucket : ICouchbaseBucket
  {
    private readonly Lazy<Task<IBucket>> _bucket;

    public CouchbaseBucket(CouchbaseCluster cluster, CouchbaseOptions options)
    {
      _bucket = new Lazy<Task<IBucket>>(async () =>
      {
        var c = await cluster.GetClusterAsync();
        return await c.BucketAsync(options.BucketName);
      });
    }

    public Task<IBucket> GetBucketAsync() => _bucket.Value;

    public async ValueTask DisposeAsync()
    {
      if (_bucket.IsValueCreated)
      {
        var bucket = await _bucket.Value;
        await bucket.DisposeAsync();
      }
    }
  }
}
