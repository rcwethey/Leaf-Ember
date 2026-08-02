using System;

namespace LeafEmber.Core
{

public static class GameServices
{
    private static ServiceRegistry registry;

    public static bool IsInitialized => registry != null;

    public static ServiceRegistry Registry =>
        registry ?? throw new InvalidOperationException(
            "Game services have not been initialized.");

    internal static void Initialize(ServiceRegistry serviceRegistry)
    {
        if (serviceRegistry == null)
        {
            throw new ArgumentNullException(nameof(serviceRegistry));
        }

        if (registry != null)
        {
            throw new InvalidOperationException(
                "Game services have already been initialized.");
        }

        registry = serviceRegistry;
    }

    internal static void Reset()
    {
        registry = null;
    }
}
}
