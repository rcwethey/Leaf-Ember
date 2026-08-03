using LeafEmber.Prototype.Interaction;
using UnityEngine;

namespace LeafEmber.Prototype
{

public static class FincaWorldBuilder
{
    public static void BuildEnvironment(Transform parent)
    {
        Material earth = CreateMaterial("Earth", new Color(0.28f, 0.23f, 0.14f));
        Material grass = CreateMaterial("Grass", new Color(0.25f, 0.39f, 0.18f));
        Material path = CreateMaterial("Path", new Color(0.47f, 0.36f, 0.22f));
        Material wood = CreateMaterial("Wood", new Color(0.34f, 0.20f, 0.11f));
        Material clay = CreateMaterial("Clay", new Color(0.62f, 0.33f, 0.19f));
        Material plaster = CreateMaterial("Plaster", new Color(0.72f, 0.65f, 0.49f));
        Material leaf = CreateMaterial("Leaf", new Color(0.31f, 0.48f, 0.20f));
        Material curedLeaf = CreateMaterial("Cured Leaf", new Color(0.50f, 0.31f, 0.13f));
        Material focus = CreateMaterial("Focused Work", new Color(0.76f, 0.46f, 0.16f));
        Material inspect = CreateMaterial("Inspection", new Color(0.20f, 0.48f, 0.46f));

        CreateBox("Finca Ground", parent, new Vector3(0f, -0.3f, 2f), new Vector3(52f, 0.6f, 46f), grass);
        CreateBox("Courtyard", parent, new Vector3(0f, 0.02f, 0f), new Vector3(18f, 0.08f, 14f), earth);
        CreatePath(parent, path, new Vector3(0f, 0.07f, 7f), new Vector3(4f, 0.06f, 24f));
        CreatePath(parent, path, new Vector3(8f, 0.07f, 1f), new Vector3(18f, 0.06f, 3f));
        CreatePath(parent, path, new Vector3(-10f, 0.07f, 6f), new Vector3(16f, 0.06f, 3f));

        BuildFieldEdge(parent, leaf, earth, inspect);
        BuildCuringBarn(parent, wood, clay, curedLeaf, inspect);
        BuildFermentationRoom(parent, plaster, clay, wood, focus);
        BuildLeafStorage(parent, plaster, wood, curedLeaf);
        BuildWorkshop(parent, plaster, clay, wood, focus);
        BuildAgingRoom(parent, wood, plaster, curedLeaf, inspect);
        BuildOffice(parent, plaster, clay, wood, focus);
        AddCourtyardDetails(parent, wood, clay, leaf);
    }

    private static void BuildFieldEdge(
        Transform parent,
        Material leaf,
        Material earth,
        Material inspect)
    {
        Transform zone = CreateZone("01 — FIELD EDGE", parent, new Vector3(-18f, 0f, 7f));
        CreateBox("Field Plot", zone, new Vector3(0f, 0.08f, 0f), new Vector3(10f, 0.15f, 13f), earth);
        for (int row = -4; row <= 4; row += 2)
        {
            for (int plant = -5; plant <= 5; plant += 2)
            {
                CreateBox(
                    "Pilot Tobacco",
                    zone,
                    new Vector3(row, 0.65f, plant),
                    new Vector3(0.45f, 1.3f, 0.45f),
                    leaf);
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
            CreateBox(
                "Hanging Rail",
                zone,
                new Vector3(index, 1.7f, 1f),
                new Vector3(0.25f, 2.8f, 4.2f),
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
        CreateBox("Pilot Pilon", zone, new Vector3(0f, 0.7f, 0.8f), new Vector3(3.5f, 1.4f, 3f), wood);
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
        Material curedLeaf)
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
            CreateBox(
                "Leaf Bale",
                zone,
                new Vector3(index, 0.65f, 1.5f),
                new Vector3(1.5f, 1.3f, 2f),
                curedLeaf);
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
        CreateBox("Rolling Table", zone, new Vector3(0f, 0.8f, 0.8f), new Vector3(4.6f, 1.6f, 1.6f), wood);
        FocusedWorkstation station = CreateFocusedStation(
            zone,
            "Rolling Work Point",
            new Vector3(0f, 0.6f, -2f),
            new Vector3(2.7f, 1.2f, 0.7f),
            focus);
        station.Configure(
            "roll a study cigar",
            "Condition a small leaf selection and personally construct one study cigar. " +
            "This focused transition protects the embodied craft moment without turning " +
            "every repeated production action into a separate minigame.",
            1);
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
            CreateBox(
                "Aging Shelf",
                zone,
                new Vector3(index, 1.25f, 1.2f),
                new Vector3(1.2f, 2.5f, 2f),
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
        Material leaf)
    {
        CreateBox("Courtyard Table", parent, new Vector3(1f, 0.75f, 1f), new Vector3(3f, 1.5f, 1.4f), wood);
        CreateBox("Water Cistern", parent, new Vector3(-6f, 0.9f, 2f), new Vector3(1.8f, 1.8f, 1.8f), clay);
        for (int index = -1; index <= 1; index++)
        {
            CreateBox(
                "Shade Tree",
                parent,
                new Vector3(-14f + (index * 13f), 1.5f, -17f),
                new Vector3(0.8f, 3f, 0.8f),
                wood);
            CreateBox(
                "Shade Canopy",
                parent,
                new Vector3(-14f + (index * 13f), 3.5f, -17f),
                new Vector3(4f, 1.8f, 4f),
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
        CreateBox("Roof", zone, new Vector3(0f, 3.3f, 0f), new Vector3(size.x + 0.6f, 0.25f, size.y + 0.6f), roof);
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

    private static Material CreateMaterial(string name, Color color)
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
        };
        return material;
    }
}
}
