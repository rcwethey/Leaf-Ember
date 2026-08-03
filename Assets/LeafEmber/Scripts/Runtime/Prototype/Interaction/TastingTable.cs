using LeafEmber.Prototype.Events;
using UnityEngine;

namespace LeafEmber.Prototype.Interaction
{

public sealed class TastingTable : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => "Begin a focused prototype tasting";

    public void Interact(InteractionContext context)
    {
        context.EventBus.Publish(new TastingTableRequestedEvent());
    }
}
}
