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
using Infra.Firestore.Commons.Connection;
using Infra.Firestore.Commons.Repository;
using Infra.Firestore.Domain.Properties.Model;
using Infra.Firestore.Domain.Properties.Providers;
using Infra.Firestore.Domain.Properties.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace EntryPoint.WebApi;

[ExcludeFromCodeCoverage]
public static class DependencyInjector
{
    private static readonly string ConnectionString =
        Environment.GetEnvironmentVariable("SERVICE_ACCOUNT_CONTENT") ?? string.Empty;

    private static readonly string ServiceAccountFile =
        Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") ?? string.Empty;

    private static readonly string DatabaseName =
        Environment.GetEnvironmentVariable("FIRESTORE_PROJECT_ID") ?? string.Empty;

    private static readonly string CollectionName =
        Environment.GetEnvironmentVariable("FIRESTORE_COLLECTION_NAME") ?? string.Empty;

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
        streamWriter.Write(ConnectionString);
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
        services.AddSingleton<ISearchPropertiesController, SearchPropertiesController>();
        services.AddSingleton<ISearchPropertiesUseCase, SearchPropertiesUseCase>();
        services.AddSingleton<ISearchPropertiesGateway, SearchPropertiesProvider>();
        services.AddSingleton<IRepository<PropertyEntity>>(_ => new PropertyRepository(connectionFactory, DatabaseName, CollectionName));
    }
}