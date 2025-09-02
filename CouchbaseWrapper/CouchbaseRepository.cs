using Couchbase.Core.Exceptions.KeyValue;
using CouchbaseWrapper.Interfaces;

namespace CouchbaseWrapper
{
  public sealed class CouchbaseRepository : ICouchbaseRepository
  {
    private readonly ICouchbaseBucket _bucket;

    public CouchbaseRepository(ICouchbaseBucket bucket) => _bucket = bucket;

    public async Task<T?> GetAsync<T>(string id)
    {
      var bucket = await _bucket.GetBucketAsync();
      var collection = bucket.DefaultCollection();

      try
      {
        var result = await collection.GetAsync(id);
        return result.ContentAs<T>();
      }
      catch (DocumentNotFoundException)
      {
        return default;
      }
    }

    public async Task UpsertAsync(string id, object value)
    {
      var bucket = await _bucket.GetBucketAsync();
      var collection = bucket.DefaultCollection();
      await collection.UpsertAsync(id, value);
    }
  }
}
