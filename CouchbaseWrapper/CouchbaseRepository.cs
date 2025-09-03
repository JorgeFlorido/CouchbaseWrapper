using Couchbase.Core.Exceptions.KeyValue;
using Couchbase.KeyValue;
using CouchbaseWrapper.Interfaces;

namespace CouchbaseWrapper
{
  public sealed class CouchbaseRepository : ICouchbaseRepository
  {
    private readonly ICouchbaseBucket _bucket;

    public CouchbaseRepository(ICouchbaseBucket bucket) => _bucket = bucket;

    public Task<T?> GetAsync<T>(string id, CancellationToken cancellationToken = default) where T : class =>
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
        }, cancellationToken);

    public Task UpsertAsync<T>(string id, T value, CancellationToken cancellationToken = default) where T : class =>
        ExecuteAsync((c, t) => c.UpsertAsync(id, value, o => o.CancellationToken(t)), cancellationToken);

    public Task InsertAsync<T>(string id, T value, CancellationToken cancellationToken = default) where T : class =>
        ExecuteAsync((c, t) => c.InsertAsync(id, value, o => o.CancellationToken(t)), cancellationToken);

    public Task ReplaceAsync<T>(string id, T value, CancellationToken cancellationToken = default) where T : class =>
        ExecuteAsync((c, t) => c.ReplaceAsync(id, value, o => o.CancellationToken(t)), cancellationToken);

    public Task RemoveAsync(string id, CancellationToken cancellationToken = default) =>
        ExecuteAsync((c, t) => c.RemoveAsync(id, o => o.CancellationToken(t)), cancellationToken);

    public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async (c, t) =>
        {
          var r = await c.ExistsAsync(id, o => o.CancellationToken(t));
          return r.Exists;
        }, cancellationToken);

    private async Task<TResult> ExecuteAsync<TResult>(
        Func<ICouchbaseCollection, CancellationToken, Task<TResult>> op,
        CancellationToken cancellationToken)
    {
      try
      {
        var bucket = await _bucket.GetBucketAsync();
        var collection = bucket.DefaultCollection();
        return await op(collection, cancellationToken);
      }
      catch (Exception ex)
      {
        throw new CouchbaseRepositoryException("Couchbase operation failed.", ex);
      }
    }

    private Task ExecuteAsync(
        Func<ICouchbaseCollection, CancellationToken, Task> op,
        CancellationToken cancellationToken) =>
        ExecuteAsync<object?>(async (c, t) => { await op(c, t); return null; }, cancellationToken);
  }
}
