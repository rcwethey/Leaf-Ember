using System;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class BlendIntentState
{
    public string name;
    public string audience;
    public string occasion;
    public StrengthBand desiredStrength;
    public SensoryBand desiredBody;
    public SensoryBand desiredIntensity;
    public AromaFamily dominantFamily;
    public AromaFamily supportingFamily;
    public string desiredProgression;
    public string desiredFinish;
    public string productionConstraint;

    public BlendIntentState Copy()
    {
        return (BlendIntentState)MemberwiseClone();
    }
}
}
