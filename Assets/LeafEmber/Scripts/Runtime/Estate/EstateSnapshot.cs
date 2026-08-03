using System;
using System.Collections.Generic;

namespace LeafEmber.Estate
{

[Serializable]
public sealed class EstateSnapshot
{
    public string estateName = "Leaf & Ember finca";
    public List<FacilityState> facilities = new();

    public EstateSnapshot Copy()
    {
        EstateSnapshot copy = new() { estateName = estateName };
        foreach (FacilityState facility in facilities)
        {
            copy.facilities.Add(facility.Copy());
        }

        return copy;
    }
}
}
