using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace IntegrationTest.Infra.MongoDB.Commons;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class Context : IDisposable
{
    private readonly DatabaseKeeper _databaseKeeper;

    public ServiceResolver ServiceResolver { get; private set; }

    private bool _disposed;

    ~Context() => Dispose(false);

    public Context()
    {
        IConfiguration configuration = InitConfiguration();

        DockerKeeper dockerKeeper = new DockerKeeper(configuration);
        dockerKeeper.DockerComposeUp();

        _databaseKeeper = new DatabaseKeeper(configuration);
        _databaseKeeper.ClearCollectionsData();

        ServiceResolver = new ServiceResolver(configuration);
    }

    private static IConfiguration InitConfiguration()
    {
        return
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("test.settings.json")
                .Build();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (!disposing) return;

        _databaseKeeper.ClearCollectionsData();
        _disposed = true;
    }
}