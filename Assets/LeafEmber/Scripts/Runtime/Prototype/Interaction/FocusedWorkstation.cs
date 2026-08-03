using LeafEmber.Prototype.Events;
using UnityEngine;

namespace LeafEmber.Prototype.Interaction
{

public sealed class FocusedWorkstation : MonoBehaviour, IInteractable
{
    private string workTitle;
    private string workDescription;
    private int blockCost;

    public string InteractionPrompt => $"Begin {workTitle} ({blockCost} block)";

    public void Configure(string title, string description, int cost)
    {
        workTitle = title;
        workDescription = description;
        blockCost = Mathf.Max(1, cost);
    }

    public void Interact(InteractionContext context)
    {
        context.EventBus.Publish(
            new FocusedWorkRequestedEvent(workTitle, workDescription, blockCost));
    }
}
}
