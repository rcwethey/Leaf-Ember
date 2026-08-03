using System;

namespace LeafEmber.Estate
{

[Serializable]
public sealed class FacilityState
{
    public string id;
    public string displayName;
    public string condition;
    public bool operational;

    public FacilityState Copy()
    {
        return new FacilityState
        {
            id = id,
            displayName = displayName,
            condition = condition,
            operational = operational,
        };
    }
}
}
