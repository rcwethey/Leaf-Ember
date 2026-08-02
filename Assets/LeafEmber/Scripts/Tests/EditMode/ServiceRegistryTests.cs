using System;
using LeafEmber.Core;
using NUnit.Framework;

namespace LeafEmber.Tests
{

public sealed class ServiceRegistryTests
{
    private interface IExampleService
    {
    }

    private sealed class ExampleService : IExampleService
    {
    }

    [Test]
    public void Register_ThenResolve_ReturnsSameInstance()
    {
        ServiceRegistry registry = new();
        IExampleService expected = new ExampleService();
        registry.Register(expected);
        Assert.That(registry.Resolve<IExampleService>(), Is.SameAs(expected));
    }

    [Test]
    public void Register_WhenTypeAlreadyRegistered_Throws()
    {
        ServiceRegistry registry = new();
        registry.Register<IExampleService>(new ExampleService());

        Assert.Throws<InvalidOperationException>(
            () => registry.Register<IExampleService>(new ExampleService()));
    }
}
}
