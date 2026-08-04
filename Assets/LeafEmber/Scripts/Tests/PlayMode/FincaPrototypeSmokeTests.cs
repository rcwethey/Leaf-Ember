using System.Collections;
using LeafEmber.Cigar;
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
        FincaExperienceHud experienceHud =
            Object.FindFirstObjectByType<FincaExperienceHud>();
        Assert.That(experienceHud, Is.Not.Null);
        Assert.That(experienceHud.IsModalOpen, Is.True);

        GameObject entryGate = GameObject.Find("Finca Entry Gate");
        GameObject estatePlot = GameObject.Find("Estate Tobacco Observation Plot");
        GameObject workshop = GameObject.Find("Personal Workshop");
        Assert.That(entryGate, Is.Not.Null);
        Assert.That(estatePlot, Is.Not.Null);
        Assert.That(workshop, Is.Not.Null);
        Assert.That(
            Vector3.Distance(estatePlot.transform.position, workshop.transform.position),
            Is.GreaterThan(60f),
            "Production landmarks should remain spatially distinct.");
        Assert.That(
            Resources.Load<Texture2D>("Surfaces/red-clay-earth"),
            Is.Not.Null,
            "The finca should retain its authored surface-material foundation.");
        Assert.That(
            Object.FindObjectsByType<FocusedWorkstation>(FindObjectsSortMode.None).Length,
            Is.GreaterThanOrEqualTo(2));
        Assert.That(
            Object.FindObjectsByType<InformationStation>(FindObjectsSortMode.None).Length,
            Is.GreaterThanOrEqualTo(3));
        Assert.That(
            Object.FindFirstObjectByType<LeafLotCabinet>(),
            Is.Not.Null);
        Assert.That(Object.FindFirstObjectByType<CigarWorkbench>(), Is.Not.Null);
        Assert.That(Object.FindFirstObjectByType<TastingTable>(), Is.Not.Null);
        Assert.That(Object.FindFirstObjectByType<CigarDevelopmentView>(), Is.Not.Null);

        Renderer[] renderers =
            Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
        Light[] lights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
        int shadowedPointLights = 0;
        foreach (Light light in lights)
        {
            if (light.type == LightType.Point && light.shadows != LightShadows.None)
            {
                shadowedPointLights++;
            }
        }

        TestContext.WriteLine(
            $"Finca performance budget: {renderers.Length} renderers, " +
            $"{shadowedPointLights} shadowed point lights.");
        Assert.That(
            renderers.Length,
            Is.LessThan(1000),
            "The procedural finca exceeded its renderer budget.");
        Assert.That(
            shadowedPointLights,
            Is.Zero,
            "Decorative point lights must not multiply full-scene shadow passes.");

        ICalendarService calendar = GameServices.Registry.Resolve<ICalendarService>();
        IInventoryService inventory = GameServices.Registry.Resolve<IInventoryService>();
        IEstateService estate = GameServices.Registry.Resolve<IEstateService>();
        ICigarDevelopmentService cigarDevelopment =
            GameServices.Registry.Resolve<ICigarDevelopmentService>();
        Assert.That(calendar.Current.block, Is.EqualTo(DayBlock.Morning));
        Assert.That(inventory.LeafLots, Has.Count.EqualTo(4));
        Assert.That(estate.Facilities, Has.Count.EqualTo(7));
        Assert.That(cigarDevelopment.Recipes, Is.Empty);
    }
}
}
