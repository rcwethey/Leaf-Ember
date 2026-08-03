using System.Collections.Generic;

namespace LeafEmber.Inventory
{

public interface IInventoryService
{
    IReadOnlyList<LeafLotState> LeafLots { get; }

    InventorySnapshot Capture();

    void Restore(InventorySnapshot snapshot);
}
}
