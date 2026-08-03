using System;
using System.Collections.Generic;

namespace LeafEmber.Time
{

public sealed class CalendarService : ICalendarService
{
    public const int BlocksPerDay = 3;
    public const int DaysPerMonth = 8;
    public const int MonthsPerYear = 12;
    public const int BlocksPerMonth = BlocksPerDay * DaysPerMonth;
    public const int BlocksPerYear = BlocksPerMonth * MonthsPerYear;

    private readonly List<ScheduledCheckpoint> checkpoints = new();
    private CalendarSnapshot current = new();

    public CalendarSnapshot Current => current.Copy();

    public int ElapsedBlocks => ToElapsedBlocks(current);

    public CalendarAdvanceResult AdvanceBlocks(int blockCount, string reason)
    {
        if (blockCount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(blockCount),
                "Time must advance by at least one block.");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException(
                "A meaningful reason is required when time advances.",
                nameof(reason));
        }

        CalendarSnapshot before = Current;
        int previousElapsedBlocks = ElapsedBlocks;
        current = FromElapsedBlocks(previousElapsedBlocks + blockCount);

        List<ScheduledCheckpoint> reached = new();
        foreach (ScheduledCheckpoint checkpoint in checkpoints)
        {
            if (checkpoint.elapsedBlock > previousElapsedBlocks &&
                checkpoint.elapsedBlock <= ElapsedBlocks)
            {
                reached.Add(checkpoint.Copy());
            }
        }

        return new CalendarAdvanceResult(before, Current, reason, reached);
    }

    public void Restore(CalendarSnapshot snapshot)
    {
        Validate(snapshot);
        current = snapshot.Copy();
    }

    public void Schedule(ScheduledCheckpoint checkpoint)
    {
        if (checkpoint == null)
        {
            throw new ArgumentNullException(nameof(checkpoint));
        }

        if (string.IsNullOrWhiteSpace(checkpoint.id) ||
            string.IsNullOrWhiteSpace(checkpoint.title))
        {
            throw new ArgumentException(
                "A checkpoint requires an identifier and title.",
                nameof(checkpoint));
        }

        if (checkpoint.elapsedBlock < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(checkpoint),
                "A checkpoint cannot occur before the beginning of Year 1.");
        }

        checkpoints.RemoveAll(existing => existing.id == checkpoint.id);
        checkpoints.Add(checkpoint.Copy());
        checkpoints.Sort((left, right) => left.elapsedBlock.CompareTo(right.elapsedBlock));
    }

    public static int ToElapsedBlocks(CalendarSnapshot snapshot)
    {
        Validate(snapshot);
        return ((snapshot.year - 1) * BlocksPerYear) +
            ((snapshot.month - 1) * BlocksPerMonth) +
            ((snapshot.day - 1) * BlocksPerDay) +
            (int)snapshot.block;
    }

    public static CalendarSnapshot FromElapsedBlocks(int elapsedBlocks)
    {
        if (elapsedBlocks < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(elapsedBlocks));
        }

        int year = (elapsedBlocks / BlocksPerYear) + 1;
        int remainder = elapsedBlocks % BlocksPerYear;
        int month = (remainder / BlocksPerMonth) + 1;
        remainder %= BlocksPerMonth;
        int day = (remainder / BlocksPerDay) + 1;
        DayBlock block = (DayBlock)(remainder % BlocksPerDay);

        return new CalendarSnapshot
        {
            year = year,
            month = month,
            day = day,
            block = block,
        };
    }

    private static void Validate(CalendarSnapshot snapshot)
    {
        if (snapshot == null)
        {
            throw new ArgumentNullException(nameof(snapshot));
        }

        if (snapshot.year < 1 ||
            snapshot.month < 1 || snapshot.month > MonthsPerYear ||
            snapshot.day < 1 || snapshot.day > DaysPerMonth ||
            !Enum.IsDefined(typeof(DayBlock), snapshot.block))
        {
            throw new ArgumentException("The calendar snapshot is invalid.", nameof(snapshot));
        }
    }
}
}
