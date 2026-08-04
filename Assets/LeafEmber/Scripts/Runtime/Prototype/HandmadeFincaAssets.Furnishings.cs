using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LeafEmber.Prototype
{

public static partial class HandmadeFincaAssets
{
    public static void CreateFenceLine(
        Transform parent,
        Vector3 start,
        Vector3 end,
        Material timber)
    {
        GameObject fence = new("Hand-built Boundary Fence");
        fence.transform.SetParent(parent, false);
        Vector3 delta = end - start;
        float length = delta.magnitude;
        Vector3 direction = delta.normalized;
        int sections = Mathf.Max(1, Mathf.CeilToInt(length / 3.2f));
        for (int index = 0; index <= sections; index++)
        {
            Vector3 position = Vector3.Lerp(start, end, index / (float)sections);
            CreatePrimitive(
                PrimitiveType.Cylinder,
                "Fence Post",
                fence.transform,
                position + new Vector3(0f, 0.8f, 0f),
                new Vector3(0.075f, 0.8f, 0.075f),
                timber);
        }

        for (int rail = 0; rail < 2; rail++)
        {
            GameObject crossRail = CreatePrimitive(
                PrimitiveType.Cube,
                "Fence Rail",
                fence.transform,
                ((start + end) * 0.5f) + new Vector3(0f, 0.62f + (rail * 0.58f), 0f),
                new Vector3(length, 0.095f, 0.095f),
                timber,
                false);
            crossRail.transform.localRotation = Quaternion.FromToRotation(Vector3.right, direction);
        }
    }

    public static void CreateEntryGate(
        Transform parent,
        Vector3 position,
        Material timber,
        Material metal)
    {
        GameObject gate = new("Finca Entry Gate");
        gate.transform.SetParent(parent, false);
        gate.transform.localPosition = position;
        for (int side = -1; side <= 1; side += 2)
        {
            CreatePrimitive(
                PrimitiveType.Cube,
                "Gate Pier",
                gate.transform,
                new Vector3(side * 2.25f, 1.5f, 0f),
                new Vector3(0.34f, 3f, 0.34f),
                timber);
        }

        CreatePrimitive(
            PrimitiveType.Cube,
            "Gate Header",
            gate.transform,
            new Vector3(0f, 2.8f, 0f),
            new Vector3(4.85f, 0.28f, 0.32f),
            timber);
        CreatePrimitive(
            PrimitiveType.Cube,
            "Forged Gate Bar",
            gate.transform,
            new Vector3(0f, 1.25f, 0f),
            new Vector3(4f, 0.06f, 0.06f),
            metal,
            false);

        CreatePhysicalSign(
            gate.transform,
            new Vector3(0f, 2.8f, -0.20f),
            "LEAF & EMBER\nFINCA",
            timber,
            new Color(0.95f, 0.84f, 0.58f));
    }

    public static GameObject CreatePhysicalSign(
        Transform parent,
        Vector3 position,
        string label,
        Material timber,
        Color textColor)
    {
        GameObject sign = new($"{label} Sign");
        sign.transform.SetParent(parent, false);
        sign.transform.localPosition = position;
        CreatePrimitive(
            PrimitiveType.Cube,
            "Painted Timber Sign",
            sign.transform,
            new Vector3(0f, 0f, 0f),
            new Vector3(2.8f, 0.78f, 0.10f),
            timber);

        GameObject textObject = new("Lettering");
        textObject.transform.SetParent(sign.transform, false);
        textObject.transform.localPosition = new Vector3(0f, 0f, -0.061f);
        textObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        TextMesh text = textObject.AddComponent<TextMesh>();
        text.text = label;
        text.anchor = TextAnchor.MiddleCenter;
        text.alignment = TextAlignment.Center;
        text.fontSize = 52;
        text.characterSize = 0.055f;
        text.color = textColor;
        return sign;
    }

    public static void CreateCrateStack(
        Transform parent,
        Vector3 position,
        Material timber,
        int count)
    {
        GameObject stack = new("Workshop Crate Stack");
        stack.transform.SetParent(parent, false);
        stack.transform.localPosition = position;
        for (int index = 0; index < count; index++)
        {
            float y = 0.32f + (index * 0.62f);
            float x = (index % 2 == 0 ? -0.08f : 0.10f);
            GameObject crate = CreatePrimitive(
                PrimitiveType.Cube,
                "Ventilated Leaf Crate",
                stack.transform,
                new Vector3(x, y, 0f),
                new Vector3(1.25f, 0.56f, 0.9f),
                timber);
            crate.transform.localRotation = Quaternion.Euler(0f, index % 2 == 0 ? 2f : -3f, 0f);
            for (int slat = -1; slat <= 1; slat++)
            {
                CreatePrimitive(
                    PrimitiveType.Cube,
                    "Crate Slat",
                    crate.transform,
                    new Vector3(slat * 0.28f, 0f, -0.51f),
                    new Vector3(0.12f, 0.42f, 0.035f),
                    timber,
                    false);
            }
        }
    }

    public static void CreateSackStack(
        Transform parent,
        Vector3 position,
        Material cloth,
        int count)
    {
        GameObject stack = new("Natural-fiber Sack Stack");
        stack.transform.SetParent(parent, false);
        stack.transform.localPosition = position;
        for (int index = 0; index < count; index++)
        {
            GameObject sack = CreatePrimitive(
                PrimitiveType.Sphere,
                "Filled Work Sack",
                stack.transform,
                new Vector3(
                    ((index % 2) - 0.5f) * 0.7f,
                    0.3f + ((index / 2) * 0.48f),
                    0f),
                new Vector3(0.75f, 0.42f, 0.45f),
                cloth,
                false);
            sack.transform.localRotation = Quaternion.Euler(0f, 0f, index % 2 == 0 ? 5f : -6f);
        }
    }

    public static void CreateBarrelCluster(
        Transform parent,
        Vector3 position,
        Material timber,
        Material metal)
    {
        GameObject cluster = new("Water Barrel Cluster");
        cluster.transform.SetParent(parent, false);
        cluster.transform.localPosition = position;
        for (int index = 0; index < 2; index++)
        {
            GameObject barrel = new($"Rain Barrel {index + 1}");
            barrel.transform.SetParent(cluster.transform, false);
            barrel.transform.localPosition = new Vector3(index * 1.05f, 0f, index * 0.18f);
            CreatePrimitive(
                PrimitiveType.Cylinder,
                "Wooden Staves",
                barrel.transform,
                new Vector3(0f, 0.65f, 0f),
                new Vector3(0.52f, 0.65f, 0.52f),
                timber);
            for (int band = 0; band < 2; band++)
            {
                CreatePrimitive(
                    PrimitiveType.Cylinder,
                    "Metal Hoop",
                    barrel.transform,
                    new Vector3(0f, 0.35f + (band * 0.62f), 0f),
                    new Vector3(0.535f, 0.025f, 0.535f),
                    metal,
                    false);
            }
        }
    }

    public static void CreatePorchBench(
        Transform parent,
        Vector3 position,
        float yaw,
        Material timber)
    {
        GameObject bench = new("Finca Porch Bench");
        bench.transform.SetParent(parent, false);
        bench.transform.localPosition = position;
        bench.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
        CreatePrimitive(
            PrimitiveType.Cube,
            "Bench Seat",
            bench.transform,
            new Vector3(0f, 0.58f, 0f),
            new Vector3(2.3f, 0.14f, 0.52f),
            timber);
        CreatePrimitive(
            PrimitiveType.Cube,
            "Bench Back",
            bench.transform,
            new Vector3(0f, 1.02f, 0.22f),
            new Vector3(2.3f, 0.72f, 0.12f),
            timber,
            false);
        for (int side = -1; side <= 1; side += 2)
        {
            CreatePrimitive(
                PrimitiveType.Cube,
                "Bench Leg",
                bench.transform,
                new Vector3(side * 0.86f, 0.28f, 0f),
                new Vector3(0.13f, 0.56f, 0.36f),
                timber,
                false);
        }
    }

    public static void CreateWarmLantern(
        Transform parent,
        Vector3 position,
        Material metal,
        Material glass)
    {
        GameObject lantern = new("Warm Workshop Lantern");
        lantern.transform.SetParent(parent, false);
        lantern.transform.localPosition = position;
        CreatePrimitive(
            PrimitiveType.Cylinder,
            "Lantern Frame",
            lantern.transform,
            Vector3.zero,
            new Vector3(0.13f, 0.26f, 0.13f),
            metal,
            false);
        CreatePrimitive(
            PrimitiveType.Sphere,
            "Lantern Glass",
            lantern.transform,
            Vector3.zero,
            new Vector3(0.18f, 0.22f, 0.18f),
            glass,
            false);
        Light light = lantern.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1f, 0.66f, 0.34f);
        light.range = 4.5f;
        light.intensity = 0.75f;
        light.shadows = LightShadows.None;
    }

    public static void CreateBananaPlant(
        Transform parent,
        Vector3 position,
        float scale,
        Material stem,
        Material leaf)
    {
        GameObject plant = new("Broadleaf Windbreak Plant");
        plant.transform.SetParent(parent, false);
        plant.transform.localPosition = position;
        CreatePrimitive(
            PrimitiveType.Cylinder,
            "Layered Pseudostem",
            plant.transform,
            new Vector3(0f, 1.35f * scale, 0f),
            new Vector3(0.18f * scale, 1.35f * scale, 0.18f * scale),
            stem,
            false);

        List<Vector3> leafVertices = new();
        List<Vector2> leafUvs = new();
        List<int> leafTriangles = new();
        for (int index = 0; index < 7; index++)
        {
            Matrix4x4 leafTransform = Matrix4x4.TRS(
                new Vector3(0f, 2.55f * scale, 0f),
                Quaternion.Euler(
                    -28f + ((index % 3) * 8f),
                    index * (360f / 7f),
                    0f),
                new Vector3(0.72f * scale, 1.75f * scale, 1.75f * scale));
            AppendLeafGeometry(leafTransform, leafVertices, leafUvs, leafTriangles);
        }

        GameObject combinedLeaves = CreateMeshObject(
            "Combined Windbreak Leaves",
            plant.transform,
            Vector3.zero,
            leafVertices.ToArray(),
            leafTriangles.ToArray(),
            leafUvs.ToArray(),
            leaf);
        combinedLeaves.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
    }
}
}
