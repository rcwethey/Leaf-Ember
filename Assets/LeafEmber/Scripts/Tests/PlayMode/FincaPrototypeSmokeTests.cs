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
            Resources.Load<Texture2D>(
                "Environment/Materials/red_laterite_soil_stones/" +
                "red_laterite_soil_stones_diff_1k"),
            Is.Not.Null,
            "The finca should load its CC0 PBR laterite foundation.");
        Assert.That(
            Resources.Load<GameObject>(
                "Environment/Authored/Architecture/PersonalWorkshop_LOD0"),
            Is.Not.Null,
            "The finca should load its imported authored architecture.");
        Assert.That(
            Resources.Load<GameObject>(
                "Environment/ThirdParty/PolyHaven/jacaranda_tree/" +
                "jacaranda_tree_LOD1"),
            Is.Not.Null,
            "The finca should retain the lightweight dense-canopy tree LOD.");
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
        LODGroup[] lodGroups =
            Object.FindObjectsByType<LODGroup>(FindObjectsSortMode.None);
        MeshFilter[] meshFilters =
            Object.FindObjectsByType<MeshFilter>(FindObjectsSortMode.None);
        int visiblePrimitiveMeshes = 0;
        foreach (MeshFilter meshFilter in meshFilters)
        {
            string meshName = meshFilter.sharedMesh != null
                ? meshFilter.sharedMesh.name
                : string.Empty;
            if (meshName == "Cube" ||
                meshName == "Cylinder" ||
                meshName == "Sphere" ||
                meshName == "Capsule" ||
                meshName == "Plane" ||
                meshName == "Quad")
            {
                visiblePrimitiveMeshes++;
            }
        }

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
            $"{lodGroups.Length} LOD groups, {visiblePrimitiveMeshes} visible primitive meshes, " +
            $"{shadowedPointLights} shadowed point lights.");
        Assert.That(
            renderers.Length,
            Is.LessThan(600),
            "The authored finca exceeded its renderer budget.");
        Assert.That(
            lodGroups.Length,
            Is.GreaterThanOrEqualTo(80),
            "Repeated environment assets should retain authored LOD groups.");
        Assert.That(
            visiblePrimitiveMeshes,
            Is.LessThan(50),
            "Unity primitives have again become a dominant visible art layer.");
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
