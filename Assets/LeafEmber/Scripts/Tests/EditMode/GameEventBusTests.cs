using LeafEmber.Events;
using NUnit.Framework;

namespace LeafEmber.Tests
{

public sealed class GameEventBusTests
{
    [Test]
    public void Publish_NotifiesSubscriber()
    {
        GameEventBus eventBus = new();
        int observed = 0;
        eventBus.Subscribe<int>(value => observed = value);

        eventBus.Publish(42);

        Assert.That(observed, Is.EqualTo(42));
    }
}
}
