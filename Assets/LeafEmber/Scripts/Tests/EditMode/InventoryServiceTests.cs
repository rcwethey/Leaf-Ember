using LeafEmber.Inventory;
using NUnit.Framework;

namespace LeafEmber.Tests
{

public sealed class InventoryServiceTests
{
    [Test]
    public void NewInventory_ContainsProvenanceAwarePrototypeLots()
    {
        InventoryService inventory = new();

        Assert.That(inventory.LeafLots, Has.Count.EqualTo(4));
        Assert.That(inventory.LeafLots[0].origin, Is.Not.Empty);
        Assert.That(inventory.LeafLots[0].grower, Is.Not.Empty);
        Assert.That(inventory.LeafLots[0].processHistory, Is.Not.Empty);
        Assert.That(inventory.LeafLots[0].potential, Is.Not.Null);
        Assert.That(inventory.LeafLots[0].process, Is.Not.Null);
        Assert.That(inventory.LeafLots[0].id, Is.Not.EqualTo(inventory.LeafLots[1].id));
    }

    [Test]
    public void Capture_ReturnsDefensiveCopy()
    {
        InventoryService inventory = new();

        InventorySnapshot snapshot = inventory.Capture();
        snapshot.leafLots[0].displayName = "Changed outside service";

        Assert.That(inventory.LeafLots[0].displayName, Is.Not.EqualTo(snapshot.leafLots[0].displayName));
    }

    [Test]
    public void Restore_PreservesCustomLotAndAddsCurrentDefinitions()
    {
        InventoryService inventory = new();
        InventorySnapshot snapshot = new();
        snapshot.leafLots.Add(new LeafLotState
        {
            id = "restored-lot",
            displayName = "Restored Lot",
            origin = "Test plot",
            quantityKilograms = 2f,
        });

        inventory.Restore(snapshot);

        Assert.That(inventory.LeafLots, Has.Count.EqualTo(5));
        Assert.That(inventory.LeafLots[0].id, Is.EqualTo("restored-lot"));
    }

    [Test]
    public void Restore_OldPrototypeSave_AddsNewLotsAndMissingPotential()
    {
        InventoryService inventory = new();
        InventorySnapshot oldSnapshot = new();
        oldSnapshot.leafLots.Add(new LeafLotState
        {
            id = "finca-pilot-seco",
            displayName = "Old saved estate lot",
            quantityKilograms = 3f,
        });

        inventory.Restore(oldSnapshot);

        Assert.That(inventory.LeafLots, Has.Count.EqualTo(4));
        Assert.That(inventory.LeafLots[0].quantityKilograms, Is.EqualTo(3f));
        Assert.That(inventory.LeafLots[0].potential, Is.Not.Null);
        Assert.That(inventory.LeafLots[0].process, Is.Not.Null);
    }
}
}
