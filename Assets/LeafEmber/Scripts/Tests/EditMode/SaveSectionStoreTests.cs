using LeafEmber.Save;
using LeafEmber.Time;
using NUnit.Framework;

namespace LeafEmber.Tests
{

public sealed class SaveSectionStoreTests
{
    [Test]
    public void SetThenGet_RoundTripsTypedSection()
    {
        SaveGameData saveGame = SaveGameData.CreateNew("section-test");
        CalendarSnapshot expected = new()
        {
            year = 2,
            month = 4,
            day = 6,
            block = DayBlock.Afternoon,
        };

        SaveSectionStore.Set(saveGame, "calendar", expected);
        bool found = SaveSectionStore.TryGet(
            saveGame,
            "calendar",
            out CalendarSnapshot actual);

        Assert.That(found, Is.True);
        Assert.That(actual.year, Is.EqualTo(2));
        Assert.That(actual.month, Is.EqualTo(4));
        Assert.That(actual.day, Is.EqualTo(6));
        Assert.That(actual.block, Is.EqualTo(DayBlock.Afternoon));
    }

    [Test]
    public void Set_WithExistingKey_ReplacesSection()
    {
        SaveGameData saveGame = SaveGameData.CreateNew("section-test");

        SaveSectionStore.Set(saveGame, "calendar", new CalendarSnapshot());
        SaveSectionStore.Set(saveGame, "calendar", new CalendarSnapshot { year = 3 });

        Assert.That(saveGame.sections, Has.Count.EqualTo(1));
        SaveSectionStore.TryGet(saveGame, "calendar", out CalendarSnapshot actual);
        Assert.That(actual.year, Is.EqualTo(3));
    }
}
}
