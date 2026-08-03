using System;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class ConstructionChoicesState
{
    public ConditioningChoice conditioning;
    public CompressionChoice compression;
    public FillerArrangement fillerArrangement;

    public ConstructionChoicesState Copy()
    {
        return (ConstructionChoicesState)MemberwiseClone();
    }
}
}
