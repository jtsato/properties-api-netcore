using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Commons;
using Core.Domains.Properties.UseCases;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.Properties.Model;
using Infra.MongoDB.Domains.Properties.Providers;
using Infra.MongoDB.Domains.Properties.Repository;
using Microsoft.Extensions.Configuration;

namespace IntegrationTest.Infra.MongoDB.Commons;

public sealed class ServiceResolver : IServiceResolver
{
    private IConnectionFactory _connectionFactory;

    private string _databaseName;
    private string _propertyCollectionName;
    private string _propertySequenceCollectionName;

    private IRepository<PropertyEntity> _propertyRepository;
    private ISequenceRepository<PropertySequence> _propertySequenceRepository;

    private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public ServiceResolver(IConfiguration configuration)
    {
        LoadEnvironmentVariables(configuration);
        AddServices();
    }

    [ExcludeFromCodeCoverage]
    public T Resolve<T>()
    {
        Type type = typeof(T);

        if (_services.TryGetValue(type, out object value)) return (T) value;

        string message = $"Could not find the type {type} in {nameof(ServiceResolver)}";
        throw new ArgumentNullException(message, (Exception) null);
    }

    private void LoadEnvironmentVariables(IConfiguration configuration)
    {
        _connectionFactory = new ConnectionFactory(configuration["MONGODB_URL"]);
        _databaseName = configuration["MONGODB_DATABASE"];
        _propertyCollectionName = configuration["PROPERTY_COLLECTION_NAME"];
        _propertySequenceCollectionName = configuration["PROPERTY_SEQUENCE_COLLECTION_NAME"];
    }

    private void AddServices()
    {
        _services.Add(typeof(IRepository<PropertyEntity>), GetPropertyRepository());
        _services.Add(typeof(ISequenceRepository<PropertySequence>), GetPropertySequenceRepository());

        _services.Add(typeof(IGetPropertyByIdGateway), GetPropertyByIdGateway());
        _services.Add(typeof(ISearchPropertiesGateway), GetSearchPropertiesGateway());
    }

    private IRepository<PropertyEntity> GetPropertyRepository()
    {
        return _propertyRepository ??=
            new PropertyRepository(_connectionFactory, _databaseName, _propertyCollectionName);
    }

    private ISequenceRepository<PropertySequence> GetPropertySequenceRepository()
    {
        return _propertySequenceRepository ??=
            new PropertySequenceRepository(_connectionFactory, _databaseName, _propertySequenceCollectionName);
    }

    private ISearchPropertiesGateway GetSearchPropertiesGateway()
    {
        return new SearchPropertiesProvider(GetPropertyRepository());
    }

    private IGetPropertyByIdGateway GetPropertyByIdGateway()
    {
        return new GetPropertyByIdProvider(GetPropertyRepository());
    }
}