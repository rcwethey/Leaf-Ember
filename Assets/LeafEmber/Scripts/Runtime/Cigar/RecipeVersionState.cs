using System;
using System.Collections.Generic;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class RecipeVersionState
{
    public string recipeId;
    public int version;
    public string versionLabel;
    public BlendIntentState intent;
    public List<BlendComponentState> components = new();
    public string vitola;
    public int lengthMillimeters;
    public int ringGauge;
    public ConditioningChoice targetConditioning;
    public CompressionChoice targetCompression;
    public FillerArrangement targetArrangement;
    public int requiredRestBlocks;
    public int createdAtElapsedBlock;
    public string revisionRationale;
    public bool hasConstructedPrototype;

    public RecipeVersionState Copy()
    {
        RecipeVersionState copy = (RecipeVersionState)MemberwiseClone();
        copy.intent = intent?.Copy();
        copy.components = new List<BlendComponentState>();
        foreach (BlendComponentState component in components)
        {
            copy.components.Add(component.Copy());
        }

        return copy;
    }
}
}
