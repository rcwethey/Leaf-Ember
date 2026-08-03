using LeafEmber.Prototype.Events;
using UnityEngine;

namespace LeafEmber.Prototype.Interaction
{

public sealed class LeafLotCabinet : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => "Inspect leaf lots and provenance (free)";

    public void Interact(InteractionContext context)
    {
        context.EventBus.Publish(new LotInspectionRequestedEvent());
    }
}
}
