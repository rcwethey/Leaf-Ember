namespace LeafEmber.Events
{

public readonly struct GameStartedEvent
{
    public GameStartedEvent(string applicationVersion)
    {
        ApplicationVersion = applicationVersion;
    }

    public string ApplicationVersion { get; }
}
}
