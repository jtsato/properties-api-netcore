using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.Events;

namespace Infra.MongoDB.Commons.Connection;

public sealed class ConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;

    public ConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IMongoClient GetClient()
    {
        MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(_connectionString));
        settings.ClusterConfigurator = ClusterConfigurator;

        return new MongoClient(settings);

        void ClusterConfigurator(ClusterBuilder builder)
        {
            builder.Subscribe<CommandStartedEvent>(startedEvent => { Console.WriteLine($"{startedEvent.CommandName} - {startedEvent.Command.ToJson()}"); });
        }
    }

    public IMongoDatabase GetDatabase(string databaseName)
    {
        IMongoClient mongoDbClient = GetClient();

        return mongoDbClient.GetDatabase(databaseName);
    }
}