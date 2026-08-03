using System.Collections.Generic;

namespace LeafEmber.Time
{

public sealed class CalendarAdvanceResult
{
    public CalendarAdvanceResult(
        CalendarSnapshot before,
        CalendarSnapshot after,
        string reason,
        IReadOnlyList<ScheduledCheckpoint> reachedCheckpoints)
    {
        Before = before;
        After = after;
        Reason = reason;
        ReachedCheckpoints = reachedCheckpoints;
    }

    public CalendarSnapshot Before { get; }

    public CalendarSnapshot After { get; }

    public string Reason { get; }

    public IReadOnlyList<ScheduledCheckpoint> ReachedCheckpoints { get; }
}
}
