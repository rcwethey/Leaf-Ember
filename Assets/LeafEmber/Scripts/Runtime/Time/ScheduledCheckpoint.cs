using System;

namespace LeafEmber.Time
{

[Serializable]
public sealed class ScheduledCheckpoint
{
    public string id;
    public string title;
    public string description;
    public int elapsedBlock;

    public ScheduledCheckpoint Copy()
    {
        return new ScheduledCheckpoint
        {
            id = id,
            title = title,
            description = description,
            elapsedBlock = elapsedBlock,
        };
    }
}
}
