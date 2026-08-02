using System;
using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Connection;

public sealed class ConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;
    private readonly Lazy<IMongoClient> _client;

    public ConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
        _client = new Lazy<IMongoClient>(CreateClient);
    }

    public IMongoClient GetClient()
    {
        return _client.Value;
    }

    public IMongoDatabase GetDatabase(string databaseName)
    {
        return GetClient().GetDatabase(databaseName);
    }

    private IMongoClient CreateClient()
    {
        MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(_connectionString));

        return new MongoClient(settings);
    }
}