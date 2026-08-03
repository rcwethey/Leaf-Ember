using System;
using LeafEmber.Cigar;
using LeafEmber.Inventory;
using NUnit.Framework;

namespace LeafEmber.Tests
{

public sealed class CigarDevelopmentServiceTests
{
    private InventoryService inventory;
    private CigarDevelopmentService development;

    [SetUp]
    public void SetUp()
    {
        inventory = new InventoryService();
        development = new CigarDevelopmentService(inventory);
    }

    [Test]
    public void CreateInitialPrototype_PreservesIntentRecipeAndHardEvidence()
    {
        PrototypeCigarState prototype = CreateInitial();

        Assert.That(development.Recipes, Has.Count.EqualTo(1));
        Assert.That(development.Recipes[0].intent.name, Is.EqualTo("Quiet Workshop"));
        Assert.That(development.Recipes[0].components, Has.Count.EqualTo(4));
        Assert.That(prototype.construction.visibleEvidence, Is.Not.Empty);
        Assert.That(prototype.construction.weightGrams, Is.GreaterThan(0f));
        Assert.That(prototype.hiddenExpression, Is.Not.Null);
        Assert.That(prototype.readyAtElapsedBlock, Is.EqualTo(1));
    }

    [Test]
    public void TastePrototype_RequiresRestAndProducesFivePerspectiveStages()
    {
        PrototypeCigarState prototype = CreateInitial();

        Assert.Throws<InvalidOperationException>(
            () => development.TastePrototype(prototype.id, 0));

        TastingRecordState tasting = development.TastePrototype(prototype.id, 1);

        Assert.That(tasting.stages, Has.Count.EqualTo(5));
        Assert.That(tasting.intentComparison, Is.Not.Empty);
        Assert.That(tasting.independentFeedback, Does.Contain("another perspective"));
        Assert.That(development.Prototypes[0].consumedByTasting, Is.True);
    }

    [Test]
    public void DiagnoseAndRevise_PreservesVersionHistory()
    {
        PrototypeCigarState prototype = CreateInitial();
        development.TastePrototype(prototype.id, 1);

        RecipeVersionState revision = development.DiagnoseAndRevise(
            prototype.id,
            DiagnosisKind.ComponentDominance,
            2);

        Assert.That(development.Recipes, Has.Count.EqualTo(2));
        Assert.That(development.Recipes[0].version, Is.EqualTo(1));
        Assert.That(revision.version, Is.EqualTo(2));
        Assert.That(revision.hasConstructedPrototype, Is.False);
        Assert.That(development.HasPendingRevision, Is.True);
        Assert.That(
            revision.components.Find(component => component.leafLotId == "ortega-valley-viso")
                .proportionPercent,
            Is.EqualTo(15));
    }

    [Test]
    public void TwoTastedVersions_CompareTradeoffsWithoutSelectingWinner()
    {
        PrototypeCigarState first = CreateInitial();
        development.TastePrototype(first.id, 1);
        development.DiagnoseAndRevise(first.id, DiagnosisKind.BunchCompression, 2);
        PrototypeCigarState second = development.ConstructPendingRevision(
            new ConstructionChoicesState
            {
                conditioning = ConditioningChoice.Balanced,
                compression = CompressionChoice.Balanced,
                fillerArrangement = FillerArrangement.OpenAirflowChannels,
            },
            2);
        development.TastePrototype(second.id, 3);

        string comparison = development.CompareLatestTastings();

        Assert.That(comparison, Does.Contain("VERSION 1"));
        Assert.That(comparison, Does.Contain("VERSION 2"));
        Assert.That(comparison, Does.Contain("tradeoffs"));
        Assert.That(comparison, Does.Contain("does not select a winner"));
    }

    [Test]
    public void CaptureAndRestore_PreserveDevelopmentHistoryDefensively()
    {
        PrototypeCigarState prototype = CreateInitial();
        development.TastePrototype(prototype.id, 1);
        CigarDevelopmentSnapshot snapshot = development.Capture();
        CigarDevelopmentService restored = new(inventory);

        restored.Restore(snapshot);
        snapshot.recipes[0].versionLabel = "Changed externally";

        Assert.That(restored.Recipes, Has.Count.EqualTo(1));
        Assert.That(restored.Tastings, Has.Count.EqualTo(1));
        Assert.That(restored.Recipes[0].versionLabel, Is.Not.EqualTo("Changed externally"));
    }

    [Test]
    public void SameInputs_ProduceSameConstructionAndExpression()
    {
        PrototypeCigarState first = CreateInitial();
        CigarDevelopmentService other = new(new InventoryService());
        PrototypeCigarState second = other.CreateInitialPrototype(
            IntentPreset.QuietWorkshop,
            BlendPreset.EstateForward,
            BalancedChoices(),
            0);

        Assert.That(second.construction.weightGrams, Is.EqualTo(first.construction.weightGrams));
        Assert.That(second.hiddenExpression.strength, Is.EqualTo(first.hiddenExpression.strength));
        Assert.That(
            second.hiddenExpression.dominantFamily,
            Is.EqualTo(first.hiddenExpression.dominantFamily));
    }

    private PrototypeCigarState CreateInitial()
    {
        return development.CreateInitialPrototype(
            IntentPreset.QuietWorkshop,
            BlendPreset.EstateForward,
            BalancedChoices(),
            0);
    }

    private static ConstructionChoicesState BalancedChoices()
    {
        return new ConstructionChoicesState
        {
            conditioning = ConditioningChoice.Balanced,
            compression = CompressionChoice.Balanced,
            fillerArrangement = FillerArrangement.ParallelFolds,
        };
    }
}
}
