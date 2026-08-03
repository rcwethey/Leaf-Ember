using System;
using System.Collections.Generic;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class TastingRecordState
{
    public string id;
    public string prototypeId;
    public string recipeId;
    public int recipeVersion;
    public int tastedAtElapsedBlock;
    public StrengthBand observedStrength;
    public SensoryBand observedBody;
    public SensoryBand observedIntensity;
    public AromaFamily observedDominantFamily;
    public AromaFamily observedSupportingFamily;
    public string observedFinish;
    public string intentComparison;
    public string independentFeedbackSource;
    public string independentFeedback;
    public List<TastingStageState> stages = new();

    public TastingRecordState Copy()
    {
        TastingRecordState copy = (TastingRecordState)MemberwiseClone();
        copy.stages = new List<TastingStageState>();
        foreach (TastingStageState stage in stages)
        {
            copy.stages.Add(stage.Copy());
        }

        return copy;
    }
}
}
