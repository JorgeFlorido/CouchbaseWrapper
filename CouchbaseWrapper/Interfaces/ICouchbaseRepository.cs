namespace CouchbaseWrapper.Interfaces
{
  public interface ICouchbaseRepository
  {
    Task<T?> GetAsync<T>(string id, CancellationToken cancellationToken = default);
    Task UpsertAsync(string id, object value, CancellationToken cancellationToken = default);
    Task InsertAsync(string id, object value, CancellationToken cancellationToken = default);
    Task ReplaceAsync(string id, object value, CancellationToken cancellationToken = default);
    Task RemoveAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);
  }
}