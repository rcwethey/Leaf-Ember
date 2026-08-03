using System;
using System.Collections.Generic;
using LeafEmber.Cigar;

namespace LeafEmber.Inventory
{

public sealed class InventoryService : IInventoryService
{
    private readonly List<LeafLotState> leafLots = new();

    public InventoryService()
    {
        leafLots.AddRange(CreateDefaultLots());
    }

    public IReadOnlyList<LeafLotState> LeafLots => leafLots;

    public InventorySnapshot Capture()
    {
        InventorySnapshot snapshot = new();
        foreach (LeafLotState leafLot in leafLots)
        {
            snapshot.leafLots.Add(leafLot.Copy());
        }

        return snapshot;
    }

    public void Restore(InventorySnapshot snapshot)
    {
        if (snapshot == null || snapshot.leafLots == null)
        {
            throw new ArgumentException("The inventory snapshot is invalid.", nameof(snapshot));
        }

        Dictionary<string, LeafLotState> defaults = new();
        foreach (LeafLotState defaultLot in CreateDefaultLots())
        {
            defaults.Add(defaultLot.id, defaultLot);
        }

        leafLots.Clear();
        foreach (LeafLotState leafLot in snapshot.leafLots)
        {
            if (leafLot == null || string.IsNullOrWhiteSpace(leafLot.id))
            {
                throw new ArgumentException(
                    "Every restored leaf lot requires an identifier.",
                    nameof(snapshot));
            }

            LeafLotState restored = leafLot.Copy();
            if (defaults.TryGetValue(restored.id, out LeafLotState definition))
            {
                restored.potential ??= definition.potential.Copy();
                restored.process ??= definition.process.Copy();
            }

            leafLots.Add(restored);
        }

        foreach (LeafLotState defaultLot in defaults.Values)
        {
            if (!leafLots.Exists(existing => existing.id == defaultLot.id))
            {
                leafLots.Add(defaultLot.Copy());
            }
        }
    }

    private static List<LeafLotState> CreateDefaultLots()
    {
        return new List<LeafLotState>
        {
            new()
            {
                id = "finca-pilot-seco",
                displayName = "Finca Pilot Seco",
                origin = "North field, middle priming",
                grower = "Leaf & Ember finca",
                tobaccoType = "Estate pilot seed",
                harvestReference = "Previous dry-period pilot harvest",
                processHistory = "Air-cured; one gentle fermentation cycle; rested 4 months",
                intendedRole = "Aromatic filler study",
                observations = "Cedar-like aroma, toasted grain, restrained sweetness; delicate leaf",
                quantityKilograms = 7.5f,
                potential = new LeafPotentialState
                {
                    strength = StrengthBand.Mild,
                    body = SensoryBand.Medium,
                    intensity = SensoryBand.Medium,
                    sweetness = SensoryBand.Medium,
                    dryness = SensoryBand.Medium,
                    irritation = SensoryBand.Low,
                    finish = SensoryBand.Medium,
                    combustionSupport = SensoryBand.Medium,
                    elasticity = SensoryBand.Medium,
                    primaryFamily = AromaFamily.Wood,
                    secondaryFamily = AromaFamily.HerbalAndGreen,
                },
                process = HealthyProcess("Gentle and even", "One controlled cycle", 4),
            },
            new()
            {
                id = "ortega-valley-viso",
                displayName = "Ortega Valley Viso",
                origin = "Neighboring upland plot, upper-middle priming",
                grower = "Elena Ortega",
                tobaccoType = "Locally selected criollo-type seed",
                harvestReference = "Most recent rainy-to-dry transition",
                processHistory = "Barn-cured; two fermentation cycles; rested 8 months",
                intendedRole = "Structure and warm spice in filler blends",
                observations = "Firm leaf with cocoa-like aroma and peppery finish",
                quantityKilograms = 4.25f,
                potential = new LeafPotentialState
                {
                    strength = StrengthBand.Full,
                    body = SensoryBand.High,
                    intensity = SensoryBand.High,
                    sweetness = SensoryBand.Low,
                    dryness = SensoryBand.Medium,
                    irritation = SensoryBand.Medium,
                    finish = SensoryBand.High,
                    combustionSupport = SensoryBand.Medium,
                    elasticity = SensoryBand.Medium,
                    primaryFamily = AromaFamily.Spice,
                    secondaryFamily = AromaFamily.RoastedAndNutty,
                },
                process = HealthyProcess("Even color", "Two complete cycles", 8),
            },
            new()
            {
                id = "san-jeronimo-binder",
                displayName = "San Jerónimo Binder",
                origin = "Partner farm, lower-middle priming",
                grower = "San Jerónimo growers' group",
                tobaccoType = "Broad, elastic local selection",
                harvestReference = "Previous rainy-period harvest",
                processHistory = "Slow barn cure; one moderate fermentation; rested 10 months",
                intendedRole = "Binder with combustion support and earthy depth",
                observations = "Supple hand, even veins, dry earth and leather-like aroma",
                quantityKilograms = 3.6f,
                potential = new LeafPotentialState
                {
                    strength = StrengthBand.Medium,
                    body = SensoryBand.Medium,
                    intensity = SensoryBand.Medium,
                    sweetness = SensoryBand.Low,
                    dryness = SensoryBand.High,
                    irritation = SensoryBand.Low,
                    finish = SensoryBand.Medium,
                    combustionSupport = SensoryBand.High,
                    elasticity = SensoryBand.High,
                    primaryFamily = AromaFamily.EarthAndMineral,
                    secondaryFamily = AromaFamily.FermentedAndLeather,
                },
                process = HealthyProcess("Slow and even", "One moderate cycle", 10),
            },
            new()
            {
                id = "las-lomas-colorado-wrapper",
                displayName = "Las Lomas Colorado Wrapper",
                origin = "Sheltered valley bench, upper-middle priming",
                grower = "María Ruiz",
                tobaccoType = "Fine-textured colorado selection",
                harvestReference = "Previous dry-period harvest",
                processHistory = "Careful barn cure; two light fermentations; rested 12 months",
                intendedRole = "Wrapper for aroma, finish, and clean presentation",
                observations = "Silky surface, resilient edge, cedar and cocoa-like aroma",
                quantityKilograms = 1.8f,
                potential = new LeafPotentialState
                {
                    strength = StrengthBand.Medium,
                    body = SensoryBand.Medium,
                    intensity = SensoryBand.High,
                    sweetness = SensoryBand.Medium,
                    dryness = SensoryBand.Low,
                    irritation = SensoryBand.Low,
                    finish = SensoryBand.High,
                    combustionSupport = SensoryBand.Medium,
                    elasticity = SensoryBand.High,
                    primaryFamily = AromaFamily.RoastedAndNutty,
                    secondaryFamily = AromaFamily.Wood,
                },
                process = HealthyProcess("Careful and even", "Two light cycles", 12),
            },
        };
    }

    private static LeafProcessState HealthyProcess(
        string curing,
        string fermentation,
        int restMonths)
    {
        return new LeafProcessState
        {
            curingCondition = curing,
            fermentationCondition = fermentation,
            storageCondition = "Stable cabinet condition",
            restMonths = restMonths,
            hasStructuralDamage = false,
        };
    }
}
}
