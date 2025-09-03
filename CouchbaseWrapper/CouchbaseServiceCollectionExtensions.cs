using CouchbaseWrapper.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

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

      services.AddScoped<ICouchbaseRepository, CouchbaseRepository>();

      return services;
    }

    private static void ValidateOptions(CouchbaseOptions options)
    {
      if (string.IsNullOrWhiteSpace(options.ConnectionString))
        throw new InvalidOperationException("Couchbase ConnectionString is required.");
      
      if (!Uri.TryCreate(options.ConnectionString, UriKind.Absolute, out _))
        throw new InvalidOperationException("Couchbase ConnectionString must be a valid URI.");
      
      if (string.IsNullOrWhiteSpace(options.Username))
        throw new InvalidOperationException("Couchbase Username is required.");
      
      if (string.IsNullOrWhiteSpace(options.Password))
        throw new InvalidOperationException("Couchbase Password is required.");
      
      if (string.IsNullOrWhiteSpace(options.BucketName))
        throw new InvalidOperationException("Couchbase BucketName is required.");
      
      if (options.BucketName.Length > 100)
        throw new InvalidOperationException("Couchbase BucketName cannot exceed 100 characters.");
      
      if (!Regex.IsMatch(options.BucketName, @"^[a-zA-Z0-9_-]+$"))
        throw new InvalidOperationException("Couchbase BucketName can only contain letters, numbers, underscores, and hyphens.");
    }
  }
}
