using CouchbaseWrapper.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CouchbaseWrapper
{
  public static class CouchbaseServiceCollectionExtensions
  {
    public static IServiceCollection AddCouchbase(this IServiceCollection services, IConfiguration configuration)
    {
      var options = configuration.GetSection("Couchbase").Get<CouchbaseOptions>()
                    ?? throw new InvalidOperationException("Couchbase configuration is missing.");

      ValidateOptions(options);

      var cluster = new CouchbaseCluster(options);
      var bucket = new CouchbaseBucket(cluster, options);

      services.AddSingleton(cluster);
      services.AddSingleton<ICouchbaseBucket>(bucket);

      return services;
    }

    private static void ValidateOptions(CouchbaseOptions options)
    {
      if (string.IsNullOrWhiteSpace(options.ConnectionString) ||
          string.IsNullOrWhiteSpace(options.Username) ||
          string.IsNullOrWhiteSpace(options.Password) ||
          string.IsNullOrWhiteSpace(options.BucketName))
      {
        throw new InvalidOperationException("Couchbase configuration is missing or invalid.");
      }
    }
  }
}
