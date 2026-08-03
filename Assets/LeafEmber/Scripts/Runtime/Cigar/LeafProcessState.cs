using System;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class LeafProcessState
{
    public string curingCondition;
    public string fermentationCondition;
    public string storageCondition;
    public int restMonths;
    public bool hasStructuralDamage;

    public LeafProcessState Copy()
    {
        return (LeafProcessState)MemberwiseClone();
    }
}
}
