namespace CouchbaseWrapper.Interfaces
{
  internal interface ICouchbaseCluster : IAsyncDisposable
  {
    Task<ICouchbaseBucket> GetBucketAsync();
  }
}
