using LeafEmber.Prototype.Interaction;
using UnityEngine;

namespace LeafEmber.Prototype
{

public static partial class FincaWorldBuilder
{
    public static void BuildEnvironment(Transform parent)
    {
        BuildExpandedEnvironment(parent);
    }

    private static void BuildFieldEdge(
        Transform parent,
        Material leaf,
        Material stem,
        Material earth,
        Material inspect)
    {
        Transform zone = CreateZone("01 — FIELD EDGE", parent, new Vector3(-18f, 0f, 7f));
        CreateBox("Field Plot", zone, new Vector3(0f, 0.08f, 0f), new Vector3(10f, 0.15f, 13f), earth);
        for (int row = -4; row <= 4; row += 2)
        {
            for (int plant = -5; plant <= 5; plant += 2)
            {
                float phase = ((row + 4) * 0.37f) + ((plant + 5) * 0.19f);
                HandmadeFincaAssets.CreateTobaccoPlant(
                    zone,
                    new Vector3(row, 0.15f, plant),
                    1.45f + (Mathf.Sin(phase) * 0.08f),
                    stem,
                    leaf,
                    phase);
            }
        }

        CreateWorldLabel(zone, "FIELD EDGE\nOBSERVATION PLOT", new Vector3(0f, 2.7f, -6.3f));
        InformationStation station = CreateInformationStation(
            zone,
            "Crop Observation Post",
            new Vector3(0f, 0.65f, -5.1f),
            new Vector3(2.2f, 1.3f, 0.6f),
            inspect);
        station.Configure(
            "pilot crop notes",
            "The estate crop gives the house a local voice, but one plot cannot supply every " +
            "desired strength, aroma, combustion, and flavor characteristic. This pilot plot " +
            "will eventually feed the first crop-to-cured-lot loop.");
    }

    private static void BuildCuringBarn(
        Transform parent,
        Material wood,
        Material roof,
        Material curedLeaf,
        Material inspect)
    {
        Transform zone = CreateBay(
            "02 — CURING BARN",
            parent,
            new Vector3(-10f, 0f, 15f),
            new Vector2(8f, 8f),
            wood,
            roof);
        for (int index = -2; index <= 2; index += 2)
        {
            HandmadeFincaAssets.CreateCuringRack(
                zone,
                new Vector3(index, 0.16f, 1f),
                1.65f,
                3.3f,
                wood,
                curedLeaf);
        }

        CreateWorldLabel(zone, "CURING BARN", new Vector3(0f, 3.6f, -4.2f));
        InformationStation station = CreateInformationStation(
            zone,
            "Curing Rack",
            new Vector3(0f, 0.55f, -2.4f),
            new Vector3(2.4f, 1.1f, 0.7f),
            inspect);
        station.Configure(
            "curing rack",
            "The pilot leaves are hanging evenly. Color and moisture observations belong to " +
            "the lot history; inspecting them is free. A future checkpoint can require a " +
            "decision when conditions or progress meaningfully change.");
    }

    private static void BuildFermentationRoom(
        Transform parent,
        Material wall,
        Material roof,
        Material wood,
        Material curedLeaf,
        Material focus)
    {
        Transform zone = CreateBay(
            "03 — FERMENTATION ROOM",
            parent,
            new Vector3(0f, 0f, 16f),
            new Vector2(8f, 7f),
            wall,
            roof);
        CreateWorldLabel(zone, "FERMENTATION", new Vector3(0f, 3.6f, -3.7f));
        HandmadeFincaAssets.CreatePilonStack(zone, new Vector3(0f, 0.16f, 0.8f), curedLeaf, wood);
        FocusedWorkstation station = CreateFocusedStation(
            zone,
            "Pilon Work Point",
            new Vector3(0f, 0.6f, -2.1f),
            new Vector3(2.5f, 1.2f, 0.7f),
            focus);
        station.Configure(
            "turn the pilot pilón",
            "Open the stack, assess its warmth and moisture, and rebuild it with a deliberate " +
            "leaf distribution. The prototype records the committed work block while later " +
            "milestones will model transformation and risk.",
            1);
    }

    private static void BuildLeafStorage(
        Transform parent,
        Material wall,
        Material wood,
        Material curedLeaf,
        Material strap)
    {
        Transform zone = CreateBay(
            "04 — LEAF STORAGE",
            parent,
            new Vector3(10f, 0f, 14f),
            new Vector2(8f, 8f),
            wall,
            wood);
        CreateWorldLabel(zone, "LEAF STORAGE", new Vector3(0f, 3.6f, -4.2f));
        for (int index = -2; index <= 2; index += 2)
        {
            HandmadeFincaAssets.CreateLeafBale(
                zone,
                new Vector3(index, 0.65f, 1.5f),
                new Vector3(1.5f, 1.3f, 2f),
                curedLeaf,
                strap);
        }

        GameObject cabinetObject = CreateBox(
            "Provenance Cabinet",
            zone,
            new Vector3(0f, 1f, -2.4f),
            new Vector3(3f, 2f, 0.7f),
            wood);
        cabinetObject.AddComponent<LeafLotCabinet>();
    }

    private static void BuildWorkshop(
        Transform parent,
        Material wall,
        Material roof,
        Material wood,
        Material curedLeaf,
        Material metal,
        Material focus)
    {
        Transform zone = CreateBay(
            "05 — WORKSHOP",
            parent,
            new Vector3(13f, 0f, 2f),
            new Vector2(9f, 8f),
            wall,
            roof);
        CreateWorldLabel(zone, "WORKSHOP", new Vector3(0f, 3.6f, -4.2f));
        HandmadeFincaAssets.CreateRollingWorkbench(
            zone,
            new Vector3(0f, 0.16f, 0.8f),
            wood,
            curedLeaf,
            metal);
        GameObject workPoint = CreateBox(
            "Cigar Development Work Point",
            zone,
            new Vector3(0f, 0.6f, -2f),
            new Vector3(2.7f, 1.2f, 0.7f),
            focus);
        workPoint.AddComponent<CigarWorkbench>();
    }

    private static void BuildAgingRoom(
        Transform parent,
        Material wall,
        Material trim,
        Material curedLeaf,
        Material inspect)
    {
        Transform zone = CreateBay(
            "06 — AGING ROOM",
            parent,
            new Vector3(8f, 0f, -10f),
            new Vector2(9f, 7f),
            wall,
            trim);
        CreateWorldLabel(zone, "AGING ROOM", new Vector3(0f, 3.6f, -3.7f));
        for (int index = -3; index <= 3; index += 2)
        {
            HandmadeFincaAssets.CreateAgingShelf(
                zone,
                new Vector3(index, 0.16f, 1.2f),
                trim,
                curedLeaf);
        }

        InformationStation station = CreateInformationStation(
            zone,
            "Aging Ledger",
            new Vector3(0f, 0.6f, -2f),
            new Vector3(2.4f, 1.2f, 0.7f),
            inspect);
        station.Configure(
            "aging ledger",
            "There are no mature house releases yet. Aging will reveal change over weeks, " +
            "months, and years, but longer will never mean automatically better. The ledger " +
            "will preserve batch identity and observations.");
    }

    private static void BuildOffice(
        Transform parent,
        Material wall,
        Material roof,
        Material wood,
        Material focus)
    {
        Transform zone = CreateBay(
            "FINCA OFFICE",
            parent,
            new Vector3(-5f, 0f, -10f),
            new Vector2(9f, 7f),
            wall,
            roof);
        CreateWorldLabel(zone, "FINCA OFFICE", new Vector3(0f, 3.6f, -3.7f));
        CreateBox("Records Shelf", zone, new Vector3(-2.8f, 1.2f, 1.3f), new Vector3(1.3f, 2.4f, 2.2f), wood);
        FocusedWorkstation station = CreateFocusedStation(
            zone,
            "Planning Desk",
            new Vector3(0.8f, 0.75f, -1.6f),
            new Vector3(3.4f, 1.5f, 1.2f),
            focus);
        station.Configure(
            "organize the house records",
            "Reconcile the lot ledger, upcoming checkpoints, and workshop notes. Reading is " +
            "free, but deliberately reorganizing the house record is committed work.",
            1);
    }

    private static void AddCourtyardDetails(
        Transform parent,
        Material wood,
        Material clay,
        Material leaf,
        Material curedLeaf,
        Material water)
    {
        GameObject courtyardTable = HandmadeFincaAssets.CreateTastingTable(
            parent,
            new Vector3(1f, 0.08f, 1f),
            wood,
            curedLeaf,
            clay);
        courtyardTable.AddComponent<TastingTable>();
        HandmadeFincaAssets.CreateCistern(parent, new Vector3(-6f, 0.08f, 2f), clay, water);
        for (int index = -1; index <= 1; index++)
        {
            HandmadeFincaAssets.CreateShadeTree(
                parent,
                new Vector3(-14f + (index * 13f), 0f, -17f),
                1f + (index * 0.04f),
                wood,
                leaf);
        }
    }

    private static Transform CreateZone(string name, Transform parent, Vector3 position)
    {
        GameObject zone = new(name);
        zone.transform.SetParent(parent, false);
        zone.transform.localPosition = position;
        return zone.transform;
    }

    private static Transform CreateBay(
        string name,
        Transform parent,
        Vector3 position,
        Vector2 size,
        Material wall,
        Material roof)
    {
        Transform zone = CreateZone(name, parent, position);
        float halfWidth = size.x * 0.5f;
        float halfDepth = size.y * 0.5f;
        CreateBox("Floor", zone, new Vector3(0f, 0.08f, 0f), new Vector3(size.x, 0.16f, size.y), wall);
        CreateBox("Back Wall", zone, new Vector3(0f, 1.6f, halfDepth), new Vector3(size.x, 3.2f, 0.3f), wall);
        CreateBox("Left Wall", zone, new Vector3(-halfWidth, 1.6f, 0.5f), new Vector3(0.3f, 3.2f, size.y - 1f), wall);
        CreateBox("Right Wall", zone, new Vector3(halfWidth, 1.6f, 0.5f), new Vector3(0.3f, 3.2f, size.y - 1f), wall);
        CreateBox(
            "Front Left Post",
            zone,
            new Vector3(-halfWidth + 0.15f, 1.6f, -halfDepth + 0.15f),
            new Vector3(0.3f, 3.2f, 0.3f),
            roof);
        CreateBox(
            "Front Right Post",
            zone,
            new Vector3(halfWidth - 0.15f, 1.6f, -halfDepth + 0.15f),
            new Vector3(0.3f, 3.2f, 0.3f),
            roof);
        HandmadeFincaAssets.CreateGabledRoof(
            zone,
            "Gabled Roof",
            new Vector3(0f, 3.18f, 0f),
            size.x + 0.7f,
            size.y + 0.7f,
            1.25f,
            roof);
        return zone;
    }

    private static FocusedWorkstation CreateFocusedStation(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 scale,
        Material material)
    {
        GameObject station = CreateBox(name, parent, position, scale, material);
        return station.AddComponent<FocusedWorkstation>();
    }

    private static InformationStation CreateInformationStation(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 scale,
        Material material)
    {
        GameObject station = CreateBox(name, parent, position, scale, material);
        return station.AddComponent<InformationStation>();
    }

    private static void CreatePath(
        Transform parent,
        Material material,
        Vector3 position,
        Vector3 scale)
    {
        CreateBox("Footpath", parent, position, scale, material);
    }

    private static GameObject CreateBox(
        string name,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Material material)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.SetParent(parent, false);
        box.transform.localPosition = position;
        box.transform.localScale = scale;
        box.GetComponent<MeshRenderer>().sharedMaterial = material;
        return box;
    }

    private static void CreateWorldLabel(Transform parent, string text, Vector3 position)
    {
        GameObject labelObject = new($"{text} Label");
        labelObject.transform.SetParent(parent, false);
        labelObject.transform.localPosition = position;
        labelObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        TextMesh label = labelObject.AddComponent<TextMesh>();
        label.text = text;
        label.anchor = TextAnchor.MiddleCenter;
        label.alignment = TextAlignment.Center;
        label.characterSize = 0.12f;
        label.fontSize = 52;
        label.color = new Color(0.95f, 0.88f, 0.65f);
    }

    private static Material CreateMaterial(
        string name,
        Color color,
        string textureResource = null,
        Vector2? textureScale = null,
        float smoothness = 0.18f,
        string normalResource = null,
        bool alphaClip = false,
        bool doubleSided = false)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        Material material = new(shader)
        {
            name = $"Prototype {name}",
            color = color,
            enableInstancing = true,
        };
        if (material.HasProperty("_Smoothness"))
        {
            material.SetFloat("_Smoothness", smoothness);
        }

        if (!string.IsNullOrWhiteSpace(textureResource))
        {
            Texture2D texture = Resources.Load<Texture2D>(textureResource);
            if (texture != null)
            {
                material.mainTexture = texture;
                material.mainTextureScale = textureScale ?? Vector2.one;
                if (material.HasProperty("_BaseMap"))
                {
                    material.SetTexture("_BaseMap", texture);
                    material.SetTextureScale("_BaseMap", textureScale ?? Vector2.one);
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(normalResource))
        {
            Texture2D normal = Resources.Load<Texture2D>(normalResource);
            if (normal != null && material.HasProperty("_BumpMap"))
            {
                material.SetTexture("_BumpMap", normal);
                material.EnableKeyword("_NORMALMAP");
                if (material.HasProperty("_BumpScale"))
                {
                    material.SetFloat("_BumpScale", 0.72f);
                }
            }
        }

        if (alphaClip)
        {
            if (material.HasProperty("_AlphaClip"))
            {
                material.SetFloat("_AlphaClip", 1f);
            }

            if (material.HasProperty("_Cutoff"))
            {
                material.SetFloat("_Cutoff", 0.32f);
            }

            material.EnableKeyword("_ALPHATEST_ON");
            material.renderQueue = 2450;
        }

        if (doubleSided && material.HasProperty("_Cull"))
        {
            material.SetFloat("_Cull", 0f);
        }

        return material;
    }
}
}
