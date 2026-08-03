using LeafEmber.Events;

namespace LeafEmber.Prototype.Interaction
{

public sealed class InteractionContext
{
    public InteractionContext(IEventBus eventBus)
    {
        EventBus = eventBus;
    }

    public IEventBus EventBus { get; }
}
}
