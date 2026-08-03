using LeafEmber.Prototype.Events;
using UnityEngine;

namespace LeafEmber.Prototype.Interaction
{

public sealed class CigarWorkbench : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => "Open the cigar-development notebook";

    public void Interact(InteractionContext context)
    {
        context.EventBus.Publish(new CigarWorkbenchRequestedEvent());
    }
}
}
