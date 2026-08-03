using System.Collections.Generic;

namespace LeafEmber.Cigar
{

public sealed partial class CigarDevelopmentService
{
    private static BlendIntentState BuildIntent(IntentPreset preset)
    {
        return preset switch
        {
            IntentPreset.QuietWorkshop => new BlendIntentState
            {
                name = CigarDevelopmentText.IntentName(preset),
                audience = "Patient craft-focused smoker",
                occasion = "Quiet late-afternoon workshop pause",
                desiredStrength = StrengthBand.Medium,
                desiredBody = SensoryBand.Medium,
                desiredIntensity = SensoryBand.Medium,
                dominantFamily = AromaFamily.Wood,
                supportingFamily = AromaFamily.RoastedAndNutty,
                desiredProgression = "Gradual and calm",
                desiredFinish = "Clean, medium-length, and smooth",
                productionConstraint = "Use the estate seco as a recognizable house voice",
            },
            IntentPreset.RainOnRedEarth => new BlendIntentState
            {
                name = CigarDevelopmentText.IntentName(preset),
                audience = "Experienced smoker seeking structure",
                occasion = "Slow evening during heavy rain",
                desiredStrength = StrengthBand.Full,
                desiredBody = SensoryBand.High,
                desiredIntensity = SensoryBand.High,
                dominantFamily = AromaFamily.EarthAndMineral,
                supportingFamily = AromaFamily.Spice,
                desiredProgression = "Building",
                desiredFinish = "Long and dry without aggressive heat",
                productionConstraint = "Preserve clarity between earth and spice",
            },
            _ => new BlendIntentState
            {
                name = CigarDevelopmentText.IntentName(IntentPreset.AfterSupper),
                audience = "Smoker who prefers rounded aromatic cigars",
                occasion = "Relaxed conversation after supper",
                desiredStrength = StrengthBand.Medium,
                desiredBody = SensoryBand.Medium,
                desiredIntensity = SensoryBand.High,
                dominantFamily = AromaFamily.RoastedAndNutty,
                supportingFamily = AromaFamily.SweetAndBaked,
                desiredProgression = "Layered middle with a gentle build",
                desiredFinish = "Lingering roast with restrained sweetness",
                productionConstraint = "Avoid turning aromatic richness into heavy strength",
            },
        };
    }

    private static List<BlendComponentState> BuildComponents(BlendPreset preset)
    {
        (int estate, int viso, int binder, int wrapper) = preset switch
        {
            BlendPreset.EstateForward => (55, 25, 12, 8),
            BlendPreset.SpiceBridge => (35, 45, 12, 8),
            _ => (45, 30, 15, 10),
        };

        return new List<BlendComponentState>
        {
            new()
            {
                leafLotId = "finca-pilot-seco",
                role = LeafRole.Filler,
                proportionPercent = estate,
                placement = "Outer filler folds for accessible combustion",
            },
            new()
            {
                leafLotId = "ortega-valley-viso",
                role = LeafRole.Filler,
                proportionPercent = viso,
                placement = "Interior filler for structure and a gradual build",
            },
            new()
            {
                leafLotId = "san-jeronimo-binder",
                role = LeafRole.Binder,
                proportionPercent = binder,
                placement = "Single structural binder around the bunch",
            },
            new()
            {
                leafLotId = "las-lomas-colorado-wrapper",
                role = LeafRole.Wrapper,
                proportionPercent = wrapper,
                placement = "Outer wrapper selected for elasticity and clean finish",
            },
        };
    }

    private static DiagnosisState BuildDiagnosis(
        string prototypeId,
        DiagnosisKind diagnosis,
        int elapsedBlock)
    {
        (string reasoning, string intendedChange) = diagnosis switch
        {
            DiagnosisKind.ComponentDominance => (
                "One filler may be masking the quieter material rather than allowing layering.",
                "Reduce the Ortega viso by ten points and return those points to the estate seco."),
            DiagnosisKind.TobaccoCondition => (
                "Leaf condition may have changed handling, wrapper integrity, and sensory sharpness.",
                "Set the next construction target to balanced conditioning."),
            DiagnosisKind.BunchCompression => (
                "Draw and temperature evidence suggest that bunch density deserves a controlled retest.",
                "Set balanced compression and open airflow channels for the next version."),
            DiagnosisKind.InsufficientRest => (
                "The components may not have recovered enough from construction to present coherently.",
                "Add one calendar block of prototype rest before the next tasting."),
            _ => (
                "The format may be concentrating the blend or changing its combustion arc.",
                "Test the same composition at a slightly narrower 48 ring gauge."),
        };

        return new DiagnosisState
        {
            prototypeId = prototypeId,
            hypothesis = diagnosis,
            reasoning = reasoning,
            intendedChange = intendedChange,
            recordedAtElapsedBlock = elapsedBlock,
        };
    }

    private static void ApplyDiagnosis(
        RecipeVersionState revision,
        DiagnosisKind diagnosis)
    {
        switch (diagnosis)
        {
            case DiagnosisKind.ComponentDominance:
                BlendComponentState estate = revision.components.Find(
                    component => component.leafLotId == "finca-pilot-seco");
                BlendComponentState viso = revision.components.Find(
                    component => component.leafLotId == "ortega-valley-viso");
                int transferable = System.Math.Min(10, viso.proportionPercent - 5);
                viso.proportionPercent -= transferable;
                estate.proportionPercent += transferable;
                break;
            case DiagnosisKind.TobaccoCondition:
                revision.targetConditioning = ConditioningChoice.Balanced;
                break;
            case DiagnosisKind.BunchCompression:
                revision.targetCompression = CompressionChoice.Balanced;
                revision.targetArrangement = FillerArrangement.OpenAirflowChannels;
                break;
            case DiagnosisKind.InsufficientRest:
                revision.requiredRestBlocks += 1;
                break;
            case DiagnosisKind.CombustionAndFormat:
                revision.ringGauge = 48;
                break;
        }
    }
}
}
