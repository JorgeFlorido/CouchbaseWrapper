using Couchbase;

namespace CouchbaseWrapper
{
  internal sealed class CouchbaseCluster : IAsyncDisposable
  {
    private readonly Lazy<Task<ICluster>> _cluster;

    public CouchbaseCluster(CouchbaseOptions options)
    {
      _cluster = new Lazy<Task<ICluster>>(() => 
        Cluster.ConnectAsync(options.ConnectionString, options.Username, options.Password));
    }

    public Task<ICluster> GetClusterAsync() => _cluster.Value;

    public async ValueTask DisposeAsync()
    {
      if (_cluster.IsValueCreated)
      {
        var cluster = await _cluster.Value;
        await cluster.DisposeAsync();
      }
    }
  }
}
