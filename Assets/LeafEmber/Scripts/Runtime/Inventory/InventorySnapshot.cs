using System;
using System.Collections.Generic;

namespace LeafEmber.Inventory
{

[Serializable]
public sealed class InventorySnapshot
{
    public List<LeafLotState> leafLots = new();

    public InventorySnapshot Copy()
    {
        InventorySnapshot copy = new();
        foreach (LeafLotState leafLot in leafLots)
        {
            copy.leafLots.Add(leafLot.Copy());
        }

        return copy;
    }
}
}
