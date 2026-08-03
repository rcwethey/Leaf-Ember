using System;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class CigarExpressionState
{
    public StrengthBand strength;
    public SensoryBand body;
    public SensoryBand intensity;
    public SensoryBand sweetness;
    public SensoryBand smoothness;
    public SensoryBand finish;
    public AromaFamily dominantFamily;
    public AromaFamily supportingFamily;
    public string progression;
    public string combustionCharacter;
    public string interaction;

    public CigarExpressionState Copy()
    {
        return (CigarExpressionState)MemberwiseClone();
    }
}
}
