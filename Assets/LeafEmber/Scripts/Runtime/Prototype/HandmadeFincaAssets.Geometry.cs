using System.Collections.Generic;
using UnityEngine;

namespace LeafEmber.Prototype
{

public static partial class HandmadeFincaAssets
{
    public static void ConfigureAtmosphere()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = new Color(0.62f, 0.70f, 0.68f);
        RenderSettings.fogStartDistance = 48f;
        RenderSettings.fogEndDistance = 125f;
        RenderSettings.ambientIntensity = 0.82f;

        Light[] lights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light light in lights)
        {
            if (light.type != LightType.Directional)
            {
                continue;
            }

            light.color = new Color(1f, 0.86f, 0.66f);
            light.intensity = 1.65f;
            light.shadowStrength = 0.78f;
            light.transform.rotation = Quaternion.Euler(42f, -32f, 0f);
            break;
        }
    }

    public static void CreateDistantHills(
        Transform parent,
        Material nearMaterial,
        Material farMaterial)
    {
        CreateHill(parent, "Western Ridge", new Vector3(-38f, -1f, 25f), 24f, 13f, nearMaterial, 0.3f);
        CreateHill(parent, "Northern Ridge", new Vector3(-10f, -1f, 52f), 35f, 18f, farMaterial, 1.4f);
        CreateHill(parent, "Northeastern Ridge", new Vector3(32f, -1f, 40f), 28f, 15f, nearMaterial, 2.3f);
        CreateHill(parent, "Eastern Ridge", new Vector3(50f, -1f, 4f), 22f, 11f, farMaterial, 0.8f);
        CreateHill(parent, "Southern Ridge", new Vector3(18f, -1f, -45f), 34f, 14f, nearMaterial, 1.9f);
    }

    public static void CreateGabledRoof(
        Transform parent,
        string name,
        Vector3 position,
        float width,
        float depth,
        float rise,
        Material material)
    {
        float halfWidth = width * 0.5f;
        float halfDepth = depth * 0.5f;
        Vector3[] vertices =
        {
            new(-halfWidth, 0f, -halfDepth),
            new(halfWidth, 0f, -halfDepth),
            new(-halfWidth, 0f, halfDepth),
            new(halfWidth, 0f, halfDepth),
            new(0f, rise, -halfDepth),
            new(0f, rise, halfDepth),
        };
        int[] triangles =
        {
            0, 2, 5, 0, 5, 4,
            1, 4, 5, 1, 5, 3,
            0, 4, 1,
            2, 3, 5,
            0, 1, 3, 0, 3, 2,
        };
        Vector2[] uvs =
        {
            new(0f, 0f), new(1f, 0f), new(0f, 1f),
            new(1f, 1f), new(0.5f, 0.35f), new(0.5f, 0.65f),
        };

        GameObject roof = CreateMeshObject(name, parent, position, vertices, triangles, uvs, material);
        MeshCollider collider = roof.AddComponent<MeshCollider>();
        collider.sharedMesh = roof.GetComponent<MeshFilter>().sharedMesh;
    }

    public static void CreateHill(
        Transform parent,
        string name,
        Vector3 position,
        float radius,
        float height,
        Material material,
        float phase)
    {
        const int segments = 24;
        List<Vector3> vertices = new() { new Vector3(0f, height, 0f) };
        for (int ring = 1; ring <= 2; ring++)
        {
            float ringRadius = radius * (ring == 1 ? 0.48f : 1f);
            float ringHeight = height * (ring == 1 ? 0.58f : 0f);
            for (int index = 0; index < segments; index++)
            {
                float angle = ((Mathf.PI * 2f) * index / segments) + phase;
                float irregularity = 1f + (Mathf.Sin((index * 2.7f) + phase) * 0.1f);
                vertices.Add(new Vector3(
                    Mathf.Cos(angle) * ringRadius * irregularity,
                    ringHeight + (Mathf.Sin((index * 1.9f) + phase) * height * 0.04f),
                    Mathf.Sin(angle) * ringRadius * irregularity));
            }
        }

        List<int> triangles = new();
        for (int index = 0; index < segments; index++)
        {
            int next = (index + 1) % segments;
            triangles.Add(0);
            triangles.Add(1 + index);
            triangles.Add(1 + next);

            int inner = 1 + index;
            int innerNext = 1 + next;
            int outer = 1 + segments + index;
            int outerNext = 1 + segments + next;
            triangles.Add(inner);
            triangles.Add(outer);
            triangles.Add(outerNext);
            triangles.Add(inner);
            triangles.Add(outerNext);
            triangles.Add(innerNext);
        }

        GameObject hill = CreateMeshObject(
            name,
            parent,
            position,
            vertices.ToArray(),
            triangles.ToArray(),
            new Vector2[vertices.Count],
            material);
        hill.transform.localScale = new Vector3(1f, 1f, 0.72f);
    }

    private static GameObject CreateMeshObject(
        string name,
        Transform parent,
        Vector3 position,
        Vector3[] vertices,
        int[] triangles,
        Vector2[] uvs,
        Material material)
    {
        Mesh mesh = new() { name = $"{name} Handmade Mesh" };
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GameObject result = new(name);
        result.transform.SetParent(parent, false);
        result.transform.localPosition = position;
        result.AddComponent<MeshFilter>().sharedMesh = mesh;
        result.AddComponent<MeshRenderer>().sharedMaterial = material;
        return result;
    }

    private static GameObject CreatePrimitive(
        PrimitiveType type,
        string name,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Material material,
        bool keepCollider = true)
    {
        GameObject result = GameObject.CreatePrimitive(type);
        result.name = name;
        result.transform.SetParent(parent, false);
        result.transform.localPosition = position;
        result.transform.localScale = scale;
        result.GetComponent<MeshRenderer>().sharedMaterial = material;
        if (!keepCollider)
        {
            Object.Destroy(result.GetComponent<Collider>());
        }

        return result;
    }
}
}
