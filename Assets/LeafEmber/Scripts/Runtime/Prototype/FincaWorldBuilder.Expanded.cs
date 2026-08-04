using LeafEmber.Prototype.Interaction;
using UnityEngine;

namespace LeafEmber.Prototype
{

public static partial class FincaWorldBuilder
{
    private static void BuildExpandedEnvironment(Transform parent)
    {
        authoredPalette = CreateAuthoredPalette();
        Material ground = authoredPalette.ground;
        Material earth = authoredPalette.earth;
        Material plaster = authoredPalette.plaster;
        Material wood = authoredPalette.wood;
        Material roof = authoredPalette.roof;
        Material foundation = authoredPalette.stone;
        Material leaf = authoredPalette.livingLeaf;
        Material leafLight = authoredPalette.livingLeafLight;
        Material curedLeaf = authoredPalette.curedLeaf;
        Material stem = authoredPalette.livingLeaf;
        Material metal = authoredPalette.metal;
        Material glass = authoredPalette.glass;
        Material water = CreateMaterial("Cistern Water", new Color(0.08f, 0.28f, 0.29f), null, null, 0.65f);
        Material cloth = authoredPalette.cloth;

        ConfigureExpandedAtmosphere();
        ConfigureAuthoredSky();
        InstantiateBackdrop(parent);
        InstantiateTerrain(parent);
        HandmadeFincaAssets.CreateCourtyardSurface(
            parent,
            new Vector3(0f, 0f, -8f),
            13.8f,
            10.8f,
            earth);

        BuildPathNetwork(parent, earth);
        BuildEstateBoundary(parent, wood, metal);
        BuildExpandedField(parent, earth, wood, stem, leaf, leafLight);
        BuildExpandedCuringBarn(parent, wood, roof, wood, foundation, glass, curedLeaf);
        BuildExpandedFermentation(parent, plaster, roof, wood, foundation, glass, curedLeaf);
        BuildExpandedStorage(parent, plaster, roof, wood, foundation, glass, curedLeaf, cloth);
        BuildExpandedWorkshop(parent, plaster, roof, wood, foundation, glass, curedLeaf, metal);
        BuildExpandedAging(parent, plaster, roof, wood, foundation, glass, curedLeaf);
        BuildExpandedOffice(parent, plaster, roof, wood, foundation, glass);
        BuildHomestead(parent, plaster, roof, wood, foundation, glass, cloth);
        BuildTastingCourtyard(
            parent,
            wood,
            roof,
            metal,
            glass,
            curedLeaf,
            earth,
            foundation,
            water,
            stem,
            leafLight);
        AddLandscapeDetail(parent, wood, stem, leaf, leafLight, cloth, metal);
    }

    private static void ConfigureExpandedAtmosphere()
    {
        HandmadeFincaAssets.ConfigureAtmosphere();
        QualitySettings.shadowDistance = Mathf.Min(QualitySettings.shadowDistance, 55f);
        QualitySettings.shadowCascades = 2;
        RenderSettings.fogStartDistance = 72f;
        RenderSettings.fogEndDistance = 185f;
        RenderSettings.fogColor = new Color(0.58f, 0.67f, 0.65f);
        RenderSettings.ambientIntensity = 0.66f;
    }

    private static void BuildPathNetwork(Transform parent, Material earth)
    {
        HandmadeFincaAssets.CreatePathRibbon(
            parent,
            "Arrival Road",
            new[]
            {
                new Vector3(0f, 0f, -52f),
                new Vector3(0.5f, 0f, -42f),
                new Vector3(-1f, 0f, -30f),
                new Vector3(0f, 0f, -20f),
                new Vector3(0f, 0f, -13f),
            },
            3.8f,
            earth);
        HandmadeFincaAssets.CreatePathRibbon(
            parent,
            "Western Production Path",
            new[]
            {
                new Vector3(-4f, 0f, -9f),
                new Vector3(-16f, 0f, -9f),
                new Vector3(-27f, 0f, -5f),
                new Vector3(-38f, 0f, -9f),
                new Vector3(-45f, 0f, -12f),
            },
            3f,
            earth);
        HandmadeFincaAssets.CreatePathRibbon(
            parent,
            "Curing Barn Path",
            new[]
            {
                new Vector3(-18f, 0f, -7f),
                new Vector3(-23f, 0f, 4f),
                new Vector3(-29f, 0f, 13f),
                new Vector3(-34f, 0f, 17f),
            },
            2.7f,
            earth);
        HandmadeFincaAssets.CreatePathRibbon(
            parent,
            "Northern Process Path",
            new[]
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, 0f, 11f),
                new Vector3(-3f, 0f, 21f),
                new Vector3(-8f, 0f, 29f),
                new Vector3(12f, 0f, 31f),
            },
            3f,
            earth);
        HandmadeFincaAssets.CreatePathRibbon(
            parent,
            "Eastern Craft Path",
            new[]
            {
                new Vector3(8f, 0f, -7f),
                new Vector3(19f, 0f, -7f),
                new Vector3(29f, 0f, -5f),
                new Vector3(30f, 0f, 8f),
                new Vector3(31f, 0f, 17f),
            },
            3f,
            earth);
        HandmadeFincaAssets.CreatePathRibbon(
            parent,
            "Southern House Path",
            new[]
            {
                new Vector3(0f, 0f, -20f),
                new Vector3(-10f, 0f, -28f),
                new Vector3(-19f, 0f, -35f),
            },
            2.5f,
            earth);
    }

    private static void BuildEstateBoundary(
        Transform parent,
        Material wood,
        Material metal)
    {
        InstantiateFenceLine(parent, new Vector3(-66f, 0f, -52f), new Vector3(-3f, 0f, -52f));
        InstantiateFenceLine(parent, new Vector3(3f, 0f, -52f), new Vector3(66f, 0f, -52f));
        InstantiateFenceLine(parent, new Vector3(-66f, 0f, 52f), new Vector3(66f, 0f, 52f));
        InstantiateFenceLine(parent, new Vector3(-66f, 0f, -52f), new Vector3(-66f, 0f, 52f));
        InstantiateFenceLine(parent, new Vector3(66f, 0f, -52f), new Vector3(66f, 0f, 52f));
        InstantiateEntryGate(parent, new Vector3(0f, 0f, -52f));
    }

    private static void BuildExpandedField(
        Transform parent,
        Material earth,
        Material wood,
        Material stem,
        Material leaf,
        Material leafLight)
    {
        Transform field = CreateZone("Estate Tobacco Observation Plot", parent, new Vector3(-46f, 0f, -10f));
        InstantiateFieldPlot(field);
        for (int x = -12; x <= 12; x += 4)
        {
            for (int z = -12; z <= 12; z += 4)
            {
                float phase = ((x + 12) * 0.23f) + ((z + 12) * 0.17f);
                InstantiateAuthoredTobacco(
                    field,
                    new Vector3(x, 0.08f, z),
                    phase * Mathf.Rad2Deg,
                    0.96f + (Mathf.Sin(phase) * 0.08f));
            }
        }

        InstantiateFenceLine(field, new Vector3(-14.5f, 0f, -15.6f), new Vector3(14.5f, 0f, -15.6f));
        GameObject plotSign = HandmadeFincaAssets.CreatePhysicalSign(
            field,
            new Vector3(0f, 1.45f, -16.1f),
            "ESTATE PILOT PLOT",
            wood,
            new Color(0.92f, 0.80f, 0.54f));
        InformationStation station = plotSign.AddComponent<InformationStation>();
        station.Configure(
            "estate pilot plot",
            "This small plot establishes a local house voice rather than supplying every possible leaf. " +
            "Middle-priming estate seco tends toward gentler strength, wood, and restrained green character. " +
            "Sourced viso, binder, and wrapper broaden what the workshop can build.");
    }

    private static void BuildExpandedCuringBarn(
        Transform parent,
        Material plaster,
        Material roof,
        Material wood,
        Material foundation,
        Material glass,
        Material curedLeaf)
    {
        Transform barn = CreateDetailedBuilding(
            "Curing Barn",
            parent,
            new Vector3(-34f, 0f, 18f),
            new Vector2(19f, 16f),
            -90f,
            plaster,
            roof,
            wood,
            foundation,
            glass,
            "CURING BARN");
        GameObject firstRack = null;
        for (int index = -1; index <= 1; index++)
        {
            GameObject rack = InstantiateProductionLod(
                "CuringRack",
                barn,
                new Vector3(index * 5.4f, 0.28f, 1.2f),
                Quaternion.Euler(0f, 90f, 0f),
                Vector3.one * 0.88f,
                new Vector3(0f, 1.35f, 0f),
                new Vector3(3.2f, 2.8f, 6f));
            firstRack ??= rack;
        }

        InformationStation station = firstRack.AddComponent<InformationStation>();
        station.Configure(
            "hanging cure",
            "Leaves hang with air space between them so color and moisture can change gradually. " +
            "This inspection is free: look for uneven drying, damage, or a barn condition that would justify intervention.");
        for (int index = 0; index < 3; index++)
        {
            InstantiatePolyHavenProp(
                "wooden_crate_01",
                barn,
                new Vector3(-7f + (index * 0.78f), 0.34f + ((index % 2) * 0.34f), 6f),
                Quaternion.Euler(0f, index * 13f, 0f),
                Vector3.one * 1.12f,
                "CC0 Curing Barn Crate");
        }
    }

    private static void BuildExpandedFermentation(
        Transform parent,
        Material plaster,
        Material roof,
        Material wood,
        Material foundation,
        Material glass,
        Material curedLeaf)
    {
        Transform room = CreateDetailedBuilding(
            "Fermentation House",
            parent,
            new Vector3(-9f, 0f, 32f),
            new Vector2(16f, 15f),
            0f,
            plaster,
            roof,
            wood,
            foundation,
            glass,
            "FERMENTATION");
        GameObject workingPilon = InstantiateProductionAsset(
            "PilonStack",
            room,
            new Vector3(-3.2f, 0.18f, 1.5f),
            Quaternion.identity,
            Vector3.one,
            new Vector3(0f, 1.1f, 0f),
            new Vector3(3.7f, 2.3f, 2.8f));
        InstantiateProductionAsset(
            "PilonStack",
            room,
            new Vector3(3.2f, 0.18f, 1.5f),
            Quaternion.identity,
            Vector3.one,
            new Vector3(0f, 1.1f, 0f),
            new Vector3(3.7f, 2.3f, 2.8f));
        FocusedWorkstation station = workingPilon.AddComponent<FocusedWorkstation>();
        station.Configure(
            "turn the pilot pilón",
            "Open the fermenting stack, compare warmth and moisture across its layers, then rebuild it " +
            "so transformation remains even. This prototype records the time commitment while the full process simulation arrives later.",
            1);
    }

    private static void BuildExpandedStorage(
        Transform parent,
        Material plaster,
        Material roof,
        Material wood,
        Material foundation,
        Material glass,
        Material curedLeaf,
        Material cloth)
    {
        Transform storage = CreateDetailedBuilding(
            "Leaf Storage",
            parent,
            new Vector3(13f, 0f, 32f),
            new Vector2(17f, 15f),
            0f,
            plaster,
            roof,
            wood,
            foundation,
            glass,
            "LEAF STORAGE");
        for (int index = -1; index <= 1; index++)
        {
            InstantiateProductionAsset(
                "PilonStack",
                storage,
                new Vector3(index * 3.2f, 0.18f, 2.6f),
                Quaternion.Euler(0f, index * 4f, 0f),
                new Vector3(0.72f, 0.58f, 0.72f),
                new Vector3(0f, 1.1f, 0f),
                new Vector3(3.7f, 2.3f, 2.8f));
        }

        GameObject cabinet = InstantiateProductionAsset(
            "AgingShelf",
            storage,
            new Vector3(0f, 0.18f, -5.5f),
            Quaternion.Euler(0f, 180f, 0f),
            new Vector3(1.75f, 1f, 1f),
            new Vector3(0f, 1.5f, 0f),
            new Vector3(2.4f, 3f, 0.95f));
        cabinet.name = "Authored Provenance Cabinet";
        cabinet.AddComponent<LeafLotCabinet>();
        for (int index = 0; index < 3; index++)
        {
            InstantiatePolyHavenProp(
                "wooden_crate_01",
                storage,
                new Vector3(-5.6f + (index * 0.82f), 0.32f + ((index % 2) * 0.34f), 4.6f),
                Quaternion.Euler(0f, index * 18f, 0f),
                Vector3.one * 1.15f,
                "CC0 Provenance Crate");
        }

        InstantiatePolyHavenProp(
            "wooden_ladder",
            storage,
            new Vector3(6.4f, 0.12f, 4.9f),
            Quaternion.Euler(0f, -18f, 0f),
            Vector3.one * 1.65f,
            "CC0 Storage Ladder");
    }

    private static void BuildExpandedWorkshop(
        Transform parent,
        Material plaster,
        Material roof,
        Material wood,
        Material foundation,
        Material glass,
        Material curedLeaf,
        Material metal)
    {
        Transform workshop = CreateDetailedBuilding(
            "Personal Workshop",
            parent,
            new Vector3(31f, 0f, -5f),
            new Vector2(19f, 15f),
            90f,
            plaster,
            roof,
            wood,
            foundation,
            glass,
            "PERSONAL WORKSHOP");
        GameObject workSurface = InstantiateProductionAsset(
            "RollingWorkbench",
            workshop,
            new Vector3(0f, 0.18f, 1.6f),
            Quaternion.identity,
            Vector3.one,
            new Vector3(0f, 0.72f, 0f),
            new Vector3(3.6f, 1.45f, 1.7f));
        workSurface.AddComponent<CigarWorkbench>();
        for (int index = 0; index < 2; index++)
        {
            InstantiatePolyHavenProp(
                "wooden_crate_01",
                workshop,
                new Vector3(-6.5f + (index * 0.85f), 0.35f, 4.8f),
                Quaternion.Euler(0f, index * 14f, 0f),
                Vector3.one * 1.18f,
                "CC0 Workshop Crate");
        }

        GameObject conditioningCabinet = InstantiateProductionAsset(
            "AgingShelf",
            workshop,
            new Vector3(6.4f, 0.18f, 4.7f),
            Quaternion.Euler(0f, 180f, 0f),
            new Vector3(0.86f, 0.92f, 0.86f),
            new Vector3(0f, 1.5f, 0f),
            new Vector3(2.4f, 3f, 0.95f));
        conditioningCabinet.name = "Authored Wrapper Conditioning Cabinet";
    }

    private static void BuildExpandedAging(
        Transform parent,
        Material plaster,
        Material roof,
        Material wood,
        Material foundation,
        Material glass,
        Material curedLeaf)
    {
        Transform aging = CreateDetailedBuilding(
            "Aging Room",
            parent,
            new Vector3(31f, 0f, 18f),
            new Vector2(18f, 14f),
            90f,
            plaster,
            roof,
            wood,
            foundation,
            glass,
            "AGING ROOM");
        GameObject ledgerShelf = null;
        for (int index = -2; index <= 2; index++)
        {
            GameObject shelf = InstantiateProductionAsset(
                "AgingShelf",
                aging,
                new Vector3(index * 2.75f, 0.18f, 3.4f),
                Quaternion.identity,
                Vector3.one,
                new Vector3(0f, 1.5f, 0f),
                new Vector3(2.4f, 3f, 0.95f));
            if (index == 0)
            {
                ledgerShelf = shelf;
            }
        }

        InformationStation station = ledgerShelf.AddComponent<InformationStation>();
        station.Configure(
            "aging ledger",
            "No mature house release exists yet. The ledger will preserve batch identity, storage history, " +
            "and observations so aging becomes a documented change—not a universal longer-is-better bonus.");
    }

    private static void BuildExpandedOffice(
        Transform parent,
        Material plaster,
        Material roof,
        Material wood,
        Material foundation,
        Material glass)
    {
        Transform office = CreateDetailedBuilding(
            "Finca Office",
            parent,
            new Vector3(-19f, 0f, -35f),
            new Vector2(17f, 13f),
            180f,
            plaster,
            roof,
            wood,
            foundation,
            glass,
            "FINCA OFFICE");
        GameObject desk = InstantiateProductionAsset(
            "PlanningDesk",
            office,
            new Vector3(0f, 0.18f, 1f),
            Quaternion.identity,
            Vector3.one,
            new Vector3(0f, 0.65f, 0f),
            new Vector3(3.8f, 1.35f, 1.6f));
        FocusedWorkstation station = desk.AddComponent<FocusedWorkstation>();
        station.Configure(
            "organize the house records",
            "Reconcile lot provenance, upcoming checkpoints, and workshop evidence. Reading is free; " +
            "deliberately reorganizing the working record consumes focused time.",
            1);
        GameObject recordsCabinet = InstantiateProductionAsset(
            "AgingShelf",
            office,
            new Vector3(-5.6f, 0.18f, 4.2f),
            Quaternion.identity,
            new Vector3(0.88f, 1f, 1f),
            new Vector3(0f, 1.5f, 0f),
            new Vector3(2.4f, 3f, 0.95f));
        recordsCabinet.name = "Authored Records Cabinet";
        InstantiatePolyHavenProp(
            "painted_wooden_bench",
            office,
            new Vector3(3.8f, 0.18f, -7.6f),
            Quaternion.identity,
            Vector3.one * 1.8f,
            "CC0 Office Veranda Bench");
    }

    private static void BuildHomestead(
        Transform parent,
        Material plaster,
        Material roof,
        Material wood,
        Material foundation,
        Material glass,
        Material cloth)
    {
        Transform home = CreateDetailedBuilding(
            "Founder Homestead",
            parent,
            new Vector3(17f, 0f, -36f),
            new Vector2(19f, 14f),
            180f,
            plaster,
            roof,
            wood,
            foundation,
            glass,
            "CASA DE LA FINCA");
        InstantiatePolyHavenProp(
            "painted_wooden_bench",
            home,
            new Vector3(-4f, 0.18f, -8.1f),
            Quaternion.identity,
            Vector3.one * 1.9f,
            "CC0 Homestead Veranda Bench");
        InstantiatePolyHavenProp(
            "wooden_crate_01",
            home,
            new Vector3(6.6f, 0.34f, 4.6f),
            Quaternion.Euler(0f, 12f, 0f),
            Vector3.one * 1.15f,
            "CC0 Homestead Utility Crate");
    }

    private static void BuildTastingCourtyard(
        Transform parent,
        Material wood,
        Material roof,
        Material metal,
        Material glass,
        Material curedLeaf,
        Material earth,
        Material foundation,
        Material water,
        Material stem,
        Material leaf)
    {
        Transform patio = CreateZone("Shaded Tasting Patio", parent, new Vector3(1f, 0f, -7f));
        GameObject tastingSet = InstantiateAuthoredModel(
            "Environment/Authored/Courtyard/TastingPergolaSet",
            patio,
            new Vector3(0f, 0.10f, 0f),
            Quaternion.identity,
            Vector3.one,
            "Authored Tasting Pergola and Furniture");
        AddBoxCollider(
            tastingSet,
            new Vector3(0f, 0.78f, 0f),
            new Vector3(2.8f, 1.3f, 2.8f));
        tastingSet.AddComponent<TastingTable>();
        InstantiateAuthoredModel(
            "Environment/Authored/Courtyard/CourtyardCistern",
            parent,
            new Vector3(-9.5f, 0.08f, -4f),
            Quaternion.identity,
            Vector3.one,
            "Authored Courtyard Cistern");
        InstantiatePolyHavenProp(
            "painted_wooden_bench",
            parent,
            new Vector3(-7.8f, 0.10f, -16.8f),
            Quaternion.Euler(0f, 18f, 0f),
            Vector3.one * 1.75f,
            "CC0 Courtyard Bench West");
        InstantiatePolyHavenProp(
            "painted_wooden_bench",
            parent,
            new Vector3(8.2f, 0.10f, -16.2f),
            Quaternion.Euler(0f, -18f, 0f),
            Vector3.one * 1.75f,
            "CC0 Courtyard Bench East");
        InstantiateBroadleafCluster(
            parent,
            new Vector3(-13.6f, 0f, -16.5f),
            18f,
            2.4f);
        InstantiateBroadleafCluster(
            parent,
            new Vector3(13.2f, 0f, -15.5f),
            -24f,
            2.2f);
        InstantiateBroadleafCluster(parent, new Vector3(-11.5f, 0f, 0f), 36f, 1.8f);
        InstantiateBroadleafCluster(parent, new Vector3(11.8f, 0f, -0.5f), -42f, 1.8f);
    }

    private static void AddLandscapeDetail(
        Transform parent,
        Material wood,
        Material stem,
        Material leaf,
        Material leafLight,
        Material cloth,
        Material metal)
    {
        Vector3[] treePositions =
        {
            new(-55f, 0f, 35f),
            new(-46f, 0f, 43f),
            new(-24f, 0f, 44f),
            new(-5f, 0f, 47f),
            new(14f, 0f, 46f),
            new(28f, 0f, 44f),
            new(49f, 0f, 34f),
            new(56f, 0f, 23f),
            new(56f, 0f, 8f),
            new(56f, 0f, -12f),
            new(53f, 0f, -25f),
            new(38f, 0f, -45f),
            new(11f, 0f, -47f),
            new(-14f, 0f, -47f),
            new(-39f, 0f, -43f),
            new(-57f, 0f, -31f),
            new(-60f, 0f, -2f),
            new(-60f, 0f, 18f),
        };
        for (int index = 0; index < treePositions.Length; index++)
        {
            InstantiateIslandTree(
                parent,
                treePositions[index],
                index * 37f,
                2.85f + ((index % 4) * 0.28f));
        }

        Vector3[] jacarandaPositions =
        {
            new(-50f, 0f, 29f),
            new(-31f, 0f, 47f),
            new(7f, 0f, 49f),
            new(45f, 0f, 36f),
            new(55f, 0f, -2f),
            new(47f, 0f, -36f),
            new(-27f, 0f, -46f),
            new(-58f, 0f, -20f),
        };
        for (int index = 0; index < jacarandaPositions.Length; index++)
        {
            InstantiateJacarandaTree(
                parent,
                jacarandaPositions[index],
                19f + (index * 43f),
                0.72f + ((index % 3) * 0.09f));
        }

        Vector3[] windbreakPositions =
        {
            new(-62f, 0f, -18f),
            new(-61f, 0f, -9f),
            new(-60f, 0f, 0f),
            new(48f, 0f, 15f),
            new(51f, 0f, 20f),
            new(47f, 0f, 25f),
        };
        foreach (Vector3 position in windbreakPositions)
        {
            InstantiateBroadleafCluster(
                parent,
                position,
                (position.x + position.z) * 5f,
                2.5f);
        }

        Vector3[] courtyardPlantings =
        {
            new(-28f, 0f, -27f),
            new(-10f, 0f, -27f),
            new(8f, 0f, -29f),
            new(26f, 0f, -27f),
            new(-24f, 0f, 24f),
            new(-18f, 0f, 25f),
            new(19f, 0f, 25f),
            new(42f, 0f, 11f),
            new(42f, 0f, -16f),
            new(-51f, 0f, 9f),
        };
        for (int index = 0; index < courtyardPlantings.Length; index++)
        {
            InstantiateBroadleafCluster(
                parent,
                courtyardPlantings[index],
                index * 41f,
                1.85f + ((index % 3) * 0.24f));
        }

        for (int index = 0; index < 40; index++)
        {
            float angle = index * Mathf.PI * 2f / 40f;
            float radiusX = 17f + ((index % 4) * 1.3f);
            float radiusZ = 13f + ((index % 3) * 1.1f);
            InstantiateGroundCluster(
                parent,
                new Vector3(
                    Mathf.Cos(angle) * radiusX,
                    0.02f,
                    -8f + (Mathf.Sin(angle) * radiusZ)),
                index * 29f,
                1.25f + ((index % 3) * 0.18f));
        }

        for (int index = 0; index < 3; index++)
        {
            InstantiatePolyHavenProp(
                "wooden_crate_01",
                parent,
                new Vector3(22f + (index * 0.75f), 0.34f, -25f + ((index % 2) * 0.62f)),
                Quaternion.Euler(0f, index * 17f, 0f),
                Vector3.one * 1.15f,
                "CC0 Courtyard Supply Crate");
        }
    }

    private static Transform CreateDetailedBuilding(
        string name,
        Transform parent,
        Vector3 position,
        Vector2 size,
        float yaw,
        Material wall,
        Material roof,
        Material wood,
        Material foundation,
        Material glass,
        string signText)
    {
        if (authoredPalette != null)
        {
            Transform authoredBuilding = CreateZone(name, parent, position);
            authoredBuilding.localRotation = Quaternion.Euler(0f, yaw, 0f);
            InstantiateBuildingShell(
                GetAuthoredBuildingAssetName(name),
                authoredBuilding,
                size);
            GameObject authoredSign = HandmadeFincaAssets.CreatePhysicalSign(
                authoredBuilding,
                new Vector3(0f, 3.15f, -(size.y * 0.5f) - 0.38f),
                signText,
                wood,
                new Color(0.94f, 0.83f, 0.57f));
            authoredSign.transform.localScale = new Vector3(0.78f, 0.78f, 0.78f);
            return authoredBuilding;
        }

        Transform building = CreateZone(name, parent, position);
        building.localRotation = Quaternion.Euler(0f, yaw, 0f);
        float halfWidth = size.x * 0.5f;
        float halfDepth = size.y * 0.5f;
        const float wallHeight = 4.2f;
        const float doorWidth = 3.2f;
        float frontSegmentWidth = (size.x - doorWidth) * 0.5f;

        CreateBox(
            "Raised Stone Foundation",
            building,
            new Vector3(0f, 0.10f, 0f),
            new Vector3(size.x + 0.7f, 0.36f, size.y + 0.7f),
            foundation);
        CreateBox(
            "Interior Floor",
            building,
            new Vector3(0f, 0.31f, 0f),
            new Vector3(size.x, 0.14f, size.y),
            earthMaterialFallback(wall));
        CreateBox(
            "Back Plaster Wall",
            building,
            new Vector3(0f, 2.3f, halfDepth),
            new Vector3(size.x, wallHeight, 0.34f),
            wall);
        CreateBox(
            "Left Plaster Wall",
            building,
            new Vector3(-halfWidth, 2.3f, 0f),
            new Vector3(0.34f, wallHeight, size.y),
            wall);
        CreateBox(
            "Right Plaster Wall",
            building,
            new Vector3(halfWidth, 2.3f, 0f),
            new Vector3(0.34f, wallHeight, size.y),
            wall);
        for (int side = -1; side <= 1; side += 2)
        {
            CreateBox(
                "Front Plaster Wall",
                building,
                new Vector3(
                    side * ((doorWidth * 0.5f) + (frontSegmentWidth * 0.5f)),
                    2.3f,
                    -halfDepth),
                new Vector3(frontSegmentWidth, wallHeight, 0.34f),
                wall);
        }

        CreateBox(
            "Door Lintel",
            building,
            new Vector3(0f, 3.72f, -halfDepth - 0.02f),
            new Vector3(doorWidth, 1.35f, 0.38f),
            wall);

        GameObject leftDoor = CreateBox(
            "Open Left Timber Door",
            building,
            new Vector3(-1.55f, 1.65f, -halfDepth - 0.25f),
            new Vector3(1.45f, 2.65f, 0.14f),
            wood);
        leftDoor.transform.localRotation = Quaternion.Euler(0f, -67f, 0f);
        GameObject rightDoor = CreateBox(
            "Open Right Timber Door",
            building,
            new Vector3(1.55f, 1.65f, -halfDepth - 0.25f),
            new Vector3(1.45f, 2.65f, 0.14f),
            wood);
        rightDoor.transform.localRotation = Quaternion.Euler(0f, 67f, 0f);

        CreateBox(
            "Covered Veranda",
            building,
            new Vector3(0f, 0.34f, -halfDepth - 1.45f),
            new Vector3(size.x + 1.6f, 0.18f, 3.2f),
            wood);
        for (int side = -1; side <= 1; side += 2)
        {
            CreateBox(
                "Veranda Post",
                building,
                new Vector3(side * (halfWidth - 0.4f), 2.25f, -halfDepth - 2.7f),
                new Vector3(0.28f, 3.85f, 0.28f),
                wood);
        }

        CreateWindow(building, -halfWidth + 2.1f, -halfDepth - 0.20f, wallHeight, wood, glass);
        CreateWindow(building, halfWidth - 2.1f, -halfDepth - 0.20f, wallHeight, wood, glass);
        HandmadeFincaAssets.CreateGabledRoof(
            building,
            "Deep-overhang Tile Roof",
            new Vector3(0f, 4.25f, 0f),
            size.x + 1.8f,
            size.y + 2f,
            1.75f,
            roof);
        GameObject sign = HandmadeFincaAssets.CreatePhysicalSign(
            building,
            new Vector3(0f, 3.25f, -halfDepth - 0.34f),
            signText,
            wood,
            new Color(0.94f, 0.83f, 0.57f));
        sign.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        return building;
    }

    private static string GetAuthoredBuildingAssetName(string buildingName)
    {
        return buildingName switch
        {
            "Curing Barn" => "CuringBarn",
            "Fermentation House" => "FermentationHouse",
            "Leaf Storage" => "LeafStorage",
            "Personal Workshop" => "PersonalWorkshop",
            "Aging Room" => "AgingRoom",
            "Finca Office" => "FincaOffice",
            "Founder Homestead" => "FounderHomestead",
            _ => throw new System.ArgumentOutOfRangeException(
                nameof(buildingName),
                buildingName,
                "No authored building shell is registered."),
        };
    }

    private static Material earthMaterialFallback(Material wall)
    {
        return CreateMaterial(
            $"{wall.name} Earthen Floor",
            new Color(0.42f, 0.30f, 0.20f),
            "Surfaces/red-clay-earth",
            Vector2.one,
            0.05f);
    }

    private static void CreateWindow(
        Transform building,
        float x,
        float z,
        float wallHeight,
        Material wood,
        Material glass)
    {
        CreateBox(
            "Deep Window",
            building,
            new Vector3(x, wallHeight * 0.58f, z),
            new Vector3(1.55f, 1.35f, 0.08f),
            glass);
        CreateBox(
            "Window Top Frame",
            building,
            new Vector3(x, (wallHeight * 0.58f) + 0.72f, z - 0.03f),
            new Vector3(1.85f, 0.13f, 0.12f),
            wood);
        CreateBox(
            "Window Bottom Frame",
            building,
            new Vector3(x, (wallHeight * 0.58f) - 0.72f, z - 0.03f),
            new Vector3(1.85f, 0.13f, 0.12f),
            wood);
        for (int side = -1; side <= 1; side += 2)
        {
            CreateBox(
                "Window Side Frame",
                building,
                new Vector3(x + (side * 0.84f), wallHeight * 0.58f, z - 0.03f),
                new Vector3(0.13f, 1.55f, 0.12f),
                wood);
        }
    }

    private static void CreatePergola(
        Transform parent,
        Material wood,
        Material roof)
    {
        for (int x = -1; x <= 1; x += 2)
        {
            for (int z = -1; z <= 1; z += 2)
            {
                CreateBox(
                    "Pergola Post",
                    parent,
                    new Vector3(x * 5f, 1.8f, z * 3.2f),
                    new Vector3(0.24f, 3.6f, 0.24f),
                    wood);
            }
        }

        for (int slat = -5; slat <= 5; slat++)
        {
            CreateBox(
                "Pergola Shade Slat",
                parent,
                new Vector3(slat * 0.92f, 3.62f, 0f),
                new Vector3(0.24f, 0.12f, 7.2f),
                slat % 2 == 0 ? wood : roof);
        }

        CreateBox(
            "Pergola Front Beam",
            parent,
            new Vector3(0f, 3.4f, -3.2f),
            new Vector3(10.4f, 0.28f, 0.24f),
            wood);
        CreateBox(
            "Pergola Back Beam",
            parent,
            new Vector3(0f, 3.4f, 3.2f),
            new Vector3(10.4f, 0.28f, 0.24f),
            wood);
    }

    private static void CreateCourtyardEdge(
        Transform parent,
        Vector3 center,
        float radiusX,
        float radiusZ,
        Material stone)
    {
        const int segments = 30;
        for (int index = 0; index < segments; index++)
        {
            float angle = (Mathf.PI * 2f * index) / segments;
            float irregularity = 1f + (Mathf.Sin((index * 2.1f) + 0.7f) * 0.025f);
            Vector3 position = center + new Vector3(
                Mathf.Cos(angle) * radiusX * irregularity,
                0.10f,
                Mathf.Sin(angle) * radiusZ * irregularity);
            GameObject edging = CreateBox(
                "Courtyard Edge Stone",
                parent,
                position,
                new Vector3(0.85f, 0.18f, 0.38f),
                stone);
            edging.transform.localRotation =
                Quaternion.Euler(0f, (-angle * Mathf.Rad2Deg) + 90f, 0f);
        }
    }

    private static void CreateCourtyardPlanter(
        Transform parent,
        Vector3 position,
        Material clay,
        Material stem,
        Material leaf)
    {
        GameObject planter = CreateBox(
            "Courtyard Clay Planter",
            parent,
            position + new Vector3(0f, 0.38f, 0f),
            new Vector3(1.4f, 0.76f, 1.4f),
            clay);
        planter.transform.localRotation = Quaternion.Euler(0f, 12f, 0f);
        HandmadeFincaAssets.CreateBananaPlant(
            parent,
            position + new Vector3(0f, 0.70f, 0f),
            0.42f,
            stem,
            leaf);
    }

    private static void CreateTableLegs(
        Transform parent,
        Vector3 basePosition,
        Vector2 topSize,
        Material wood)
    {
        for (int x = -1; x <= 1; x += 2)
        {
            for (int z = -1; z <= 1; z += 2)
            {
                CreateBox(
                    "Desk Leg",
                    parent,
                    basePosition + new Vector3(
                        x * ((topSize.x * 0.5f) - 0.3f),
                        0.43f,
                        z * ((topSize.y * 0.5f) - 0.25f)),
                    new Vector3(0.16f, 0.86f, 0.16f),
                    wood);
            }
        }
    }
}
}
