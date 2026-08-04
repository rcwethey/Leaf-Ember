using LeafEmber.Prototype.Events;
using UnityEngine;

namespace LeafEmber.Prototype.Interaction
{

public sealed class LeafLotCabinet : MonoBehaviour, IInteractable, IContextualInteractable
{
    public string InteractionPrompt => "Inspect leaf lots and provenance (free)";

    public InteractionPresentation Presentation => new(
        "Leaf storage",
        "LEAF LIBRARY",
        "Study the available leaf lots",
        "Compare origin, grower, process history, intended role, quantity, and house observations before designing a blend.",
        "Free — browse without spending a work block");

    public void Interact(InteractionContext context)
    {
        context.EventBus.Publish(new LotInspectionRequestedEvent());
    }
}
}
