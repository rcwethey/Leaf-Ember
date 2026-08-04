using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LeafEmber.Prototype
{

public static partial class HandmadeFincaAssets
{
    public static void CreateTobaccoPlant(
        Transform parent,
        Vector3 position,
        float height,
        Material stemMaterial,
        Material leafMaterial,
        float phase)
    {
        GameObject plant = new("Tobacco Plant");
        plant.transform.SetParent(parent, false);
        plant.transform.localPosition = position;

        GameObject stalk = CreatePrimitive(
            PrimitiveType.Cylinder,
            "Stalk",
            plant.transform,
            new Vector3(0f, height * 0.45f, 0f),
            new Vector3(0.045f, height * 0.45f, 0.045f),
            stemMaterial,
            false);
        MeshRenderer stalkRenderer = stalk.GetComponent<MeshRenderer>();
        stalkRenderer.shadowCastingMode = ShadowCastingMode.Off;

        List<Vector3> vertices = new();
        List<Vector2> uvs = new();
        List<int> triangles = new();
        for (int level = 0; level < 5; level++)
        {
            float vertical = height * (0.2f + (level * 0.14f));
            float leafLength = height * (0.53f - (level * 0.045f));
            for (int side = 0; side < 2; side++)
            {
                float yaw = (phase * Mathf.Rad2Deg) + (level * 71f) + (side * 180f);
                Matrix4x4 leafTransform = Matrix4x4.TRS(
                    new Vector3(0f, vertical, 0f),
                    Quaternion.Euler(
                        8f + (level * 2f),
                        yaw,
                        side == 0 ? -5f : 5f),
                    new Vector3(leafLength * 0.42f, leafLength, leafLength));
                AppendLeafGeometry(leafTransform, vertices, uvs, triangles);
            }
        }

        GameObject combinedLeaves = CreateMeshObject(
            "Combined Broad Leaves",
            plant.transform,
            Vector3.zero,
            vertices.ToArray(),
            triangles.ToArray(),
            uvs.ToArray(),
            leafMaterial);
        MeshRenderer leafRenderer = combinedLeaves.GetComponent<MeshRenderer>();
        leafRenderer.shadowCastingMode = ShadowCastingMode.Off;
        leafRenderer.receiveShadows = true;
    }

    private static void AppendLeafGeometry(
        Matrix4x4 leafTransform,
        List<Vector3> vertices,
        List<Vector2> uvs,
        List<int> triangles)
    {
        Vector3[] face =
        {
            new(0f, 0f, 0f),
            new(-0.20f, 0.015f, 0.22f),
            new(-0.43f, 0.045f, 0.55f),
            new(-0.28f, 0.025f, 0.88f),
            new(0f, 0f, 1.15f),
            new(0.28f, 0.025f, 0.88f),
            new(0.43f, 0.045f, 0.55f),
            new(0.20f, 0.015f, 0.22f),
        };
        int[] front =
        {
            0, 1, 2,
            0, 2, 3,
            0, 3, 4,
            0, 4, 5,
            0, 5, 6,
            0, 6, 7,
        };
        int firstVertex = vertices.Count;
        for (int index = 0; index < face.Length; index++)
        {
            Vector3 transformed = leafTransform.MultiplyPoint3x4(face[index]);
            vertices.Add(transformed);
            vertices.Add(transformed);
            float u = 0.5f + face[index].x;
            float v = face[index].z / 1.15f;
            uvs.Add(new Vector2(u, v));
            uvs.Add(new Vector2(u, v));
        }

        for (int index = 0; index < front.Length; index += 3)
        {
            triangles.Add(firstVertex + (front[index] * 2));
            triangles.Add(firstVertex + (front[index + 1] * 2));
            triangles.Add(firstVertex + (front[index + 2] * 2));

            triangles.Add(firstVertex + (front[index] * 2) + 1);
            triangles.Add(firstVertex + (front[index + 2] * 2) + 1);
            triangles.Add(firstVertex + (front[index + 1] * 2) + 1);
        }
    }

    public static GameObject CreateCuringRack(
        Transform parent,
        Vector3 position,
        float width,
        float depth,
        Material timber,
        Material curedLeaf)
    {
        GameObject rack = new("Handmade Curing Rack");
        rack.transform.SetParent(parent, false);
        rack.transform.localPosition = position;

        float halfWidth = width * 0.5f;
        float halfDepth = depth * 0.5f;
        for (int x = -1; x <= 1; x += 2)
        {
            for (int z = -1; z <= 1; z += 2)
            {
                CreatePrimitive(
                    PrimitiveType.Cube,
                    "Rack Post",
                    rack.transform,
                    new Vector3(x * halfWidth, 1.25f, z * halfDepth),
                    new Vector3(0.11f, 2.5f, 0.11f),
                    timber,
                    false);
            }
        }

        List<Vector3> leafVertices = new();
        List<Vector2> leafUvs = new();
        List<int> leafTriangles = new();
        for (int rail = -1; rail <= 1; rail++)
        {
            float z = rail * halfDepth;
            CreatePrimitive(
                PrimitiveType.Cube,
                "Curing Pole",
                rack.transform,
                new Vector3(0f, 2.25f, z),
                new Vector3(width + 0.3f, 0.1f, 0.1f),
                timber,
                false);

            for (int leafIndex = -3; leafIndex <= 3; leafIndex++)
            {
                Matrix4x4 leafTransform = Matrix4x4.TRS(
                    new Vector3(leafIndex * (width / 8f), 2.2f, z),
                    Quaternion.Euler(90f, 2f * leafIndex, 0f),
                    new Vector3(0.32f, 0.78f, 0.78f));
                AppendLeafGeometry(leafTransform, leafVertices, leafUvs, leafTriangles);
            }
        }

        GameObject hangingLeaves = CreateMeshObject(
            "Combined Hanging Cured Leaves",
            rack.transform,
            Vector3.zero,
            leafVertices.ToArray(),
            leafTriangles.ToArray(),
            leafUvs.ToArray(),
            curedLeaf);
        hangingLeaves.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        return rack;
    }

    public static void CreateLeafBale(
        Transform parent,
        Vector3 position,
        Vector3 size,
        Material curedLeaf,
        Material strap)
    {
        GameObject bale = new("Hand-wrapped Leaf Bale");
        bale.transform.SetParent(parent, false);
        bale.transform.localPosition = position;
        CreatePrimitive(
            PrimitiveType.Cube,
            "Compressed Leaves",
            bale.transform,
            Vector3.zero,
            size,
            curedLeaf);

        for (int side = -1; side <= 1; side += 2)
        {
            CreatePrimitive(
                PrimitiveType.Cube,
                "Bale Strap",
                bale.transform,
                new Vector3(side * size.x * 0.27f, 0f, 0f),
                new Vector3(0.07f, size.y + 0.035f, size.z + 0.04f),
                strap,
                false);
        }
    }

    public static GameObject CreatePilonStack(
        Transform parent,
        Vector3 position,
        Material curedLeaf,
        Material timber)
    {
        GameObject pilon = new("Pilot Pilón Stack");
        pilon.transform.SetParent(parent, false);
        pilon.transform.localPosition = position;
        CreatePrimitive(
            PrimitiveType.Cube,
            "Wooden Pallet",
            pilon.transform,
            new Vector3(0f, 0.09f, 0f),
            new Vector3(3.4f, 0.18f, 2.7f),
            timber);

        for (int layer = 0; layer < 5; layer++)
        {
            GameObject leafLayer = CreatePrimitive(
                PrimitiveType.Cube,
                $"Fermenting Leaf Layer {layer + 1}",
                pilon.transform,
                new Vector3(
                    Mathf.Sin(layer * 1.7f) * 0.08f,
                    0.25f + (layer * 0.25f),
                    Mathf.Cos(layer * 1.3f) * 0.06f),
                new Vector3(3.05f - (layer * 0.08f), 0.22f, 2.35f - (layer * 0.06f)),
                curedLeaf);
            leafLayer.transform.localRotation = Quaternion.Euler(0f, layer % 2 == 0 ? 1.5f : -1.5f, 0f);
        }

        return pilon;
    }

    public static GameObject CreateRollingWorkbench(
        Transform parent,
        Vector3 position,
        Material timber,
        Material curedLeaf,
        Material metal)
    {
        GameObject bench = new("Handmade Rolling Workbench");
        bench.transform.SetParent(parent, false);
        bench.transform.localPosition = position;
        GameObject workSurface =
            CreateTableFrame(bench.transform, new Vector3(4.7f, 0.16f, 1.65f), timber);

        for (int index = -1; index <= 1; index++)
        {
            GameObject leaf = CreateLeaf(
                bench.transform,
                "Prepared Wrapper Leaf",
                new Vector3(index * 0.75f, 0.94f + (index * 0.006f), 0.08f),
                curedLeaf);
            leaf.transform.localRotation = Quaternion.Euler(0f, 18f + (index * 22f), 0f);
            leaf.transform.localScale = new Vector3(0.48f, 0.83f, 0.83f);
        }

        GameObject cigar = CreatePrimitive(
            PrimitiveType.Cylinder,
            "Rolled Study Cigar",
            bench.transform,
            new Vector3(1.45f, 0.98f, -0.28f),
            new Vector3(0.075f, 0.55f, 0.075f),
            curedLeaf,
            false);
        cigar.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        CreatePrimitive(
            PrimitiveType.Cube,
            "Chaveta Blade",
            bench.transform,
            new Vector3(-1.45f, 1.01f, -0.3f),
            new Vector3(0.48f, 0.025f, 0.2f),
            metal,
            false).transform.localRotation = Quaternion.Euler(0f, -18f, 0f);
        CreatePrimitive(
            PrimitiveType.Cylinder,
            "Chaveta Handle",
            bench.transform,
            new Vector3(-1.72f, 1.03f, -0.4f),
            new Vector3(0.055f, 0.24f, 0.055f),
            timber,
            false).transform.localRotation = Quaternion.Euler(0f, 0f, 72f);

        return workSurface;
    }

    public static GameObject CreateAgingShelf(
        Transform parent,
        Vector3 position,
        Material timber,
        Material boxMaterial)
    {
        GameObject shelf = new("Aging Shelf");
        shelf.transform.SetParent(parent, false);
        shelf.transform.localPosition = position;

        for (int side = -1; side <= 1; side += 2)
        {
            CreatePrimitive(
                PrimitiveType.Cube,
                "Shelf Upright",
                shelf.transform,
                new Vector3(side * 0.54f, 1.25f, 0f),
                new Vector3(0.09f, 2.5f, 1.75f),
                timber);
        }

        for (int level = 0; level < 4; level++)
        {
            float y = 0.2f + (level * 0.68f);
            CreatePrimitive(
                PrimitiveType.Cube,
                "Shelf Board",
                shelf.transform,
                new Vector3(0f, y, 0f),
                new Vector3(1.18f, 0.08f, 1.8f),
                timber);
            if (level < 3)
            {
                CreatePrimitive(
                    PrimitiveType.Cube,
                    "Resting Cigar Box",
                    shelf.transform,
                    new Vector3(0f, y + 0.19f, 0.08f),
                    new Vector3(0.86f, 0.28f, 1.2f),
                    boxMaterial,
                false);
            }
        }

        return shelf;
    }

    public static GameObject CreateTastingTable(
        Transform parent,
        Vector3 position,
        Material timber,
        Material curedLeaf,
        Material clay)
    {
        GameObject table = new("Courtyard Tasting Table");
        table.transform.SetParent(parent, false);
        table.transform.localPosition = position;
        GameObject top = CreateTableFrame(table.transform, new Vector3(3f, 0.14f, 1.35f), timber);

        CreatePrimitive(
            PrimitiveType.Cylinder,
            "Tasting Cigar",
            table.transform,
            new Vector3(0.35f, 0.98f, 0f),
            new Vector3(0.065f, 0.5f, 0.065f),
            curedLeaf,
            false).transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        CreatePrimitive(
            PrimitiveType.Cylinder,
            "Clay Ash Dish",
            table.transform,
            new Vector3(-0.72f, 0.96f, 0f),
            new Vector3(0.32f, 0.055f, 0.32f),
            clay,
            false);
        return top;
    }

    public static void CreateCistern(
        Transform parent,
        Vector3 position,
        Material clay,
        Material darkMaterial)
    {
        GameObject cistern = new("Water Cistern");
        cistern.transform.SetParent(parent, false);
        cistern.transform.localPosition = position;
        CreatePrimitive(
            PrimitiveType.Cylinder,
            "Rendered Cistern",
            cistern.transform,
            new Vector3(0f, 0.8f, 0f),
            new Vector3(0.95f, 0.8f, 0.95f),
            clay);
        CreatePrimitive(
            PrimitiveType.Cylinder,
            "Water",
            cistern.transform,
            new Vector3(0f, 1.61f, 0f),
            new Vector3(0.72f, 0.025f, 0.72f),
            darkMaterial,
            false);
    }

    public static void CreateShadeTree(
        Transform parent,
        Vector3 position,
        float scale,
        Material timber,
        Material leaf)
    {
        GameObject tree = new("Shade Tree");
        tree.transform.SetParent(parent, false);
        tree.transform.localPosition = position;
        CreatePrimitive(
            PrimitiveType.Cylinder,
            "Trunk",
            tree.transform,
            new Vector3(0f, 1.5f * scale, 0f),
            new Vector3(0.28f * scale, 1.5f * scale, 0.28f * scale),
            timber);

        Vector3[] crownPositions =
        {
            new(0f, 3.45f, 0f),
            new(-0.85f, 3.15f, 0.18f),
            new(0.78f, 3.22f, 0.25f),
            new(0.12f, 3.18f, -0.78f),
        };
        foreach (Vector3 crownPosition in crownPositions)
        {
            CreatePrimitive(
                PrimitiveType.Sphere,
                "Leaf Crown",
                tree.transform,
                crownPosition * scale,
                new Vector3(1.9f, 0.95f, 1.55f) * scale,
                leaf,
                false);
        }
    }

    private static GameObject CreateTableFrame(
        Transform parent,
        Vector3 topSize,
        Material timber)
    {
        GameObject top = CreatePrimitive(
            PrimitiveType.Cube,
            "Work Surface",
            parent,
            new Vector3(0f, 0.88f, 0f),
            topSize,
            timber);
        float halfX = (topSize.x * 0.5f) - 0.25f;
        float halfZ = (topSize.z * 0.5f) - 0.2f;
        for (int x = -1; x <= 1; x += 2)
        {
            for (int z = -1; z <= 1; z += 2)
            {
                CreatePrimitive(
                    PrimitiveType.Cube,
                    "Table Leg",
                    parent,
                    new Vector3(x * halfX, 0.43f, z * halfZ),
                    new Vector3(0.14f, 0.86f, 0.14f),
                    timber,
                    false);
            }
        }

        return top;
    }

    private static GameObject CreateLeaf(
        Transform parent,
        string name,
        Vector3 position,
        Material material)
    {
        Vector3[] face =
        {
            new(0f, 0f, 0f),
            new(-0.20f, 0.015f, 0.22f),
            new(-0.43f, 0.045f, 0.55f),
            new(-0.28f, 0.025f, 0.88f),
            new(0f, 0f, 1.15f),
            new(0.28f, 0.025f, 0.88f),
            new(0.43f, 0.045f, 0.55f),
            new(0.20f, 0.015f, 0.22f),
        };
        Vector3[] vertices = new Vector3[face.Length * 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int index = 0; index < face.Length; index++)
        {
            vertices[index] = face[index];
            vertices[index + face.Length] = face[index];
            float u = 0.5f + face[index].x;
            float v = face[index].z / 1.15f;
            uvs[index] = new Vector2(u, v);
            uvs[index + face.Length] = new Vector2(u, v);
        }

        int[] front =
        {
            0, 1, 2,
            0, 2, 3,
            0, 3, 4,
            0, 4, 5,
            0, 5, 6,
            0, 6, 7,
        };
        int[] triangles = new int[front.Length * 2];
        for (int index = 0; index < front.Length; index++)
        {
            triangles[index] = front[index];
        }

        for (int triangle = 0; triangle < front.Length; triangle += 3)
        {
            int target = front.Length + triangle;
            triangles[target] = front[triangle] + face.Length;
            triangles[target + 1] = front[triangle + 2] + face.Length;
            triangles[target + 2] = front[triangle + 1] + face.Length;
        }

        return CreateMeshObject(name, parent, position, vertices, triangles, uvs, material);
    }
}
}
