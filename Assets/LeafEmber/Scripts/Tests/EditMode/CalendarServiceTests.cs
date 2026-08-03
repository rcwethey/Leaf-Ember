using System;
using LeafEmber.Time;
using NUnit.Framework;

namespace LeafEmber.Tests
{

public sealed class CalendarServiceTests
{
    [Test]
    public void NewCalendar_StartsAtFirstMorning()
    {
        CalendarService calendar = new();

        CalendarSnapshot current = calendar.Current;

        Assert.That(current.year, Is.EqualTo(1));
        Assert.That(current.month, Is.EqualTo(1));
        Assert.That(current.day, Is.EqualTo(1));
        Assert.That(current.block, Is.EqualTo(DayBlock.Morning));
        Assert.That(calendar.ElapsedBlocks, Is.Zero);
    }

    [Test]
    public void AdvanceBlocks_CrossesDayMonthAndYearBoundaries()
    {
        CalendarService calendar = new();

        calendar.AdvanceBlocks(CalendarService.BlocksPerDay, "Complete one day of work");
        AssertCalendar(calendar.Current, 1, 1, 2, DayBlock.Morning);

        calendar.Restore(new CalendarSnapshot());
        calendar.AdvanceBlocks(CalendarService.BlocksPerMonth, "Complete one month of work");
        AssertCalendar(calendar.Current, 1, 2, 1, DayBlock.Morning);

        calendar.Restore(new CalendarSnapshot());
        calendar.AdvanceBlocks(CalendarService.BlocksPerYear, "Complete one year of work");
        AssertCalendar(calendar.Current, 2, 1, 1, DayBlock.Morning);
    }

    [Test]
    public void AdvanceBlocks_ReturnsCheckpointsReachedWithinWindow()
    {
        CalendarService calendar = new();
        calendar.Schedule(new ScheduledCheckpoint
        {
            id = "test-checkpoint",
            title = "Inspect the barn",
            elapsedBlock = 2,
        });

        CalendarAdvanceResult result = calendar.AdvanceBlocks(2, "Work at the bench");

        Assert.That(result.ReachedCheckpoints, Has.Count.EqualTo(1));
        Assert.That(result.ReachedCheckpoints[0].id, Is.EqualTo("test-checkpoint"));
        AssertCalendar(result.After, 1, 1, 1, DayBlock.Evening);
    }

    [Test]
    public void AdvanceBlocks_WithoutMeaningfulReason_Throws()
    {
        CalendarService calendar = new();

        Assert.Throws<ArgumentException>(() => calendar.AdvanceBlocks(1, " "));
        Assert.Throws<ArgumentOutOfRangeException>(() => calendar.AdvanceBlocks(0, "Wait"));
    }

    [Test]
    public void Current_ReturnsDefensiveCopy()
    {
        CalendarService calendar = new();

        CalendarSnapshot external = calendar.Current;
        external.year = 99;

        Assert.That(calendar.Current.year, Is.EqualTo(1));
    }

    private static void AssertCalendar(
        CalendarSnapshot actual,
        int year,
        int month,
        int day,
        DayBlock block)
    {
        Assert.That(actual.year, Is.EqualTo(year));
        Assert.That(actual.month, Is.EqualTo(month));
        Assert.That(actual.day, Is.EqualTo(day));
        Assert.That(actual.block, Is.EqualTo(block));
    }
}
}
