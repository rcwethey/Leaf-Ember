using LeafEmber.Prototype.Events;
using UnityEngine;

namespace LeafEmber.Prototype.Interaction
{

public sealed class CigarWorkbench : MonoBehaviour, IInteractable, IContextualInteractable
{
    public string InteractionPrompt => "Open the cigar-development notebook";

    public InteractionPresentation Presentation => new(
        "Personal workshop",
        "CIGAR DEVELOPMENT",
        "Open the recipe notebook and rolling bench",
        "Record an intended experience, choose a composition, and make physical construction decisions. The result becomes evidence for tasting and revision.",
        "Browsing is free; constructing a study cigar costs 1 block");

    public void Interact(InteractionContext context)
    {
        context.EventBus.Publish(new CigarWorkbenchRequestedEvent());
    }
}
}
