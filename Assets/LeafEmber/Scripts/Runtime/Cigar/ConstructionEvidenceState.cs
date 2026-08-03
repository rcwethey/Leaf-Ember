using System;
using System.Collections.Generic;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class ConstructionEvidenceState
{
    public float weightGrams;
    public float lengthMillimeters;
    public float diameterMillimeters;
    public DrawBehavior draw;
    public string firmness;
    public string burnExpectation;
    public string wrapperCondition;
    public string seamAndCap;
    public string moistureDistribution;
    public List<string> visibleEvidence = new();

    public ConstructionEvidenceState Copy()
    {
        ConstructionEvidenceState copy = (ConstructionEvidenceState)MemberwiseClone();
        copy.visibleEvidence = new List<string>(visibleEvidence);
        return copy;
    }
}
}
