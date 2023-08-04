using System.Diagnostics.CodeAnalysis;
using Core.Commons;
using EntryPoint.WebApi.Commons;
using EntryPoint.WebApi.Commons.Exceptions;
using EntryPoint.WebApi.Commons.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace EntryPoint.WebApi;

[ExcludeFromCodeCoverage]
public static class DependencyInjector
{
    public static void ConfigureServices(IServiceCollection services)
    {
        AddSharedServices(services);
        AddApplicationServices(services);
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

    private static void AddApplicationServices(IServiceCollection services)
    {
        // TODO: Add your services here
        // services.AddTransient<IXYZController, XYZController>();
        // services.AddTransient<IXYZUseCase, XYZUseCase>();
        // services.AddTransient<IXYZGateway, XYZProvider>();
    }
}
