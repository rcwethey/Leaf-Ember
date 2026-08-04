using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeafEmber.Cigar;
using LeafEmber.Events;
using LeafEmber.Prototype.Events;
using LeafEmber.Time;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeafEmber.Prototype.UI
{

public sealed class CigarDevelopmentView : MonoBehaviour
{
    private enum Page
    {
        None,
        Notebook,
        Intent,
        Blend,
        Construction,
        Review,
        ConstructionResult,
        TastingSelection,
        TastingResult,
        Diagnosis,
        Comparison,
        Message,
    }

    private IEventBus eventBus;
    private ICalendarService calendar;
    private ICigarDevelopmentService development;
    private Page page;
    private IntentPreset selectedIntent = IntentPreset.QuietWorkshop;
    private BlendPreset selectedBlend = BlendPreset.EstateForward;
    private ConstructionChoicesState choices = BalancedChoices();
    private int constructionField;
    private int selectedPrototypeIndex;
    private DiagnosisKind selectedDiagnosis = DiagnosisKind.ComponentDominance;
    private PrototypeCigarState latestPrototype;
    private TastingRecordState latestTasting;
    private string messageTitle;
    private string messageBody;
    private GUIStyle titleStyle;
    private GUIStyle bodyStyle;
    private GUIStyle footerStyle;
    private Vector2 scrollPosition;
    private Page renderedPage;

    public bool IsOpen => page != Page.None;

    public void Initialize(
        IEventBus gameEventBus,
        ICalendarService calendarService,
        ICigarDevelopmentService developmentService)
    {
        eventBus = gameEventBus;
        calendar = calendarService;
        development = developmentService;
        eventBus.Subscribe<CigarWorkbenchRequestedEvent>(OnWorkbenchRequested);
        eventBus.Subscribe<TastingTableRequestedEvent>(OnTastingRequested);
    }

    private void OnDestroy()
    {
        if (eventBus == null)
        {
            return;
        }

        eventBus.Unsubscribe<CigarWorkbenchRequestedEvent>(OnWorkbenchRequested);
        eventBus.Unsubscribe<TastingTableRequestedEvent>(OnTastingRequested);
    }

    private void Update()
    {
        if (!IsOpen || Keyboard.current == null)
        {
            return;
        }

        Keyboard keyboard = Keyboard.current;
        if (keyboard.escapeKey.wasPressedThisFrame)
        {
            Close();
            return;
        }

        bool previous = keyboard.leftArrowKey.wasPressedThisFrame || keyboard.aKey.wasPressedThisFrame;
        bool next = keyboard.rightArrowKey.wasPressedThisFrame || keyboard.dKey.wasPressedThisFrame;
        bool up = keyboard.upArrowKey.wasPressedThisFrame || keyboard.wKey.wasPressedThisFrame;
        bool down = keyboard.downArrowKey.wasPressedThisFrame || keyboard.sKey.wasPressedThisFrame;
        bool confirm = keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame;
        bool back = keyboard.backspaceKey.wasPressedThisFrame;

        switch (page)
        {
            case Page.Intent:
                if (previous) selectedIntent = Cycle(selectedIntent, -1);
                if (next) selectedIntent = Cycle(selectedIntent, 1);
                if (confirm) page = Page.Blend;
                break;
            case Page.Blend:
                if (previous) selectedBlend = Cycle(selectedBlend, -1);
                if (next) selectedBlend = Cycle(selectedBlend, 1);
                if (back) page = Page.Intent;
                if (confirm) page = Page.Construction;
                break;
            case Page.Construction:
                if (up) constructionField = Mathf.Max(0, constructionField - 1);
                if (down) constructionField = Mathf.Min(2, constructionField + 1);
                if (previous) AdjustConstruction(-1);
                if (next) AdjustConstruction(1);
                if (back && !development.HasPendingRevision) page = Page.Blend;
                if (confirm) page = Page.Review;
                break;
            case Page.Review:
                if (back) page = Page.Construction;
                if (confirm) CommitConstruction();
                break;
            case Page.TastingSelection:
                AdjustPrototypeSelection(up, down);
                if (confirm) CommitTasting();
                break;
            case Page.TastingResult:
                if (keyboard.dKey.wasPressedThisFrame ||
                    (confirm && development.Tastings.Count < 2))
                {
                    page = Page.Diagnosis;
                }
                else if (confirm && development.Tastings.Count >= 2)
                {
                    page = Page.Comparison;
                }
                break;
            case Page.Diagnosis:
                if (previous) selectedDiagnosis = Cycle(selectedDiagnosis, -1);
                if (next) selectedDiagnosis = Cycle(selectedDiagnosis, 1);
                if (confirm) CommitDiagnosis();
                break;
            case Page.Notebook:
                if (confirm && development.Tastings.Count >= 2)
                {
                    page = Page.Comparison;
                }
                break;
            case Page.ConstructionResult:
            case Page.Comparison:
            case Page.Message:
                break;
        }
    }

    private void OnGUI()
    {
        if (!IsOpen)
        {
            return;
        }

        EnsureStyles();
        float width = Mathf.Min(920f, Screen.width - 60f);
        float height = Mathf.Min(690f, Screen.height - 70f);
        Rect panel = new((Screen.width - width) * 0.5f, (Screen.height - height) * 0.5f, width, height);
        GUI.Box(panel, GUIContent.none);
        GUI.Label(
            new Rect(panel.x + 32f, panel.y + 24f, panel.width - 64f, 38f),
            PageTitle(),
            titleStyle);
        if (renderedPage != page)
        {
            renderedPage = page;
            scrollPosition = Vector2.zero;
        }

        string body = PageBody();
        Rect bodyArea = new(
            panel.x + 32f,
            panel.y + 76f,
            panel.width - 64f,
            panel.height - 142f);
        float bodyWidth = bodyArea.width - 22f;
        float contentHeight = Mathf.Max(
            bodyArea.height,
            bodyStyle.CalcHeight(new GUIContent(body), bodyWidth) + 8f);
        scrollPosition = GUI.BeginScrollView(
            bodyArea,
            scrollPosition,
            new Rect(0f, 0f, bodyWidth, contentHeight));
        GUI.Label(new Rect(0f, 0f, bodyWidth, contentHeight), body, bodyStyle);
        GUI.EndScrollView();
        GUI.Label(
            new Rect(panel.x + 32f, panel.yMax - 48f, panel.width - 64f, 26f),
            PageFooter(),
            footerStyle);
    }

    private void OnWorkbenchRequested(CigarWorkbenchRequestedEvent request)
    {
        if (development.Recipes.Count == 0)
        {
            selectedIntent = IntentPreset.QuietWorkshop;
            selectedBlend = BlendPreset.EstateForward;
            choices = BalancedChoices();
            constructionField = 0;
            page = Page.Intent;
            return;
        }

        if (development.HasPendingRevision)
        {
            RecipeVersionState pending = development.LatestRecipe;
            choices = new ConstructionChoicesState
            {
                conditioning = pending.targetConditioning,
                compression = pending.targetCompression,
                fillerArrangement = pending.targetArrangement,
            };
            constructionField = 0;
            page = Page.Construction;
            return;
        }

        TastingRecordState unresolved = development.Tastings.LastOrDefault(
            tasting => !development.Diagnoses.Any(
                diagnosis => diagnosis.prototypeId == tasting.prototypeId));
        if (unresolved != null)
        {
            latestTasting = unresolved;
            page = Page.TastingResult;
            return;
        }

        page = Page.Notebook;
    }

    private void OnTastingRequested(TastingTableRequestedEvent request)
    {
        List<PrototypeCigarState> available = AvailablePrototypes();
        if (available.Count == 0)
        {
            messageTitle = "No prototype is ready";
            PrototypeCigarState resting = development.Prototypes
                .FirstOrDefault(prototype => !prototype.consumedByTasting);
            messageBody = resting == null
                ? "Construct a study cigar at the workshop before beginning a tasting."
                : $"{resting.displayName} needs " +
                  $"{Mathf.Max(0, resting.readyAtElapsedBlock - calendar.ElapsedBlocks)} more calendar block(s) of rest.";
            page = Page.Message;
            return;
        }

        selectedPrototypeIndex = Mathf.Clamp(selectedPrototypeIndex, 0, available.Count - 1);
        page = Page.TastingSelection;
    }

    private void CommitConstruction()
    {
        try
        {
            int beforeWork = calendar.ElapsedBlocks;
            latestPrototype = development.HasPendingRevision
                ? development.ConstructPendingRevision(choices, beforeWork)
                : development.CreateInitialPrototype(selectedIntent, selectedBlend, choices, beforeWork);
            calendar.AdvanceBlocks(1, $"Construct {latestPrototype.displayName}");
            page = Page.ConstructionResult;
        }
        catch (Exception exception)
        {
            messageTitle = "Construction could not begin";
            messageBody = exception.Message;
            page = Page.Message;
        }
    }

    private void CommitTasting()
    {
        List<PrototypeCigarState> available = AvailablePrototypes();
        if (available.Count == 0)
        {
            return;
        }

        try
        {
            PrototypeCigarState selected = available[selectedPrototypeIndex];
            latestTasting = development.TastePrototype(selected.id, calendar.ElapsedBlocks);
            calendar.AdvanceBlocks(1, $"Taste {selected.displayName}");
            page = Page.TastingResult;
        }
        catch (Exception exception)
        {
            messageTitle = "Tasting could not begin";
            messageBody = exception.Message;
            page = Page.Message;
        }
    }

    private void CommitDiagnosis()
    {
        if (latestTasting == null)
        {
            return;
        }

        try
        {
            RecipeVersionState revision = development.DiagnoseAndRevise(
                latestTasting.prototypeId,
                selectedDiagnosis,
                calendar.ElapsedBlocks);
            messageTitle = $"{revision.versionLabel} entered in the notebook";
            messageBody =
                $"Hypothesis: {CigarDevelopmentText.DiagnosisName(selectedDiagnosis)}\n\n" +
                $"Revision: {revision.revisionRationale}\n\n" +
                "Return to the workshop. The notebook preserves the earlier version and will build this revision as a separate study cigar.";
            page = Page.Message;
        }
        catch (Exception exception)
        {
            messageTitle = "Diagnosis could not be recorded";
            messageBody = exception.Message;
            page = Page.Message;
        }
    }

    private string PageTitle()
    {
        return page switch
        {
            Page.Notebook => "Recipe notebook",
            Page.Intent => "1 — Define the intended experience",
            Page.Blend => "2 — Select a starting composition",
            Page.Construction => development.HasPendingRevision
                ? $"Construct {development.LatestRecipe.versionLabel}"
                : "3 — Set construction decisions",
            Page.Review => "Review the study specification",
            Page.ConstructionResult => "Immediate construction evidence",
            Page.TastingSelection => "Select a rested prototype",
            Page.TastingResult => "Focused tasting journal",
            Page.Diagnosis => "Record a causal hypothesis",
            Page.Comparison => "Version comparison",
            Page.Message => messageTitle,
            _ => "Cigar development",
        };
    }

    private string PageBody()
    {
        return page switch
        {
            Page.Notebook => FormatNotebook(),
            Page.Intent => FormatIntent(),
            Page.Blend => FormatBlend(),
            Page.Construction => FormatConstructionChoices(),
            Page.Review => FormatReview(),
            Page.ConstructionResult => FormatConstructionEvidence(latestPrototype),
            Page.TastingSelection => FormatTastingSelection(),
            Page.TastingResult => FormatTasting(latestTasting),
            Page.Diagnosis => FormatDiagnosis(),
            Page.Comparison => development.CompareLatestTastings(),
            Page.Message => messageBody,
            _ => string.Empty,
        };
    }

    private string PageFooter()
    {
        return page switch
        {
            Page.Intent or Page.Blend => "Left/Right: choose  •  Enter: continue  •  Backspace: back  •  Esc: close",
            Page.Construction => "Up/Down: field  •  Left/Right: adjust  •  Enter: review  •  Esc: close",
            Page.Review => "Enter: commit one work block and construct  •  Backspace: revise  •  Esc: cancel",
            Page.TastingSelection => "Up/Down: select  •  Enter: consume one cigar and one work block  •  Esc: cancel",
            Page.TastingResult when development.Tastings.Count >= 2 =>
                "Enter: compare latest versions  •  D: diagnose another revision  •  Esc: close",
            Page.TastingResult => "Enter: diagnose and create a revision  •  Esc: close",
            Page.Diagnosis => "Left/Right: hypothesis  •  Enter: record and create next version  •  Esc: cancel",
            Page.Notebook when development.Tastings.Count >= 2 =>
                "Enter: reopen latest version comparison  •  Esc: close",
            _ => "Esc: close",
        };
    }

    private string FormatNotebook()
    {
        RecipeVersionState latest = development.LatestRecipe;
        int tastedCount = development.Tastings.Count;
        return
            $"{latest.versionLabel}\n" +
            $"Intent: {latest.intent.name}\n" +
            $"Format: {latest.vitola}, {latest.lengthMillimeters} mm, {latest.ringGauge} ring gauge\n" +
            $"Versions preserved: {development.Recipes.Count}\n" +
            $"Study cigars constructed: {development.Prototypes.Count}\n" +
            $"Tasting records: {tastedCount}\n\n" +
            "No revision is waiting to be built. Visit the courtyard tasting table with a rested study cigar. " +
            "A tasting diagnosis creates the next version without erasing this one.";
    }

    private string FormatIntent()
    {
        return
            $"◀  {CigarDevelopmentText.IntentName(selectedIntent)}  ▶\n\n" +
            CigarDevelopmentText.IntentDescription(selectedIntent) +
            "\n\nWHY THIS MATTERS\n" +
            "The intent is your reference point. A cigar can be technically sound and still miss the desired occasion, strength, progression, or house character.";
    }

    private string FormatBlend()
    {
        return
            $"◀  {CigarDevelopmentText.BlendName(selectedBlend)}  ▶\n\n" +
            CigarDevelopmentText.BlendDescription(selectedBlend) +
            "\n\n" +
            CigarDevelopmentText.BlendRationale(selectedBlend) +
            "\n\nEvery component references a named lot. This is a prediction—not a flavor answer. Construction, rest, and tasting turn it into evidence.";
    }

    private string FormatConstructionChoices()
    {
        string prefix0 = constructionField == 0 ? "▶" : " ";
        string prefix1 = constructionField == 1 ? "▶" : " ";
        string prefix2 = constructionField == 2 ? "▶" : " ";
        return
            $"{prefix0} Conditioning:  ◀ {choices.conditioning} ▶\n\n" +
            $"    {CigarDevelopmentText.ConditioningEffect(choices.conditioning)}\n\n" +
            $"{prefix1} Compression:   ◀ {choices.compression} ▶\n\n" +
            $"    {CigarDevelopmentText.CompressionEffect(choices.compression)}\n\n" +
            $"{prefix2} Filler form:   ◀ {FormatArrangement(choices.fillerArrangement)} ▶\n\n" +
            $"    {CigarDevelopmentText.ArrangementEffect(choices.fillerArrangement)}\n\n" +
            "These are physical decisions, not a timing challenge. Their separate consequences will appear as measurements and observations—not a rolling score.";
    }

    private string FormatReview()
    {
        RecipeVersionState pending = development.HasPendingRevision ? development.LatestRecipe : null;
        string intent = pending?.intent.name ?? CigarDevelopmentText.IntentName(selectedIntent);
        string blend = pending == null
            ? CigarDevelopmentText.BlendName(selectedBlend)
            : $"Version {pending.version} inherited composition";
        return
            $"Intent: {intent}\n" +
            $"Composition: {blend}\n" +
            $"Conditioning: {choices.conditioning}\n" +
            $"Compression: {choices.compression}\n" +
            $"Filler arrangement: {FormatArrangement(choices.fillerArrangement)}\n\n" +
            "Construction will consume one work block. The resulting evidence reports dimensions, weight, draw, firmness, moisture, wrapper, seam, and cap separately.";
    }

    private static string FormatConstructionEvidence(PrototypeCigarState prototype)
    {
        if (prototype == null)
        {
            return "No construction record is available.";
        }

        ConstructionEvidenceState evidence = prototype.construction;
        StringBuilder body = new();
        body.AppendLine(prototype.displayName);
        body.AppendLine();
        body.AppendLine($"Dimensions: {evidence.lengthMillimeters:0.0} × {evidence.diameterMillimeters:0.0} mm");
        body.AppendLine($"Weight: {evidence.weightGrams:0.00} g");
        body.AppendLine($"Draw: {evidence.draw}");
        body.AppendLine($"Firmness: {evidence.firmness}");
        body.AppendLine($"Wrapper: {evidence.wrapperCondition}");
        body.AppendLine($"Seam and cap: {evidence.seamAndCap}");
        body.AppendLine($"Moisture: {evidence.moistureDistribution}");
        body.AppendLine($"Combustion expectation: {evidence.burnExpectation}");
        body.AppendLine();
        body.AppendLine($"Ready for tasting at elapsed block {prototype.readyAtElapsedBlock}. The clock has advanced one block.");
        body.AppendLine();
        body.Append("This is evidence, not a rolling score. Smoking remains necessary to evaluate the design intent.");
        return body.ToString();
    }

    private string FormatTastingSelection()
    {
        List<PrototypeCigarState> available = AvailablePrototypes();
        StringBuilder body = new();
        body.AppendLine("A focused session consumes the selected cigar and one calendar block.");
        body.AppendLine();
        for (int index = 0; index < available.Count; index++)
        {
            body.AppendLine($"{(index == selectedPrototypeIndex ? "▶" : " ")} {available[index].displayName}");
        }

        body.AppendLine();
        body.Append("The journal will preserve staged construction and sensory observations with confidence, then compare them with the recorded intent.");
        return body.ToString();
    }

    private static string FormatTasting(TastingRecordState tasting)
    {
        if (tasting == null)
        {
            return "No tasting record is available.";
        }

        StringBuilder body = new();
        foreach (TastingStageState stage in tasting.stages)
        {
            body.AppendLine(stage.stage.ToUpperInvariant());
            body.AppendLine($"Construction: {stage.constructionObservation}");
            body.AppendLine($"Perception ({stage.confidence} confidence): {stage.sensoryObservation}");
            body.AppendLine();
        }

        body.AppendLine("INTENT COMPARISON");
        body.AppendLine(tasting.intentComparison);
        body.AppendLine();
        body.AppendLine(tasting.independentFeedbackSource.ToUpperInvariant());
        body.Append(tasting.independentFeedback);
        return body.ToString();
    }

    private string FormatDiagnosis()
    {
        return
            $"◀  {CigarDevelopmentText.DiagnosisName(selectedDiagnosis)}  ▶\n\n" +
            CigarDevelopmentText.DiagnosisDescription(selectedDiagnosis) +
            "\n\nA hypothesis is not declared correct. The next version tests it with more time and material while preserving the failed or ambiguous experiment as house knowledge.";
    }

    private List<PrototypeCigarState> AvailablePrototypes()
    {
        return development.Prototypes
            .Where(prototype =>
                !prototype.consumedByTasting &&
                prototype.readyAtElapsedBlock <= calendar.ElapsedBlocks)
            .OrderBy(prototype => prototype.recipeVersion)
            .ToList();
    }

    private void AdjustPrototypeSelection(bool up, bool down)
    {
        int count = AvailablePrototypes().Count;
        if (count == 0)
        {
            selectedPrototypeIndex = 0;
            return;
        }

        if (up) selectedPrototypeIndex = (selectedPrototypeIndex - 1 + count) % count;
        if (down) selectedPrototypeIndex = (selectedPrototypeIndex + 1) % count;
    }

    private void AdjustConstruction(int direction)
    {
        switch (constructionField)
        {
            case 0:
                choices.conditioning = Cycle(choices.conditioning, direction);
                break;
            case 1:
                choices.compression = Cycle(choices.compression, direction);
                break;
            case 2:
                choices.fillerArrangement = Cycle(choices.fillerArrangement, direction);
                break;
        }
    }

    private void Close()
    {
        page = Page.None;
        messageTitle = null;
        messageBody = null;
    }

    private void EnsureStyles()
    {
        if (titleStyle != null)
        {
            return;
        }

        titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 22,
            fontStyle = FontStyle.Bold,
            wordWrap = true,
        };
        bodyStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16,
            wordWrap = true,
            richText = false,
        };
        footerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 13,
            wordWrap = true,
        };
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

    private static string FormatArrangement(FillerArrangement arrangement)
    {
        return arrangement switch
        {
            FillerArrangement.ParallelFolds => "Parallel folds",
            FillerArrangement.LayeredBook => "Layered book fold",
            FillerArrangement.OpenAirflowChannels => "Open airflow channels",
            _ => arrangement.ToString(),
        };
    }

    private static T Cycle<T>(T current, int direction)
        where T : struct, Enum
    {
        T[] values = (T[])Enum.GetValues(typeof(T));
        int index = Array.IndexOf(values, current);
        return values[(index + direction + values.Length) % values.Length];
    }
}
}
