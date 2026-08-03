namespace LeafEmber.Prototype.Events
{

public readonly struct FocusedWorkRequestedEvent
{
    public FocusedWorkRequestedEvent(
        string title,
        string description,
        int blockCost)
    {
        Title = title;
        Description = description;
        BlockCost = blockCost;
    }

    public string Title { get; }

    public string Description { get; }

    public int BlockCost { get; }
}
}
