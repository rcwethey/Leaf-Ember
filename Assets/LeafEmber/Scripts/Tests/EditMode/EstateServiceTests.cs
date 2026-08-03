using LeafEmber.Estate;
using NUnit.Framework;

namespace LeafEmber.Tests
{

public sealed class EstateServiceTests
{
    [Test]
    public void NewEstate_ContainsCoreProductionRoute()
    {
        EstateService estate = new();

        Assert.That(estate.Facilities, Has.Count.EqualTo(7));
        Assert.That(estate.Facilities, Has.Some.Matches<FacilityState>(
            facility => facility.id == "curing-barn"));
        Assert.That(estate.Facilities, Has.Some.Matches<FacilityState>(
            facility => facility.id == "workshop"));
        Assert.That(estate.Facilities, Has.Some.Matches<FacilityState>(
            facility => facility.id == "aging-room"));
    }

    [Test]
    public void CaptureAndRestore_PreserveFacilityCondition()
    {
        EstateService estate = new();
        EstateSnapshot snapshot = estate.Capture();
        snapshot.facilities[0].condition = "Repaired";

        estate.Restore(snapshot);

        Assert.That(estate.Facilities[0].condition, Is.EqualTo("Repaired"));
        snapshot.facilities[0].condition = "Changed externally";
        Assert.That(estate.Facilities[0].condition, Is.EqualTo("Repaired"));
    }
}
}
