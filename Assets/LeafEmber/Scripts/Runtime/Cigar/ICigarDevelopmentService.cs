using System.Collections.Generic;

namespace LeafEmber.Cigar
{

public interface ICigarDevelopmentService
{
    IReadOnlyList<RecipeVersionState> Recipes { get; }

    IReadOnlyList<PrototypeCigarState> Prototypes { get; }

    IReadOnlyList<TastingRecordState> Tastings { get; }

    IReadOnlyList<DiagnosisState> Diagnoses { get; }

    bool HasPendingRevision { get; }

    RecipeVersionState LatestRecipe { get; }

    PrototypeCigarState CreateInitialPrototype(
        IntentPreset intent,
        BlendPreset blend,
        ConstructionChoicesState choices,
        int elapsedBlock);

    PrototypeCigarState ConstructPendingRevision(
        ConstructionChoicesState choices,
        int elapsedBlock);

    TastingRecordState TastePrototype(string prototypeId, int elapsedBlock);

    RecipeVersionState DiagnoseAndRevise(
        string prototypeId,
        DiagnosisKind diagnosis,
        int elapsedBlock);

    string CompareLatestTastings();

    CigarDevelopmentSnapshot Capture();

    void Restore(CigarDevelopmentSnapshot snapshot);
}
}
