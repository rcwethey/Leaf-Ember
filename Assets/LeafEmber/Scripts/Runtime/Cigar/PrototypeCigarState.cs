using System;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class PrototypeCigarState
{
    public string id;
    public string recipeId;
    public int recipeVersion;
    public string displayName;
    public ConstructionChoicesState choices;
    public ConstructionEvidenceState construction;
    public CigarExpressionState hiddenExpression;
    public int constructedAtElapsedBlock;
    public int readyAtElapsedBlock;
    public bool consumedByTasting;

    public PrototypeCigarState Copy()
    {
        PrototypeCigarState copy = (PrototypeCigarState)MemberwiseClone();
        copy.choices = choices?.Copy();
        copy.construction = construction?.Copy();
        copy.hiddenExpression = hiddenExpression?.Copy();
        return copy;
    }
}
}
