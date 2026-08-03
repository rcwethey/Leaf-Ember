using System;
using System.Collections.Generic;
using System.Linq;
using LeafEmber.Inventory;

namespace LeafEmber.Cigar
{

public sealed partial class CigarDevelopmentService : ICigarDevelopmentService
{
    private const string FoundingRecipeId = "founding-study";

    private readonly IInventoryService inventory;
    private CigarDevelopmentSnapshot state = new();

    public CigarDevelopmentService(IInventoryService inventoryService)
    {
        inventory = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
    }

    public IReadOnlyList<RecipeVersionState> Recipes => state.recipes;

    public IReadOnlyList<PrototypeCigarState> Prototypes => state.prototypes;

    public IReadOnlyList<TastingRecordState> Tastings => state.tastings;

    public IReadOnlyList<DiagnosisState> Diagnoses => state.diagnoses;

    public bool HasPendingRevision =>
        state.recipes.Count > 0 && !state.recipes[^1].hasConstructedPrototype;

    public RecipeVersionState LatestRecipe =>
        state.recipes.Count > 0 ? state.recipes[^1] : null;

    public PrototypeCigarState CreateInitialPrototype(
        IntentPreset intent,
        BlendPreset blend,
        ConstructionChoicesState choices,
        int elapsedBlock)
    {
        if (state.recipes.Count > 0)
        {
            throw new InvalidOperationException(
                "The founding recipe already exists. Revise its recorded history instead.");
        }

        RecipeVersionState recipe = BuildInitialRecipe(intent, blend, choices, elapsedBlock);
        state.recipes.Add(recipe);
        return ConstructRecipe(recipe, choices, elapsedBlock);
    }

    public PrototypeCigarState ConstructPendingRevision(
        ConstructionChoicesState choices,
        int elapsedBlock)
    {
        if (!HasPendingRevision)
        {
            throw new InvalidOperationException("No diagnosed recipe revision is waiting to be built.");
        }

        return ConstructRecipe(state.recipes[^1], choices, elapsedBlock);
    }

    public TastingRecordState TastePrototype(string prototypeId, int elapsedBlock)
    {
        PrototypeCigarState prototype = FindPrototype(prototypeId);
        if (prototype.consumedByTasting)
        {
            throw new InvalidOperationException("That study cigar has already been consumed.");
        }

        if (elapsedBlock < prototype.readyAtElapsedBlock)
        {
            throw new InvalidOperationException(
                $"The prototype needs {prototype.readyAtElapsedBlock - elapsedBlock} more calendar block(s) of rest.");
        }

        RecipeVersionState recipe = FindRecipe(prototype.recipeId, prototype.recipeVersion);
        TastingRecordState tasting = BuildTastingRecord(recipe, prototype, elapsedBlock);
        prototype.consumedByTasting = true;
        state.tastings.Add(tasting);
        return tasting.Copy();
    }

    public RecipeVersionState DiagnoseAndRevise(
        string prototypeId,
        DiagnosisKind diagnosis,
        int elapsedBlock)
    {
        PrototypeCigarState prototype = FindPrototype(prototypeId);
        if (!prototype.consumedByTasting)
        {
            throw new InvalidOperationException("Taste the prototype before recording a diagnosis.");
        }

        if (state.diagnoses.Exists(existing => existing.prototypeId == prototypeId))
        {
            throw new InvalidOperationException("This prototype already has a recorded diagnosis.");
        }

        RecipeVersionState source = FindRecipe(prototype.recipeId, prototype.recipeVersion);
        DiagnosisState diagnosisState = BuildDiagnosis(prototypeId, diagnosis, elapsedBlock);
        state.diagnoses.Add(diagnosisState);

        RecipeVersionState revision = source.Copy();
        revision.version = state.recipes.Max(recipe => recipe.version) + 1;
        revision.versionLabel = $"{source.intent.name} v{revision.version}";
        revision.createdAtElapsedBlock = elapsedBlock;
        revision.revisionRationale = diagnosisState.intendedChange;
        revision.hasConstructedPrototype = false;
        ApplyDiagnosis(revision, diagnosis);
        state.recipes.Add(revision);
        return revision.Copy();
    }

    public string CompareLatestTastings()
    {
        List<TastingRecordState> records = state.tastings
            .Where(tasting => tasting.recipeId == FoundingRecipeId)
            .OrderBy(tasting => tasting.recipeVersion)
            .ToList();

        if (records.Count < 2)
        {
            return "Two tasted recipe versions are required for comparison.";
        }

        return BuildComparison(records[^2], records[^1]);
    }

    public CigarDevelopmentSnapshot Capture()
    {
        return state.Copy();
    }

    public void Restore(CigarDevelopmentSnapshot snapshot)
    {
        if (snapshot == null || snapshot.recipes == null || snapshot.prototypes == null ||
            snapshot.tastings == null || snapshot.diagnoses == null)
        {
            throw new ArgumentException(
                "The cigar-development snapshot is invalid.",
                nameof(snapshot));
        }

        state = snapshot.Copy();
    }

    private RecipeVersionState BuildInitialRecipe(
        IntentPreset intentPreset,
        BlendPreset blendPreset,
        ConstructionChoicesState choices,
        int elapsedBlock)
    {
        BlendIntentState intent = BuildIntent(intentPreset);
        return new RecipeVersionState
        {
            recipeId = FoundingRecipeId,
            version = 1,
            versionLabel = $"{intent.name} v1",
            intent = intent,
            components = BuildComponents(blendPreset),
            vitola = "Robusto study format",
            lengthMillimeters = 124,
            ringGauge = 50,
            targetConditioning = choices.conditioning,
            targetCompression = choices.compression,
            targetArrangement = choices.fillerArrangement,
            requiredRestBlocks = 1,
            createdAtElapsedBlock = elapsedBlock,
            revisionRationale = "Founding blend study",
            hasConstructedPrototype = false,
        };
    }

    private PrototypeCigarState ConstructRecipe(
        RecipeVersionState recipe,
        ConstructionChoicesState choices,
        int elapsedBlock)
    {
        if (choices == null)
        {
            throw new ArgumentNullException(nameof(choices));
        }

        ValidateRecipeLots(recipe);
        recipe.targetConditioning = choices.conditioning;
        recipe.targetCompression = choices.compression;
        recipe.targetArrangement = choices.fillerArrangement;
        recipe.hasConstructedPrototype = true;

        PrototypeCigarState prototype = new()
        {
            id = $"{recipe.recipeId}-v{recipe.version}-study",
            recipeId = recipe.recipeId,
            recipeVersion = recipe.version,
            displayName = $"{recipe.intent.name} — Version {recipe.version} study cigar",
            choices = choices.Copy(),
            construction = BuildConstructionEvidence(recipe, choices),
            hiddenExpression = BuildExpression(recipe, choices),
            constructedAtElapsedBlock = elapsedBlock,
            readyAtElapsedBlock = elapsedBlock + recipe.requiredRestBlocks,
            consumedByTasting = false,
        };
        state.prototypes.Add(prototype);
        return prototype.Copy();
    }

    private void ValidateRecipeLots(RecipeVersionState recipe)
    {
        int total = recipe.components.Sum(component => component.proportionPercent);
        if (total != 100)
        {
            throw new InvalidOperationException("Recipe component proportions must total 100 percent.");
        }

        foreach (BlendComponentState component in recipe.components)
        {
            LeafLotState lot = inventory.LeafLots.FirstOrDefault(
                candidate => candidate.id == component.leafLotId);
            if (lot == null || lot.potential == null || lot.process == null)
            {
                throw new InvalidOperationException(
                    $"Recipe component {component.leafLotId} lacks usable provenance or process state.");
            }
        }
    }

    private RecipeVersionState FindRecipe(string recipeId, int version)
    {
        return state.recipes.FirstOrDefault(
                   recipe => recipe.recipeId == recipeId && recipe.version == version) ??
            throw new InvalidOperationException("The prototype's recipe version no longer exists.");
    }

    private PrototypeCigarState FindPrototype(string prototypeId)
    {
        if (string.IsNullOrWhiteSpace(prototypeId))
        {
            throw new ArgumentException("A prototype identifier is required.", nameof(prototypeId));
        }

        return state.prototypes.FirstOrDefault(prototype => prototype.id == prototypeId) ??
            throw new InvalidOperationException("The requested prototype does not exist.");
    }
}
}
