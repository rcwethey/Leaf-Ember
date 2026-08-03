using System;
using System.Collections.Generic;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class CigarDevelopmentSnapshot
{
    public List<RecipeVersionState> recipes = new();
    public List<PrototypeCigarState> prototypes = new();
    public List<TastingRecordState> tastings = new();
    public List<DiagnosisState> diagnoses = new();

    public CigarDevelopmentSnapshot Copy()
    {
        CigarDevelopmentSnapshot copy = new();
        foreach (RecipeVersionState recipe in recipes)
        {
            copy.recipes.Add(recipe.Copy());
        }

        foreach (PrototypeCigarState prototype in prototypes)
        {
            copy.prototypes.Add(prototype.Copy());
        }

        foreach (TastingRecordState tasting in tastings)
        {
            copy.tastings.Add(tasting.Copy());
        }

        foreach (DiagnosisState diagnosis in diagnoses)
        {
            copy.diagnoses.Add(diagnosis.Copy());
        }

        return copy;
    }
}
}
