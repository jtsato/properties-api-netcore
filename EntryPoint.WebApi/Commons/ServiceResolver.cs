using System;
using Core.Commons;

namespace EntryPoint.WebApi.Commons;

public sealed class ServiceResolver : IServiceResolver
{
    private IServiceProvider _serviceProvider;

    public void Setup(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T Resolve<T>()
    {
        Type type = typeof(T);
        T service = (T) _serviceProvider.GetService(type);
        return ArgumentValidator.CheckNull(service, nameof(type));
    }
}