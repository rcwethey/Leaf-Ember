using LeafEmber.Prototype.Events;
using UnityEngine;

namespace LeafEmber.Prototype.Interaction
{

public sealed class InformationStation : MonoBehaviour, IInteractable, IContextualInteractable
{
    private string stationTitle;
    private string stationBody;

    public string InteractionPrompt => $"Inspect {stationTitle} (free)";

    public InteractionPresentation Presentation => new(
        stationTitle,
        "OBSERVATION",
        $"Inspect {stationTitle}",
        stationBody,
        "Free — observation never advances time");

    public void Configure(string title, string body)
    {
        stationTitle = title;
        stationBody = body;
    }

    public void Interact(InteractionContext context)
    {
        context.EventBus.Publish(new InformationRequestedEvent(stationTitle, stationBody));
    }
}
}
