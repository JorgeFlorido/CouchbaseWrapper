# Couchbase Wrapper for .NET

A lightweight and opinionated wrapper around the official [Couchbase .NET SDK](https://docs.couchbase.com/dotnet-sdk/current/hello-world/start-using-sdk.html), designed for clean integration with ASP.NET Core applications.

## ✨ Features

- Centralized cluster and bucket management (singleton, no TCP storm).
- Dependency Injection ready (`IServiceCollection` extensions).
- Configuration via `appsettings.json` and `IOptions<T>`.
- Safe asynchronous disposal of cluster and bucket resources.
- Generic repository interface for common CRUD operations.

## 📦 Installation

Clone the repo or add the project to your solution.

```bash
git clone https://github.com/JorgeFlorido/CouchbaseWrapper.git
