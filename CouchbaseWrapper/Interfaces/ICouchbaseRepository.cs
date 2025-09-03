namespace CouchbaseWrapper.Interfaces
{
  public interface ICouchbaseRepository
  {
    Task<T?> GetAsync<T>(string id, CancellationToken cancellationToken = default) where T : class;
    Task UpsertAsync<T>(string id, T value, CancellationToken cancellationToken = default) where T : class;
    Task InsertAsync<T>(string id, T value, CancellationToken cancellationToken = default) where T : class;
    Task ReplaceAsync<T>(string id, T value, CancellationToken cancellationToken = default) where T : class;
    Task RemoveAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);
  }
}