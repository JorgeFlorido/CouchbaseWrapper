using Couchbase;

namespace CouchbaseWrapper.Interfaces
{
  internal interface ICouchbaseCluster : IAsyncDisposable
  {
    Task<ICluster> GetClusterAsync();
  }
}
