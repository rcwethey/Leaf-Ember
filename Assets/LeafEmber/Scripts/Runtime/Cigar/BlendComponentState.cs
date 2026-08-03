using System;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class BlendComponentState
{
    public string leafLotId;
    public LeafRole role;
    public int proportionPercent;
    public string placement;

    public BlendComponentState Copy()
    {
        return (BlendComponentState)MemberwiseClone();
    }
}
}
