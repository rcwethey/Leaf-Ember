namespace LeafEmber.Prototype.Events
{

public readonly struct InformationRequestedEvent
{
    public InformationRequestedEvent(string title, string body)
    {
        Title = title;
        Body = body;
    }

    public string Title { get; }

    public string Body { get; }
}
}
