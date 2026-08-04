using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LeafEmber.Prototype
{

public static partial class FincaWorldBuilder
{
    private sealed class AuthoredPalette
    {
        public Material ground;
        public Material earth;
        public Material plaster;
        public Material plasterAccent;
        public Material wood;
        public Material roof;
        public Material stone;
        public Material glass;
        public Material darkInterior;
        public Material livingLeaf;
        public Material livingLeafLight;
        public Material curedLeaf;
        public Material metal;
        public Material cloth;
        public Material backdrop;
        public Material treeTrunk;
        public Material treeLeaves;
        public Material treeBranches;
        public Material jacarandaTrunk;
        public Material jacarandaLeaves;
        public Material jacarandaBranches;
        public Material grass;
        public Material calathea;
        public Material crate;
        public Material bench;
        public Material ladder;

        public Material Resolve(string sourceName)
        {
            string normalized = (sourceName ?? string.Empty).ToLowerInvariant();
            if (normalized.Contains("le_plasteraccent")) return plasterAccent;
            if (normalized.Contains("le_plaster")) return plaster;
            if (normalized.Contains("le_rooftile")) return roof;
            if (normalized.Contains("le_stone")) return stone;
            if (normalized.Contains("le_glass")) return glass;
            if (normalized.Contains("le_darkinterior")) return darkInterior;
            if (normalized.Contains("le_livingleaflight")) return livingLeafLight;
            if (normalized.Contains("le_livingleaf")) return livingLeaf;
            if (normalized.Contains("le_curedleaf")) return curedLeaf;
            if (normalized.Contains("le_metal")) return metal;
            if (normalized.Contains("le_cloth")) return cloth;
            if (normalized.Contains("le_earth")) return earth;
            if (normalized.Contains("le_ground")) return ground;
            if (normalized.Contains("le_backdrop")) return backdrop;
            if (normalized.Contains("le_timber")) return wood;
            if (normalized.Contains("island_tree_02_leaves")) return treeLeaves;
            if (normalized.Contains("island_tree_02_branches")) return treeBranches;
            if (normalized.Contains("island_tree_02")) return treeTrunk;
            if (normalized.Contains("jacaranda_tree_leaves")) return jacarandaLeaves;
            if (normalized.Contains("jacaranda_tree_branches")) return jacarandaBranches;
            if (normalized.Contains("jacaranda_tree")) return jacarandaTrunk;
            if (normalized.Contains("grass_bermuda")) return grass;
            if (normalized.Contains("calathea_orbifolia")) return calathea;
            if (normalized.Contains("wooden_crate")) return crate;
            if (normalized.Contains("painted_wooden_bench")) return bench;
            if (normalized.Contains("wooden_ladder")) return ladder;
            return wood;
        }
    }

    private static AuthoredPalette authoredPalette;

    private static AuthoredPalette CreateAuthoredPalette()
    {
        const string pbr = "Environment/Materials/";
        const string thirdParty = "Environment/ThirdParty/PolyHaven/";
        return new AuthoredPalette
        {
            ground = CreateMaterial(
                "PBR Mixed Ground",
                Color.white,
                pbr + "grass_path_2/grass_path_2_diff_1k",
                new Vector2(14f, 11f),
                0.04f,
                pbr + "grass_path_2/grass_path_2_nor_gl_1k"),
            earth = CreateMaterial(
                "PBR Red Laterite",
                new Color(0.95f, 0.92f, 0.88f),
                pbr + "red_laterite_soil_stones/red_laterite_soil_stones_diff_1k",
                new Vector2(8f, 8f),
                0.03f,
                pbr + "red_laterite_soil_stones/red_laterite_soil_stones_nor_gl_1k"),
            plaster = CreateMaterial(
                "PBR Rough Lime Plaster",
                new Color(0.97f, 0.95f, 0.88f),
                pbr + "white_rough_plaster/white_rough_plaster_diff_1k",
                new Vector2(3.5f, 2.5f),
                0.06f,
                pbr + "white_rough_plaster/white_rough_plaster_nor_gl_1k"),
            plasterAccent = CreateMaterial(
                "Ochre Plaster Accent",
                new Color(0.48f, 0.24f, 0.12f),
                pbr + "white_rough_plaster/white_rough_plaster_diff_1k",
                new Vector2(3.5f, 2.5f),
                0.05f,
                pbr + "white_rough_plaster/white_rough_plaster_nor_gl_1k"),
            wood = CreateMaterial(
                "PBR Weathered Timber",
                new Color(0.90f, 0.86f, 0.78f),
                pbr + "wood_planks_dirt/wood_planks_dirt_diff_1k",
                new Vector2(4f, 4f),
                0.12f,
                pbr + "wood_planks_dirt/wood_planks_dirt_nor_gl_1k"),
            roof = CreateMaterial(
                "PBR Clay Roof",
                new Color(0.96f, 0.88f, 0.82f),
                pbr + "clay_roof_tiles_02/clay_roof_tiles_02_diff_1k",
                new Vector2(4.5f, 4.5f),
                0.08f,
                pbr + "clay_roof_tiles_02/clay_roof_tiles_02_nor_gl_1k"),
            stone = CreateMaterial("Dark Foundation Stone", new Color(0.29f, 0.26f, 0.21f)),
            glass = CreateMaterial("Recessed Window Glass", new Color(0.055f, 0.13f, 0.14f), null, null, 0.68f),
            darkInterior = CreateMaterial("Unlit Interior", new Color(0.028f, 0.022f, 0.017f), null, null, 0.02f),
            livingLeaf = CreateMaterial("Living Tobacco", new Color(0.18f, 0.42f, 0.10f), null, null, 0.05f, null, false, true),
            livingLeafLight = CreateMaterial("Sunlit Tobacco", new Color(0.34f, 0.58f, 0.16f), null, null, 0.05f, null, false, true),
            curedLeaf = CreateMaterial("Cured Tobacco", new Color(0.48f, 0.25f, 0.08f), null, null, 0.06f, null, false, true),
            metal = CreateMaterial("Dark Forged Metal", new Color(0.14f, 0.15f, 0.14f), null, null, 0.36f),
            cloth = CreateMaterial("Natural Fiber Cloth", new Color(0.62f, 0.50f, 0.30f), null, null, 0.04f),
            backdrop = CreateMaterial("Distant Vegetated Ridge", new Color(0.16f, 0.29f, 0.18f), null, null, 0.02f),
            treeTrunk = CreateMaterial(
                "CC0 Island Tree Trunk",
                Color.white,
                thirdParty + "island_tree_02/Textures/island_tree_02_diff_1k",
                Vector2.one,
                0.08f),
            treeLeaves = CreateMaterial(
                "CC0 Island Tree Leaves",
                Color.white,
                thirdParty + "island_tree_02/Textures/island_tree_02_leaves_diff_1k",
                Vector2.one,
                0.04f,
                null,
                true,
                true),
            treeBranches = CreateMaterial(
                "CC0 Island Tree Branches",
                Color.white,
                thirdParty + "island_tree_02/Textures/island_tree_02_branches_diff_1k",
                Vector2.one,
                0.07f,
                null,
                true,
                true),
            jacarandaTrunk = CreateMaterial(
                "CC0 Jacaranda Trunk",
                Color.white,
                thirdParty + "jacaranda_tree/Textures/jacaranda_tree_trunk_diff_1k",
                Vector2.one,
                0.08f,
                thirdParty + "jacaranda_tree/Textures/jacaranda_tree_trunk_nor_gl_1k"),
            jacarandaLeaves = CreateMaterial(
                "CC0 Jacaranda Leaves",
                new Color(0.88f, 1f, 0.82f),
                thirdParty + "jacaranda_tree/Textures/jacaranda_tree_leaves_diff_1k",
                Vector2.one,
                0.035f,
                thirdParty + "jacaranda_tree/Textures/jacaranda_tree_leaves_nor_gl_1k",
                true,
                true),
            jacarandaBranches = CreateMaterial(
                "CC0 Jacaranda Branches",
                Color.white,
                thirdParty + "jacaranda_tree/Textures/jacaranda_tree_branches_diff_1k",
                Vector2.one,
                0.06f,
                thirdParty + "jacaranda_tree/Textures/jacaranda_tree_branches_nor_gl_1k",
                true,
                true),
            grass = CreateMaterial(
                "CC0 Bermuda Grass",
                Color.white,
                thirdParty + "grass_bermuda_01/Textures/grass_bermuda_01_diff_1k",
                Vector2.one,
                0.03f,
                thirdParty + "grass_bermuda_01/Textures/grass_bermuda_01_nor_gl_1k",
                false,
                true),
            calathea = CreateMaterial(
                "CC0 Broadleaf Calathea",
                Color.white,
                thirdParty + "calathea_orbifolia_01/Textures/calathea_orbifolia_01_diff_1k",
                Vector2.one,
                0.04f,
                thirdParty + "calathea_orbifolia_01/Textures/calathea_orbifolia_01_nor_gl_1k",
                false,
                true),
            crate = CreateMaterial(
                "CC0 Wooden Crate",
                Color.white,
                thirdParty + "wooden_crate_01/Textures/wooden_crate_01_diff_1k",
                Vector2.one,
                0.16f,
                thirdParty + "wooden_crate_01/Textures/wooden_crate_01_nor_gl_1k"),
            bench = CreateMaterial(
                "CC0 Painted Wooden Bench",
                Color.white,
                thirdParty + "painted_wooden_bench/Textures/painted_wooden_bench_diff_1k",
                Vector2.one,
                0.14f,
                thirdParty + "painted_wooden_bench/Textures/painted_wooden_bench_nor_gl_1k"),
            ladder = CreateMaterial(
                "CC0 Wooden Ladder",
                Color.white,
                thirdParty + "wooden_ladder/Textures/wooden_ladder_diff_1k",
                Vector2.one,
                0.12f,
                thirdParty + "wooden_ladder/Textures/wooden_ladder_nor_gl_1k"),
        };
    }

    private static GameObject InstantiateAuthoredModel(
        string resourcePath,
        Transform parent,
        Vector3 localPosition,
        Quaternion localRotation,
        Vector3 localScale,
        string instanceName,
        bool foliage = false)
    {
        GameObject source = Resources.Load<GameObject>(resourcePath);
        if (source == null)
        {
            throw new InvalidOperationException($"Missing authored environment asset: {resourcePath}");
        }

        GameObject instance = new(instanceName);
        instance.transform.SetParent(parent, false);
        instance.transform.localPosition = localPosition;
        instance.transform.localRotation = localRotation;
        instance.transform.localScale = localScale;

        // Unity's FBX importer carries Blender-to-Unity axis conversion on the
        // model root (normally an X rotation near -90 degrees). Keep that
        // imported transform intact beneath our placement root. Replacing it
        // with Quaternion.identity turns every Z-up asset onto its side.
        GameObject geometry = UnityEngine.Object.Instantiate(source, instance.transform, false);
        geometry.name = $"{instanceName} Geometry";
        ApplyAuthoredMaterials(geometry, foliage);
        return instance;
    }

    private static void ApplyAuthoredMaterials(GameObject instance, bool foliage)
    {
        Renderer[] renderers = instance.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer renderer in renderers)
        {
            Material[] sourceMaterials = renderer.sharedMaterials;
            Material[] mappedMaterials = new Material[sourceMaterials.Length];
            for (int index = 0; index < sourceMaterials.Length; index++)
            {
                mappedMaterials[index] = authoredPalette.Resolve(sourceMaterials[index]?.name);
            }

            renderer.sharedMaterials = mappedMaterials;
            renderer.allowOcclusionWhenDynamic = true;
            renderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
            if (foliage)
            {
                renderer.shadowCastingMode = ShadowCastingMode.Off;
            }
        }
    }

    private static GameObject InstantiateAuthoredLod(
        string lod0Path,
        string lod1Path,
        Transform parent,
        Vector3 localPosition,
        Quaternion localRotation,
        Vector3 localScale,
        string instanceName,
        bool foliage,
        float lod0Threshold,
        float lod1Threshold)
    {
        GameObject root = new(instanceName);
        root.transform.SetParent(parent, false);
        root.transform.localPosition = localPosition;
        root.transform.localRotation = localRotation;
        root.transform.localScale = localScale;
        GameObject lod0 = InstantiateAuthoredModel(
            lod0Path,
            root.transform,
            Vector3.zero,
            Quaternion.identity,
            Vector3.one,
            "LOD0",
            foliage);
        GameObject lod1 = InstantiateAuthoredModel(
            lod1Path,
            root.transform,
            Vector3.zero,
            Quaternion.identity,
            Vector3.one,
            "LOD1",
            foliage);
        LODGroup group = root.AddComponent<LODGroup>();
        group.fadeMode = LODFadeMode.CrossFade;
        group.animateCrossFading = false;
        group.SetLODs(
            new[]
            {
                new LOD(lod0Threshold, lod0.GetComponentsInChildren<Renderer>(true)),
                new LOD(lod1Threshold, lod1.GetComponentsInChildren<Renderer>(true)),
            });
        group.RecalculateBounds();
        return root;
    }

    private static GameObject InstantiateBuildingShell(
        string assetName,
        Transform building,
        Vector2 footprint)
    {
        GameObject shell = InstantiateAuthoredLod(
            $"Environment/Authored/Architecture/{assetName}_LOD0",
            $"Environment/Authored/Architecture/{assetName}_LOD1",
            building,
            Vector3.zero,
            Quaternion.Euler(0f, 180f, 0f),
            Vector3.one,
            $"{assetName} Authored Shell",
            false,
            0.20f,
            0.055f);
        AddBuildingCollision(building.gameObject, footprint);
        return shell;
    }

    private static void AddBuildingCollision(GameObject building, Vector2 footprint)
    {
        const float height = 3.7f;
        const float thickness = 0.28f;
        const float doorWidth = 2.6f;
        float halfWidth = footprint.x * 0.5f;
        float halfDepth = footprint.y * 0.5f;
        float frontSegmentWidth = (footprint.x - doorWidth) * 0.5f;
        AddBoxCollider(building, new Vector3(0f, height * 0.5f, halfDepth), new Vector3(footprint.x, height, thickness));
        AddBoxCollider(building, new Vector3(-halfWidth, height * 0.5f, 0f), new Vector3(thickness, height, footprint.y));
        AddBoxCollider(building, new Vector3(halfWidth, height * 0.5f, 0f), new Vector3(thickness, height, footprint.y));
        for (int side = -1; side <= 1; side += 2)
        {
            AddBoxCollider(
                building,
                new Vector3(side * (doorWidth * 0.5f + frontSegmentWidth * 0.5f), height * 0.5f, -halfDepth),
                new Vector3(frontSegmentWidth, height, thickness));
        }
    }

    private static BoxCollider AddBoxCollider(GameObject owner, Vector3 center, Vector3 size)
    {
        BoxCollider collider = owner.AddComponent<BoxCollider>();
        collider.center = center;
        collider.size = size;
        return collider;
    }

    private static GameObject InstantiateTerrain(Transform parent)
    {
        GameObject terrain = InstantiateAuthoredModel(
            "Environment/Authored/Landscape/FincaTerrain",
            parent,
            Vector3.zero,
            Quaternion.identity,
            Vector3.one,
            "Authored Finca Terrain");
        MeshFilter meshFilter = terrain.GetComponentInChildren<MeshFilter>(true);
        MeshCollider collider = meshFilter.gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = meshFilter.sharedMesh;
        return terrain;
    }

    private static GameObject InstantiateFieldPlot(Transform parent)
    {
        GameObject field = InstantiateAuthoredModel(
            "Environment/Authored/Landscape/AuthoredFieldPlot",
            parent,
            Vector3.zero,
            Quaternion.identity,
            Vector3.one,
            "Authored Laterite Field Plot");
        MeshFilter meshFilter = field.GetComponentInChildren<MeshFilter>(true);
        MeshCollider collider = meshFilter.gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = meshFilter.sharedMesh;
        return field;
    }

    private static void InstantiateBackdrop(Transform parent)
    {
        InstantiateAuthoredModel(
            "Environment/Authored/Landscape/FincaBackdrop",
            parent,
            Vector3.zero,
            Quaternion.identity,
            Vector3.one,
            "Authored Distant Ridges");
    }

    private static void InstantiateFenceLine(
        Transform parent,
        Vector3 start,
        Vector3 end)
    {
        Vector3 delta = end - start;
        float length = delta.magnitude;
        Vector3 direction = delta.normalized;
        int sections = Mathf.Max(1, Mathf.CeilToInt(length / 10f));
        for (int index = 0; index < sections; index++)
        {
            float startT = index / (float)sections;
            float endT = (index + 1) / (float)sections;
            Vector3 sectionStart = Vector3.Lerp(start, end, startT);
            Vector3 sectionEnd = Vector3.Lerp(start, end, endT);
            Vector3 center = (sectionStart + sectionEnd) * 0.5f;
            float sectionLength = Vector3.Distance(sectionStart, sectionEnd);
            GameObject section = InstantiateAuthoredModel(
                "Environment/Authored/Landscape/BoundaryFenceSection",
                parent,
                center,
                Quaternion.FromToRotation(Vector3.right, direction),
                new Vector3(sectionLength / 10f, 1f, 1f),
                "Authored Boundary Fence Section");
            AddBoxCollider(
                section,
                new Vector3(0f, 0.86f, 0f),
                new Vector3(10f, 1.72f, 0.24f));
        }
    }

    private static GameObject InstantiateEntryGate(
        Transform parent,
        Vector3 localPosition)
    {
        return InstantiateAuthoredModel(
            "Environment/Authored/Landscape/FincaEntryGate",
            parent,
            localPosition,
            Quaternion.identity,
            Vector3.one,
            "Finca Entry Gate");
    }

    private static GameObject InstantiateProductionAsset(
        string assetName,
        Transform parent,
        Vector3 localPosition,
        Quaternion localRotation,
        Vector3 localScale,
        Vector3 colliderCenter,
        Vector3 colliderSize)
    {
        GameObject instance = InstantiateAuthoredModel(
            $"Environment/Authored/Production/{assetName}",
            parent,
            localPosition,
            localRotation,
            localScale,
            assetName);
        AddBoxCollider(instance, colliderCenter, colliderSize);
        return instance;
    }

    private static GameObject InstantiateProductionLod(
        string assetName,
        Transform parent,
        Vector3 localPosition,
        Quaternion localRotation,
        Vector3 localScale,
        Vector3 colliderCenter,
        Vector3 colliderSize)
    {
        GameObject instance = InstantiateAuthoredLod(
            $"Environment/Authored/Production/{assetName}_LOD0",
            $"Environment/Authored/Production/{assetName}_LOD1",
            parent,
            localPosition,
            localRotation,
            localScale,
            assetName,
            false,
            0.16f,
            0.045f);
        AddBoxCollider(instance, colliderCenter, colliderSize);
        return instance;
    }

    private static GameObject InstantiateAuthoredTobacco(
        Transform parent,
        Vector3 localPosition,
        float yaw,
        float scale)
    {
        return InstantiateAuthoredLod(
            "Environment/Authored/Vegetation/TobaccoPlant_LOD0",
            "Environment/Authored/Vegetation/TobaccoPlant_LOD1",
            parent,
            localPosition,
            Quaternion.Euler(0f, yaw, 0f),
            Vector3.one * scale,
            "Authored Tobacco Plant",
            true,
            0.12f,
            0.025f);
    }

    private static GameObject InstantiateIslandTree(
        Transform parent,
        Vector3 localPosition,
        float yaw,
        float scale)
    {
        return InstantiateAuthoredLod(
            "Environment/ThirdParty/PolyHaven/island_tree_02/island_tree_02_LOD0",
            "Environment/ThirdParty/PolyHaven/island_tree_02/island_tree_02_LOD1",
            parent,
            localPosition,
            Quaternion.Euler(0f, yaw, 0f),
            Vector3.one * scale,
            "CC0 Tropical Shade Tree",
            true,
            0.13f,
            0.028f);
    }

    private static GameObject InstantiateJacarandaTree(
        Transform parent,
        Vector3 localPosition,
        float yaw,
        float scale)
    {
        return InstantiateAuthoredLod(
            "Environment/ThirdParty/PolyHaven/jacaranda_tree/jacaranda_tree_LOD0",
            "Environment/ThirdParty/PolyHaven/jacaranda_tree/jacaranda_tree_LOD1",
            parent,
            localPosition,
            Quaternion.Euler(0f, yaw, 0f),
            Vector3.one * scale,
            "CC0 Jacaranda Shade Tree",
            true,
            0.12f,
            0.025f);
    }

    private static GameObject InstantiateGroundCluster(
        Transform parent,
        Vector3 localPosition,
        float yaw,
        float scale)
    {
        return InstantiateAuthoredLod(
            "Environment/ThirdParty/PolyHaven/grass_bermuda_01/grass_bermuda_01_LOD0",
            "Environment/ThirdParty/PolyHaven/grass_bermuda_01/grass_bermuda_01_LOD1",
            parent,
            localPosition,
            Quaternion.Euler(0f, yaw, 0f),
            Vector3.one * scale,
            "CC0 Bermuda Ground Cluster",
            true,
            0.075f,
            0.018f);
    }

    private static GameObject InstantiateBroadleafCluster(
        Transform parent,
        Vector3 localPosition,
        float yaw,
        float scale)
    {
        return InstantiateAuthoredLod(
            "Environment/ThirdParty/PolyHaven/calathea_orbifolia_01/calathea_orbifolia_01_LOD0",
            "Environment/ThirdParty/PolyHaven/calathea_orbifolia_01/calathea_orbifolia_01_LOD1",
            parent,
            localPosition,
            Quaternion.Euler(0f, yaw, 0f),
            Vector3.one * scale,
            "CC0 Broadleaf Planting",
            true,
            0.09f,
            0.022f);
    }

    private static GameObject InstantiatePolyHavenProp(
        string assetId,
        Transform parent,
        Vector3 localPosition,
        Quaternion localRotation,
        Vector3 localScale,
        string instanceName)
    {
        return InstantiateAuthoredModel(
            $"Environment/ThirdParty/PolyHaven/{assetId}/{assetId}",
            parent,
            localPosition,
            localRotation,
            localScale,
            instanceName);
    }

    private static void ConfigureAuthoredSky()
    {
        Texture skyTexture = Resources.Load<Texture>("Environment/Lighting/rural_landscape_1k");
        Shader skyShader = Shader.Find("Skybox/Panoramic");
        if (skyTexture == null || skyShader == null)
        {
            return;
        }

        Material sky = new(skyShader)
        {
            name = "CC0 Rural Landscape Sky",
        };
        sky.SetTexture("_MainTex", skyTexture);
        sky.SetFloat("_Exposure", 0.72f);
        sky.SetFloat("_Rotation", 116f);
        RenderSettings.skybox = sky;
        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.ambientIntensity = 0.66f;
        DynamicGI.UpdateEnvironment();
    }
}
}
