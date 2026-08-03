using System.Collections;
using LeafEmber.Core;
using LeafEmber.Estate;
using LeafEmber.Inventory;
using LeafEmber.Prototype.Interaction;
using LeafEmber.Prototype.Player;
using LeafEmber.Prototype.UI;
using LeafEmber.Time;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LeafEmber.Tests
{

public sealed class FincaPrototypeSmokeTests
{
    [UnityTest]
    public IEnumerator Startup_CreatesPlayableFincaAndRegisteredState()
    {
        yield return null;
        yield return null;

        GameObject world = GameObject.Find("[Leaf & Ember] Finca Prototype");
        GameObject founder = GameObject.Find("Founder");

        Assert.That(GameServices.IsInitialized, Is.True);
        Assert.That(world, Is.Not.Null);
        Assert.That(founder, Is.Not.Null);
        Assert.That(founder.GetComponent<CharacterController>(), Is.Not.Null);
        Assert.That(founder.GetComponent<PrototypePlayerController>(), Is.Not.Null);
        Assert.That(founder.GetComponent<PlayerInteractor>(), Is.Not.Null);
        Assert.That(Object.FindFirstObjectByType<PrototypeHud>(), Is.Not.Null);
        Assert.That(
            Object.FindObjectsByType<FocusedWorkstation>(FindObjectsSortMode.None).Length,
            Is.GreaterThanOrEqualTo(3));
        Assert.That(
            Object.FindObjectsByType<InformationStation>(FindObjectsSortMode.None).Length,
            Is.GreaterThanOrEqualTo(3));
        Assert.That(
            Object.FindFirstObjectByType<LeafLotCabinet>(),
            Is.Not.Null);

        ICalendarService calendar = GameServices.Registry.Resolve<ICalendarService>();
        IInventoryService inventory = GameServices.Registry.Resolve<IInventoryService>();
        IEstateService estate = GameServices.Registry.Resolve<IEstateService>();
        Assert.That(calendar.Current.block, Is.EqualTo(DayBlock.Morning));
        Assert.That(inventory.LeafLots, Has.Count.EqualTo(2));
        Assert.That(estate.Facilities, Has.Count.EqualTo(7));
    }
}
}
