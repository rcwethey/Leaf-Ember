using System.Collections.Generic;
using UnityEngine;

namespace LeafEmber.Prototype
{

public static partial class HandmadeFincaAssets
{
    public static GameObject CreateGroundSurface(
        Transform parent,
        float width,
        float depth,
        Material material)
    {
        const int xSegments = 24;
        const int zSegments = 20;
        Vector3[] vertices = new Vector3[(xSegments + 1) * (zSegments + 1)];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[xSegments * zSegments * 6];

        for (int z = 0; z <= zSegments; z++)
        {
            for (int x = 0; x <= xSegments; x++)
            {
                int index = (z * (xSegments + 1)) + x;
                float normalizedX = x / (float)xSegments;
                float normalizedZ = z / (float)zSegments;
                float worldX = (normalizedX - 0.5f) * width;
                float worldZ = (normalizedZ - 0.5f) * depth;
                float edgeRise =
                    Mathf.Pow(Mathf.Abs(normalizedX - 0.5f) * 2f, 3f) * 0.35f +
                    Mathf.Pow(Mathf.Abs(normalizedZ - 0.5f) * 2f, 3f) * 0.2f;
                float undulation =
                    (Mathf.PerlinNoise((worldX + 180f) * 0.035f, (worldZ + 140f) * 0.035f) - 0.5f) *
                    0.18f;
                vertices[index] = new Vector3(worldX, -0.10f + edgeRise + undulation, worldZ);
                uvs[index] = new Vector2(worldX / 8f, worldZ / 8f);
            }
        }

        int triangleIndex = 0;
        for (int z = 0; z < zSegments; z++)
        {
            for (int x = 0; x < xSegments; x++)
            {
                int bottomLeft = (z * (xSegments + 1)) + x;
                int bottomRight = bottomLeft + 1;
                int topLeft = bottomLeft + xSegments + 1;
                int topRight = topLeft + 1;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = topLeft;
                triangles[triangleIndex++] = topRight;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = topRight;
                triangles[triangleIndex++] = bottomRight;
            }
        }

        GameObject ground = CreateMeshObject(
            "Finca Terrain",
            parent,
            Vector3.zero,
            vertices,
            triangles,
            uvs,
            material);
        MeshCollider collider = ground.AddComponent<MeshCollider>();
        collider.sharedMesh = ground.GetComponent<MeshFilter>().sharedMesh;
        return ground;
    }

    public static GameObject CreatePathRibbon(
        Transform parent,
        string name,
        Vector3[] centers,
        float width,
        Material material)
    {
        if (centers == null || centers.Length < 2)
        {
            return null;
        }

        Vector3[] vertices = new Vector3[centers.Length * 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[(centers.Length - 1) * 6];
        float distance = 0f;
        for (int index = 0; index < centers.Length; index++)
        {
            Vector3 direction;
            if (index == 0)
            {
                direction = centers[1] - centers[0];
            }
            else if (index == centers.Length - 1)
            {
                direction = centers[index] - centers[index - 1];
            }
            else
            {
                direction = centers[index + 1] - centers[index - 1];
            }

            direction.y = 0f;
            Vector3 right = Vector3.Cross(Vector3.up, direction.normalized) * (width * 0.5f);
            Vector3 center = centers[index] + (Vector3.up * 0.055f);
            vertices[index * 2] = center - right;
            vertices[(index * 2) + 1] = center + right;
            if (index > 0)
            {
                distance += Vector3.Distance(centers[index - 1], centers[index]);
            }

            float v = distance / 4f;
            uvs[index * 2] = new Vector2(0f, v);
            uvs[(index * 2) + 1] = new Vector2(1f, v);
        }

        for (int index = 0; index < centers.Length - 1; index++)
        {
            int source = index * 2;
            int target = index * 6;
            triangles[target] = source;
            triangles[target + 1] = source + 2;
            triangles[target + 2] = source + 3;
            triangles[target + 3] = source;
            triangles[target + 4] = source + 3;
            triangles[target + 5] = source + 1;
        }

        GameObject path = CreateMeshObject(
            name,
            parent,
            Vector3.zero,
            vertices,
            triangles,
            uvs,
            material);
        MeshCollider collider = path.AddComponent<MeshCollider>();
        collider.sharedMesh = path.GetComponent<MeshFilter>().sharedMesh;
        return path;
    }

    public static GameObject CreateCourtyardSurface(
        Transform parent,
        Vector3 position,
        float radiusX,
        float radiusZ,
        Material material)
    {
        const int segments = 40;
        Vector3[] vertices = new Vector3[segments + 1];
        Vector2[] uvs = new Vector2[segments + 1];
        int[] triangles = new int[segments * 3];
        vertices[0] = Vector3.zero;
        uvs[0] = new Vector2(0.5f, 0.5f);
        for (int index = 0; index < segments; index++)
        {
            float angle = (Mathf.PI * 2f * index) / segments;
            vertices[index + 1] =
                new Vector3(Mathf.Cos(angle) * radiusX, 0f, Mathf.Sin(angle) * radiusZ);
            uvs[index + 1] = new Vector2(
                0.5f + (Mathf.Cos(angle) * radiusX / 5f),
                0.5f + (Mathf.Sin(angle) * radiusZ / 5f));
            int next = ((index + 1) % segments) + 1;
            int target = index * 3;
            triangles[target] = 0;
            triangles[target + 1] = next;
            triangles[target + 2] = index + 1;
        }

        GameObject courtyard = CreateMeshObject(
            "Compacted Courtyard",
            parent,
            position + (Vector3.up * 0.045f),
            vertices,
            triangles,
            uvs,
            material);
        MeshCollider collider = courtyard.AddComponent<MeshCollider>();
        collider.sharedMesh = courtyard.GetComponent<MeshFilter>().sharedMesh;
        return courtyard;
    }

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
        CreateHill(parent, "Western Ridge", new Vector3(-92f, -2f, 28f), 46f, 24f, nearMaterial, 0.3f);
        CreateHill(parent, "Northern Ridge", new Vector3(-24f, -2f, 100f), 58f, 30f, farMaterial, 1.4f);
        CreateHill(parent, "Northeastern Ridge", new Vector3(68f, -2f, 86f), 48f, 26f, nearMaterial, 2.3f);
        CreateHill(parent, "Eastern Ridge", new Vector3(98f, -2f, 12f), 44f, 22f, farMaterial, 0.8f);
        CreateHill(parent, "Southern Ridge", new Vector3(42f, -2f, -98f), 56f, 25f, nearMaterial, 1.9f);
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
