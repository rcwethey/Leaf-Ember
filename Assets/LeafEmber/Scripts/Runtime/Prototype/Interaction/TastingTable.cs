using LeafEmber.Prototype.Events;
using UnityEngine;

namespace LeafEmber.Prototype.Interaction
{

public sealed class TastingTable : MonoBehaviour, IInteractable, IContextualInteractable
{
    public string InteractionPrompt => "Begin a focused prototype tasting";

    public InteractionPresentation Presentation => new(
        "Shaded tasting patio",
        "FOCUSED TASTING",
        "Taste a rested study cigar",
        "Observe construction and sensory progression, compare the result with its intent, then record a causal hypothesis for the next version.",
        "Consumes 1 rested cigar and 1 calendar block");

    public void Interact(InteractionContext context)
    {
        context.EventBus.Publish(new TastingTableRequestedEvent());
    }
}
}
