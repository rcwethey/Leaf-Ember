using System;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class TastingStageState
{
    public string stage;
    public string constructionObservation;
    public string sensoryObservation;
    public string confidence;

    public TastingStageState Copy()
    {
        return (TastingStageState)MemberwiseClone();
    }
}
}
