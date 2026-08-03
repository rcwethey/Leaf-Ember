using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeafEmber.Inventory;
using UnityEngine;

namespace LeafEmber.Cigar
{

public sealed partial class CigarDevelopmentService
{
    private static ConstructionEvidenceState BuildConstructionEvidence(
        RecipeVersionState recipe,
        ConstructionChoicesState choices)
    {
        DrawBehavior draw = choices.compression switch
        {
            CompressionChoice.Light => DrawBehavior.Open,
            CompressionChoice.Firm => DrawBehavior.Tight,
            _ => DrawBehavior.Balanced,
        };
        if (choices.fillerArrangement == FillerArrangement.OpenAirflowChannels &&
            draw == DrawBehavior.Tight)
        {
            draw = DrawBehavior.Balanced;
        }

        float diameter = (recipe.ringGauge / 64f) * 25.4f;
        float compressionWeight = ((int)choices.compression - 1) * 0.8f;
        float versionVariation = ((recipe.version % 3) - 1) * 0.08f;
        ConstructionEvidenceState evidence = new()
        {
            weightGrams = 11.8f + compressionWeight + versionVariation,
            lengthMillimeters = recipe.lengthMillimeters + ((recipe.version % 2 == 0) ? -0.6f : 0.4f),
            diameterMillimeters = diameter + ((int)choices.compression - 1) * 0.15f,
            draw = draw,
            firmness = choices.compression switch
            {
                CompressionChoice.Light => "Evenly soft with a slightly loose foot",
                CompressionChoice.Firm => "Dense through the middle with little spring",
                _ => "Consistent spring from head to foot",
            },
            burnExpectation = draw switch
            {
                DrawBehavior.Open => "Likely quick combustion; watch edge temperature",
                DrawBehavior.Tight => "Likely slow combustion; watch tunneling and relights",
                _ => "Likely steady combustion if the cigar rests evenly",
            },
            wrapperCondition = choices.conditioning switch
            {
                ConditioningChoice.Dry => "Intact, with one fine edge crack near the foot",
                ConditioningChoice.Supple => "Intact and elastic, slightly tacky at the seam",
                _ => "Intact, elastic, and evenly tensioned",
            },
            seamAndCap = choices.conditioning == ConditioningChoice.Dry
                ? "Seam visible at one turn; cap remains secure"
                : "Seam lies flat; cap is centered and secure",
            moistureDistribution = choices.conditioning switch
            {
                ConditioningChoice.Dry => "Dryer wrapper edge than the interior bunch",
                ConditioningChoice.Supple => "Slightly elevated surface moisture",
                _ => "Even by touch across wrapper and bunch",
            },
        };

        evidence.visibleEvidence.Add($"Measured {evidence.lengthMillimeters:0.0} mm × {evidence.diameterMillimeters:0.0} mm");
        evidence.visibleEvidence.Add($"Weight {evidence.weightGrams:0.00} g; draw reads {draw.ToString().ToLowerInvariant()}");
        evidence.visibleEvidence.Add(evidence.firmness);
        evidence.visibleEvidence.Add(evidence.wrapperCondition);
        return evidence;
    }

    private CigarExpressionState BuildExpression(
        RecipeVersionState recipe,
        ConstructionChoicesState choices)
    {
        Dictionary<AromaFamily, float> familyWeights = new();
        Dictionary<AromaFamily, int> familyOccurrences = new();
        foreach (AromaFamily family in Enum.GetValues(typeof(AromaFamily)))
        {
            familyWeights[family] = 0f;
            familyOccurrences[family] = 0;
        }

        float strength = 0f;
        float body = 0f;
        float intensity = 0f;
        float sweetness = 0f;
        float irritation = 0f;
        float finish = 0f;
        float combustion = 0f;

        foreach (BlendComponentState component in recipe.components)
        {
            LeafLotState lot = inventory.LeafLots.First(candidate => candidate.id == component.leafLotId);
            LeafPotentialState potential = lot.potential;
            float proportion = component.proportionPercent / 100f;
            float roleWeight = component.role switch
            {
                LeafRole.Wrapper => 1.2f,
                LeafRole.Binder => 0.8f,
                _ => 1f,
            };

            strength += (int)potential.strength * proportion;
            body += (int)potential.body * proportion;
            intensity += (int)potential.intensity * proportion;
            sweetness += (int)potential.sweetness * proportion;
            irritation += (int)potential.irritation * proportion;
            finish += (int)potential.finish * proportion;
            combustion += (int)potential.combustionSupport * proportion;
            familyWeights[potential.primaryFamily] += component.proportionPercent * roleWeight;
            familyWeights[potential.secondaryFamily] += component.proportionPercent * roleWeight * 0.55f;
            familyOccurrences[potential.primaryFamily]++;
        }

        foreach ((AromaFamily family, int occurrences) in familyOccurrences)
        {
            if (occurrences > 1)
            {
                familyWeights[family] *= 1f + ((occurrences - 1) * 0.12f);
            }
        }

        List<KeyValuePair<AromaFamily, float>> orderedFamilies = familyWeights
            .OrderByDescending(pair => pair.Value)
            .ToList();
        AromaFamily dominant = orderedFamilies[0].Key;
        AromaFamily supporting = orderedFamilies[1].Key;
        float separation = orderedFamilies[0].Value - orderedFamilies[1].Value;

        float smoothness = 2f - irritation;
        if (choices.conditioning == ConditioningChoice.Balanced)
        {
            smoothness += 0.25f;
        }
        else if (choices.conditioning == ConditioningChoice.Dry)
        {
            smoothness -= 0.45f;
            intensity += 0.2f;
        }

        if (choices.compression == CompressionChoice.Firm)
        {
            intensity += 0.2f;
            smoothness -= 0.25f;
        }
        else if (choices.compression == CompressionChoice.Light)
        {
            body -= 0.2f;
        }

        string interaction;
        string progression;
        if (familyOccurrences[dominant] > 1)
        {
            interaction = "Reinforcement makes the dominant family more prominent.";
            progression = separation < 16f ? "Layered, then gradually building" : "Dominant and steadily building";
        }
        else if (separation < 10f)
        {
            interaction = "Closely weighted families layer rather than one fully masking the other.";
            progression = "Layered with a changing middle";
        }
        else
        {
            interaction = "The leading component masks part of the quieter supporting family.";
            progression = "Direct opening with a quieter supporting finish";
        }

        string combustionCharacter = choices.compression switch
        {
            CompressionChoice.Light => "Fast smoke delivery with a warmer final portion",
            CompressionChoice.Firm when choices.fillerArrangement != FillerArrangement.OpenAirflowChannels =>
                "Restricted smoke delivery with heat rising during corrective puffing",
            _ => combustion >= 1.25f
                ? "Steady smoke delivery and moderate temperature"
                : "Generally steady with some sensitivity to smoking pace",
        };

        return new CigarExpressionState
        {
            strength = ToStrengthBand(strength),
            body = ToSensoryBand(body),
            intensity = ToSensoryBand(intensity),
            sweetness = ToSensoryBand(sweetness),
            smoothness = ToSensoryBand(smoothness),
            finish = ToSensoryBand(finish),
            dominantFamily = dominant,
            supportingFamily = supporting,
            progression = progression,
            combustionCharacter = combustionCharacter,
            interaction = interaction,
        };
    }

    private static TastingRecordState BuildTastingRecord(
        RecipeVersionState recipe,
        PrototypeCigarState prototype,
        int elapsedBlock)
    {
        CigarExpressionState expression = prototype.hiddenExpression;
        string dominant = CigarDevelopmentText.AromaFamilyName(expression.dominantFamily);
        string supporting = CigarDevelopmentText.AromaFamilyName(expression.supportingFamily);
        string draw = prototype.construction.draw.ToString().ToLowerInvariant();
        string finish = expression.finish switch
        {
            SensoryBand.Low => "brief and quiet",
            SensoryBand.High => "long and persistent",
            _ => "moderate in length",
        };

        TastingRecordState record = new()
        {
            id = $"{prototype.id}-tasting",
            prototypeId = prototype.id,
            recipeId = prototype.recipeId,
            recipeVersion = prototype.recipeVersion,
            tastedAtElapsedBlock = elapsedBlock,
            observedStrength = expression.strength,
            observedBody = expression.body,
            observedIntensity = expression.intensity,
            observedDominantFamily = expression.dominantFamily,
            observedSupportingFamily = expression.supportingFamily,
            observedFinish = finish,
            intentComparison = BuildIntentComparison(recipe.intent, expression),
            independentFeedbackSource = "Visiting maker's note",
            independentFeedback =
                $"The visiting maker also records {supporting}, but groups the {dominant} impression " +
                $"more broadly and calls the finish {finish}. This is another perspective, not a correction.",
        };

        record.stages.Add(new TastingStageState
        {
            stage = "Pre-light",
            constructionObservation = $"Wrapper is {prototype.construction.wrapperCondition.ToLowerInvariant()}; pre-light draw feels {draw}.",
            sensoryObservation = $"A restrained {supporting} suggestion is present at the wrapper and foot.",
            confidence = "Moderate",
        });
        record.stages.Add(new TastingStageState
        {
            stage = "Opening",
            constructionObservation = $"{prototype.construction.burnExpectation}.",
            sensoryObservation = $"{dominant} presents first; {supporting} remains quieter behind it.",
            confidence = "Moderate",
        });
        record.stages.Add(new TastingStageState
        {
            stage = "Middle",
            constructionObservation = $"Smoke delivery is {expression.combustionCharacter.ToLowerInvariant()}.",
            sensoryObservation = $"The middle reads as {expression.progression.ToLowerInvariant()}. {expression.interaction}",
            confidence = "Moderate-high",
        });
        record.stages.Add(new TastingStageState
        {
            stage = "Final portion",
            constructionObservation = prototype.construction.draw == DrawBehavior.Balanced
                ? "Draw remains stable; temperature rises gradually."
                : "The initial draw behavior becomes more consequential as temperature rises.",
            sensoryObservation = expression.smoothness == SensoryBand.Low
                ? $"Sharpness competes with the {dominant} impression."
                : $"{dominant} remains clear while {supporting} recedes.",
            confidence = "Moderate",
        });
        record.stages.Add(new TastingStageState
        {
            stage = "Finish",
            constructionObservation = "No additional structural evidence after setting the cigar down.",
            sensoryObservation = $"The finish is {finish}, led by {dominant} with a trace of {supporting}.",
            confidence = "Moderate-high",
        });
        return record;
    }

    private string BuildComparison(TastingRecordState earlier, TastingRecordState later)
    {
        PrototypeCigarState earlierPrototype = FindPrototype(earlier.prototypeId);
        PrototypeCigarState laterPrototype = FindPrototype(later.prototypeId);
        StringBuilder comparison = new();
        comparison.AppendLine($"VERSION {earlier.recipeVersion} ↔ VERSION {later.recipeVersion}");
        comparison.AppendLine();
        comparison.AppendLine(
            $"Construction: {earlierPrototype.construction.draw} draw / {earlierPrototype.construction.firmness} " +
            $"versus {laterPrototype.construction.draw} draw / {laterPrototype.construction.firmness}.");
        comparison.AppendLine();
        comparison.AppendLine(
            $"Strength and body: V{earlier.recipeVersion} {earlier.observedStrength}, {earlier.observedBody} body; " +
            $"V{later.recipeVersion} {later.observedStrength}, {later.observedBody} body.");
        comparison.AppendLine();
        comparison.AppendLine(
            $"Character: V{earlier.recipeVersion} led with " +
            $"{CigarDevelopmentText.AromaFamilyName(earlier.observedDominantFamily)} over " +
            $"{CigarDevelopmentText.AromaFamilyName(earlier.observedSupportingFamily)}; " +
            $"V{later.recipeVersion} led with " +
            $"{CigarDevelopmentText.AromaFamilyName(later.observedDominantFamily)} over " +
            $"{CigarDevelopmentText.AromaFamilyName(later.observedSupportingFamily)}.");
        comparison.AppendLine();
        comparison.AppendLine($"V{earlier.recipeVersion} intent comparison: {earlier.intentComparison}");
        comparison.AppendLine($"V{later.recipeVersion} intent comparison: {later.intentComparison}");
        comparison.AppendLine();
        comparison.Append(
            "This comparison exposes tradeoffs. It does not select a winner or collapse technical evidence, intent fidelity, and preference into one score.");
        return comparison.ToString();
    }

    private static string BuildIntentComparison(
        BlendIntentState intent,
        CigarExpressionState expression)
    {
        string strength = CompareOrdinal(
            (int)expression.strength,
            (int)intent.desiredStrength,
            "lighter than intended",
            "aligned with the intended strength",
            "fuller than intended");
        string body = CompareOrdinal(
            (int)expression.body,
            (int)intent.desiredBody,
            "lighter-bodied than intended",
            "aligned with the intended body",
            "heavier-bodied than intended");
        string family = expression.dominantFamily == intent.dominantFamily
            ? "The leading aroma family supports the intent."
            : $"The leading family is {CigarDevelopmentText.AromaFamilyName(expression.dominantFamily)}, " +
              $"not the intended {CigarDevelopmentText.AromaFamilyName(intent.dominantFamily)}.";
        return $"The cigar is {strength} and {body}. {family}";
    }

    private static string CompareOrdinal(
        int observed,
        int intended,
        string lower,
        string aligned,
        string higher)
    {
        return observed < intended ? lower : observed > intended ? higher : aligned;
    }

    private static StrengthBand ToStrengthBand(float value)
    {
        return (StrengthBand)Mathf.Clamp(Mathf.RoundToInt(value), 0, 2);
    }

    private static SensoryBand ToSensoryBand(float value)
    {
        return (SensoryBand)Mathf.Clamp(Mathf.RoundToInt(value), 0, 2);
    }
}
}
