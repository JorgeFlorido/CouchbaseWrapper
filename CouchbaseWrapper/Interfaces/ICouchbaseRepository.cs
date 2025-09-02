namespace CouchbaseWrapper.Interfaces
{
  public interface ICouchbaseRepository
  {
    Task<T?> GetAsync<T>(string id);
    Task UpsertAsync(string id, object value);
  }
}