using System;
using System.Collections.Generic;

namespace LeafEmber.Inventory
{

public sealed class InventoryService : IInventoryService
{
    private readonly List<LeafLotState> leafLots = new();

    public InventoryService()
    {
        leafLots.Add(new LeafLotState
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
        });
        leafLots.Add(new LeafLotState
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
        });
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

        leafLots.Clear();
        foreach (LeafLotState leafLot in snapshot.leafLots)
        {
            if (leafLot == null || string.IsNullOrWhiteSpace(leafLot.id))
            {
                throw new ArgumentException(
                    "Every restored leaf lot requires an identifier.",
                    nameof(snapshot));
            }

            leafLots.Add(leafLot.Copy());
        }
    }
}
}
