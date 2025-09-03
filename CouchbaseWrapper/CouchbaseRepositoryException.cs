namespace CouchbaseWrapper
{
  internal class CouchbaseRepositoryException : Exception
  {
    public CouchbaseRepositoryException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
  }
}
