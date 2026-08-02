using System;
using System.Collections.Generic;

namespace LeafEmber.Core
{

public sealed class ServiceRegistry
{
    private readonly Dictionary<Type, object> services = new();

    public void Register<TService>(TService service, bool replace = false)
        where TService : class
    {
        if (service == null)
        {
            throw new ArgumentNullException(nameof(service));
        }
        Type serviceType = typeof(TService);

        if (!replace && services.ContainsKey(serviceType))
        {
            throw new InvalidOperationException(
                $"A service is already registered for {serviceType.FullName}.");
        }

        services[serviceType] = service;
    }

    public TService Resolve<TService>()
        where TService : class
    {
        Type serviceType = typeof(TService);
        if (services.TryGetValue(serviceType, out object service))
        {
            return (TService)service;
        }

        throw new InvalidOperationException(
            $"No service is registered for {serviceType.FullName}.");
    }

    public bool TryResolve<TService>(out TService service)
        where TService : class
    {
        if (services.TryGetValue(typeof(TService), out object value))
        {
            service = (TService)value;
            return true;
        }

        service = null;
        return false;
    }
}
}
