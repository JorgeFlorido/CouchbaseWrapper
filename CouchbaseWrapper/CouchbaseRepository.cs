using Couchbase.Core.Exceptions.KeyValue;
using Couchbase.KeyValue;
using CouchbaseWrapper.Interfaces;

namespace CouchbaseWrapper
{
  public sealed class CouchbaseRepository : ICouchbaseRepository
  {
    private readonly ICouchbaseBucket _bucket;

    public CouchbaseRepository(ICouchbaseBucket bucket) => _bucket = bucket;

    public Task<T?> GetAsync<T>(string id, CancellationToken ct = default) =>
        ExecuteAsync(async (c, t) =>
        {
          try
          {
            var r = await c.GetAsync(id, o => o.CancellationToken(t));
            return r.ContentAs<T>();
          }
          catch (DocumentNotFoundException)
          {
            return default;
          }
        }, ct);

    public Task UpsertAsync(string id, object value, CancellationToken ct = default) =>
        ExecuteAsync((c, t) => c.UpsertAsync(id, value, o => o.CancellationToken(t)), ct);

    public Task InsertAsync(string id, object value, CancellationToken ct = default) =>
        ExecuteAsync((c, t) => c.InsertAsync(id, value, o => o.CancellationToken(t)), ct);

    public Task ReplaceAsync(string id, object value, CancellationToken ct = default) =>
        ExecuteAsync((c, t) => c.ReplaceAsync(id, value, o => o.CancellationToken(t)), ct);

    public Task RemoveAsync(string id, CancellationToken ct = default) =>
        ExecuteAsync((c, t) => c.RemoveAsync(id, o => o.CancellationToken(t)), ct);

    public Task<bool> ExistsAsync(string id, CancellationToken ct = default) =>
        ExecuteAsync(async (c, t) =>
        {
          var r = await c.ExistsAsync(id, o => o.CancellationToken(t));
          return r.Exists;
        }, ct);

    private async Task<TResult> ExecuteAsync<TResult>(
        Func<ICouchbaseCollection, CancellationToken, Task<TResult>> op,
        CancellationToken ct)
    {
      try
      {
        var bucket = await _bucket.GetBucketAsync();
        var collection = bucket.DefaultCollection();
        return await op(collection, ct);
      }
      catch (Exception ex)
      {
        throw new CouchbaseRepositoryException("Couchbase operation failed.", ex);
      }
    }

    private Task ExecuteAsync(
        Func<ICouchbaseCollection, CancellationToken, Task> op,
        CancellationToken ct) =>
        ExecuteAsync<object?>(async (c, t) => { await op(c, t); return null; }, ct);
  }
}
