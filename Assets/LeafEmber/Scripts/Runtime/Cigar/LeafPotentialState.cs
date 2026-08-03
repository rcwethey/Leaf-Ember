using System;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class LeafPotentialState
{
    public StrengthBand strength;
    public SensoryBand body;
    public SensoryBand intensity;
    public SensoryBand sweetness;
    public SensoryBand dryness;
    public SensoryBand irritation;
    public SensoryBand finish;
    public SensoryBand combustionSupport;
    public SensoryBand elasticity;
    public AromaFamily primaryFamily;
    public AromaFamily secondaryFamily;

    public LeafPotentialState Copy()
    {
        return (LeafPotentialState)MemberwiseClone();
    }
}
}
