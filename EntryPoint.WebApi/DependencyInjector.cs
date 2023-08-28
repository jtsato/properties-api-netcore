using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Core.Commons;
using Core.Domains.Properties.UseCases;
using EntryPoint.WebApi.Commons;
using EntryPoint.WebApi.Commons.Exceptions;
using EntryPoint.WebApi.Commons.Filters;
using EntryPoint.WebApi.Domains.Commons;
using EntryPoint.WebApi.Domains.Properties.EntryPoints;
using Infra.MongoDB.Commons.Connection;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.Properties.Model;
using Infra.MongoDB.Domains.Properties.Providers;
using Infra.MongoDB.Domains.Properties.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace EntryPoint.WebApi;

[ExcludeFromCodeCoverage]
public static class DependencyInjector
{
    private static readonly string ConnectionString =
        Environment.GetEnvironmentVariable("MONGODB_URL") ?? string.Empty;

    private static readonly string ServiceAccountFile =
        Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") ?? string.Empty;

    private static readonly string DatabaseName =
        Environment.GetEnvironmentVariable("MONGODB_DATABASE") ?? string.Empty;

    private static readonly string CollectionName =
        Environment.GetEnvironmentVariable("PROPERTY_COLLECTION_NAME") ?? string.Empty;

    public static void ConfigureServices(IServiceCollection services)
    {
        CreateServiceAccountFile();
        AddSharedServices(services);
        IConnectionFactory connectionFactory = new ConnectionFactory(ConnectionString);
        AddApplicationServices(services, connectionFactory);
    }

    private static void CreateServiceAccountFile()
    {
        string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (basePath == null) return;
        
        string serviceAccountPath = Path.Combine(basePath, ServiceAccountFile);
        if (File.Exists(serviceAccountPath)) return;

        StreamWriter streamWriter = File.CreateText(serviceAccountPath);
        streamWriter.Write(ServiceAccountFile);
        streamWriter.Close();
        
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", serviceAccountPath);
    }

    private static void AddSharedServices(IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddSingleton<IServiceResolver, ServiceResolver>();
        services.AddSingleton<IExceptionHandler, ExceptionHandler>();
        services.AddSingleton<IGetDateTime, GetDateTime>();
        services.AddSingleton<IGetCorrelationId, GetCorrelationId>();

        services.AddTransient<ILoggerAdapter, LoggerAdapter<ExceptionHandlerFilterAttribute>>();
    }

    private static void AddApplicationServices(IServiceCollection services, IConnectionFactory connectionFactory)
    {
        services.AddSingleton<IRepository<PropertyEntity>>(_ => new PropertyRepository(connectionFactory, DatabaseName, CollectionName));
        
        services.AddSingleton<ISearchPropertiesController, SearchPropertiesController>();
        services.AddSingleton<ISearchPropertiesUseCase, SearchPropertiesUseCase>();
        services.AddSingleton<ISearchPropertiesGateway, SearchPropertiesProvider>();
        
        services.AddSingleton<IGetPropertyByIdController, GetPropertyByIdController>();
        services.AddSingleton<IGetPropertyByIdUseCase, GetPropertyByIdUseCase>();
        services.AddSingleton<IGetPropertyByIdGateway, GetPropertyByIdProvider>();
    }
}