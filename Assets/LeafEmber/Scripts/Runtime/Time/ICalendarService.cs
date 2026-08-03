namespace LeafEmber.Time
{

public interface ICalendarService
{
    CalendarSnapshot Current { get; }

    int ElapsedBlocks { get; }

    CalendarAdvanceResult AdvanceBlocks(int blockCount, string reason);

    void Restore(CalendarSnapshot snapshot);

    void Schedule(ScheduledCheckpoint checkpoint);
}
}
