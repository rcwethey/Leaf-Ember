using System;
using System.Linq;
using LeafEmber.Cigar;
using LeafEmber.Prototype.Interaction;
using LeafEmber.Prototype.Player;
using LeafEmber.Time;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LeafEmber.Prototype.UI
{

public sealed class FincaExperienceHud : MonoBehaviour
{
    private static readonly Color Ink = new(0.08f, 0.065f, 0.05f, 0.96f);
    private static readonly Color InkSoft = new(0.10f, 0.08f, 0.06f, 0.88f);
    private static readonly Color Cream = new(0.94f, 0.88f, 0.72f, 1f);
    private static readonly Color Muted = new(0.72f, 0.66f, 0.54f, 1f);
    private static readonly Color Ember = new(0.82f, 0.40f, 0.17f, 1f);
    private static readonly Color Leaf = new(0.42f, 0.58f, 0.30f, 1f);

    private ICalendarService calendar;
    private ICigarDevelopmentService development;
    private PlayerInteractor interactor;
    private Func<bool> hasOtherModal;
    private Font font;
    private GameObject statusPanel;
    private GameObject objectivePanel;
    private GameObject promptPanel;
    private GameObject modalShade;
    private Text calendarText;
    private Text climateText;
    private Text objectiveTitle;
    private Text objectiveBody;
    private Text objectiveDestination;
    private Text promptCategory;
    private Text promptAction;
    private Text promptExplanation;
    private Text promptCost;
    private Text modalEyebrow;
    private Text modalTitle;
    private Text modalBody;
    private Text modalFooter;
    private Text crosshair;
    private int onboardingPage;
    private bool onboardingActive = true;
    private bool glossaryActive;

    public bool IsModalOpen => onboardingActive || glossaryActive;

    public void Initialize(
        ICalendarService calendarService,
        ICigarDevelopmentService developmentService,
        PlayerInteractor playerInteractor,
        Func<bool> otherModalOpen)
    {
        calendar = calendarService;
        development = developmentService;
        interactor = playerInteractor;
        hasOtherModal = otherModalOpen;
        BuildInterface();
        RefreshModal();
        RefreshPersistentInformation();
    }

    private void Update()
    {
        if (calendar == null || development == null)
        {
            return;
        }

        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (onboardingActive)
            {
                if (keyboard.escapeKey.wasPressedThisFrame)
                {
                    onboardingActive = false;
                    RefreshModal();
                }
                else if (keyboard.enterKey.wasPressedThisFrame ||
                         keyboard.numpadEnterKey.wasPressedThisFrame ||
                         keyboard.spaceKey.wasPressedThisFrame)
                {
                    onboardingPage++;
                    if (onboardingPage >= 3)
                    {
                        onboardingActive = false;
                    }

                    RefreshModal();
                }
            }
            else if (glossaryActive && keyboard.escapeKey.wasPressedThisFrame)
            {
                glossaryActive = false;
                RefreshModal();
            }
            else if (keyboard.gKey.wasPressedThisFrame &&
                     (hasOtherModal == null || !hasOtherModal()))
            {
                glossaryActive = !glossaryActive;
                RefreshModal();
            }
        }

        RefreshPersistentInformation();
    }

    private void BuildInterface()
    {
        font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        GameObject canvasObject = new("Finca Experience Canvas");
        canvasObject.transform.SetParent(transform, false);
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 0.5f;
        canvas.sortingOrder = 80;
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;
        canvasObject.AddComponent<GraphicRaycaster>();

        statusPanel = CreatePanel(
            "Estate Status",
            canvasObject.transform,
            new Vector2(0f, 1f),
            new Vector2(0f, 1f),
            new Vector2(34f, -30f),
            new Vector2(360f, 118f),
            new Vector2(0f, 1f),
            Ink);
        CreateText(
            "House Name",
            statusPanel.transform,
            new Vector2(18f, -14f),
            new Vector2(324f, 28f),
            "LEAF & EMBER",
            20,
            Cream,
            FontStyle.Bold,
            TextAnchor.UpperLeft);
        calendarText = CreateText(
            "Calendar",
            statusPanel.transform,
            new Vector2(18f, -49f),
            new Vector2(324f, 26f),
            string.Empty,
            16,
            Color.white,
            FontStyle.Normal,
            TextAnchor.UpperLeft);
        climateText = CreateText(
            "Climate",
            statusPanel.transform,
            new Vector2(18f, -80f),
            new Vector2(324f, 22f),
            string.Empty,
            13,
            Muted,
            FontStyle.Italic,
            TextAnchor.UpperLeft);

        objectivePanel = CreatePanel(
            "Current Objective",
            canvasObject.transform,
            new Vector2(1f, 1f),
            new Vector2(1f, 1f),
            new Vector2(-34f, -30f),
            new Vector2(570f, 154f),
            new Vector2(1f, 1f),
            InkSoft);
        CreateText(
            "Objective Label",
            objectivePanel.transform,
            new Vector2(20f, -14f),
            new Vector2(530f, 20f),
            "CURRENT STUDY",
            12,
            Ember,
            FontStyle.Bold,
            TextAnchor.UpperLeft);
        objectiveTitle = CreateText(
            "Objective Title",
            objectivePanel.transform,
            new Vector2(20f, -38f),
            new Vector2(530f, 30f),
            string.Empty,
            20,
            Cream,
            FontStyle.Bold,
            TextAnchor.UpperLeft);
        objectiveBody = CreateText(
            "Objective Explanation",
            objectivePanel.transform,
            new Vector2(20f, -72f),
            new Vector2(530f, 48f),
            string.Empty,
            14,
            Color.white,
            FontStyle.Normal,
            TextAnchor.UpperLeft);
        objectiveDestination = CreateText(
            "Objective Destination",
            objectivePanel.transform,
            new Vector2(20f, -126f),
            new Vector2(530f, 20f),
            string.Empty,
            13,
            Leaf,
            FontStyle.Bold,
            TextAnchor.UpperLeft);

        promptPanel = CreatePanel(
            "Contextual Interaction",
            canvasObject.transform,
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 34f),
            new Vector2(820f, 154f),
            new Vector2(0.5f, 0f),
            Ink);
        promptCategory = CreateText(
            "Interaction Category",
            promptPanel.transform,
            new Vector2(22f, -14f),
            new Vector2(776f, 20f),
            string.Empty,
            12,
            Ember,
            FontStyle.Bold,
            TextAnchor.UpperLeft);
        promptAction = CreateText(
            "Interaction Action",
            promptPanel.transform,
            new Vector2(22f, -39f),
            new Vector2(776f, 30f),
            string.Empty,
            20,
            Cream,
            FontStyle.Bold,
            TextAnchor.UpperLeft);
        promptExplanation = CreateText(
            "Interaction Explanation",
            promptPanel.transform,
            new Vector2(22f, -72f),
            new Vector2(776f, 45f),
            string.Empty,
            14,
            Color.white,
            FontStyle.Normal,
            TextAnchor.UpperLeft);
        promptCost = CreateText(
            "Interaction Cost",
            promptPanel.transform,
            new Vector2(22f, -124f),
            new Vector2(776f, 20f),
            string.Empty,
            13,
            Muted,
            FontStyle.Bold,
            TextAnchor.UpperLeft);

        crosshair = CreateText(
            "Crosshair",
            canvasObject.transform,
            Vector2.zero,
            new Vector2(20f, 20f),
            "•",
            18,
            new Color(1f, 0.93f, 0.72f, 0.82f),
            FontStyle.Bold,
            TextAnchor.MiddleCenter,
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f));

        CreateText(
            "Glossary Hint",
            canvasObject.transform,
            new Vector2(-34f, 26f),
            new Vector2(300f, 24f),
            "[G] Craft glossary",
            13,
            Muted,
            FontStyle.Normal,
            TextAnchor.MiddleRight,
            new Vector2(1f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 0f));

        modalShade = CreatePanel(
            "Guidance Modal Shade",
            canvasObject.transform,
            Vector2.zero,
            Vector2.one,
            Vector2.zero,
            Vector2.zero,
            new Vector2(0.5f, 0.5f),
            new Color(0.025f, 0.02f, 0.015f, 0.88f),
            true);
        GameObject card = CreatePanel(
            "Guidance Card",
            modalShade.transform,
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            Vector2.zero,
            new Vector2(900f, 650f),
            new Vector2(0.5f, 0.5f),
            new Color(0.095f, 0.075f, 0.055f, 0.99f));
        modalEyebrow = CreateText(
            "Modal Eyebrow",
            card.transform,
            new Vector2(52f, -42f),
            new Vector2(796f, 24f),
            string.Empty,
            13,
            Ember,
            FontStyle.Bold,
            TextAnchor.UpperLeft);
        modalTitle = CreateText(
            "Modal Title",
            card.transform,
            new Vector2(52f, -78f),
            new Vector2(796f, 52f),
            string.Empty,
            32,
            Cream,
            FontStyle.Bold,
            TextAnchor.UpperLeft);
        modalBody = CreateText(
            "Modal Body",
            card.transform,
            new Vector2(52f, -150f),
            new Vector2(796f, 400f),
            string.Empty,
            18,
            Color.white,
            FontStyle.Normal,
            TextAnchor.UpperLeft);
        modalFooter = CreateText(
            "Modal Footer",
            card.transform,
            new Vector2(52f, -580f),
            new Vector2(796f, 30f),
            string.Empty,
            14,
            Muted,
            FontStyle.Bold,
            TextAnchor.UpperLeft);
    }

    private void RefreshPersistentInformation()
    {
        CalendarSnapshot now = calendar.Current;
        calendarText.text =
            $"Year {now.year}  •  Month {now.month}, Day {now.day}  •  {FormatBlock(now.block)}";
        climateText.text = ClimateLabel(now.month);

        BuildObjective(out string title, out string body, out string destination);
        objectiveTitle.text = title;
        objectiveBody.text = body;
        objectiveDestination.text = $"GO TO: {destination}";

        bool anotherModal = hasOtherModal != null && hasOtherModal();
        InteractionPresentation presentation = interactor?.CurrentPresentation;
        bool showPrompt = !IsModalOpen && !anotherModal && presentation != null;
        promptPanel.SetActive(showPrompt);
        crosshair.gameObject.SetActive(!IsModalOpen && !anotherModal);
        if (showPrompt)
        {
            promptCategory.text = $"{presentation.Location.ToUpperInvariant()}  •  {presentation.Category}";
            promptAction.text = $"[E]  {presentation.Action}";
            promptExplanation.text = presentation.Explanation;
            promptCost.text = presentation.Cost;
        }
    }

    private void BuildObjective(
        out string title,
        out string body,
        out string destination)
    {
        if (development.Recipes.Count == 0)
        {
            title = "Create the founding study cigar";
            body = "Start with an intended experience, then choose leaf and construction decisions that might produce it.";
            destination = "Personal workshop — east side of the courtyard";
            return;
        }

        if (development.HasPendingRevision)
        {
            title = $"Construct {development.LatestRecipe.versionLabel}";
            body = "Your diagnosis created a testable revision. Build it as a separate cigar so the earlier evidence remains intact.";
            destination = "Personal workshop — rolling bench";
            return;
        }

        PrototypeCigarState ready = development.Prototypes.FirstOrDefault(
            prototype =>
                !prototype.consumedByTasting &&
                prototype.readyAtElapsedBlock <= calendar.ElapsedBlocks);
        if (ready != null)
        {
            title = $"Taste {ready.displayName}";
            body = "The cigar has rested. Taste it in stages, compare it with the recorded intent, and look for a plausible cause.";
            destination = "Shaded tasting patio — central courtyard";
            return;
        }

        PrototypeCigarState resting = development.Prototypes.FirstOrDefault(
            prototype => !prototype.consumedByTasting);
        if (resting != null)
        {
            int remaining = Mathf.Max(0, resting.readyAtElapsedBlock - calendar.ElapsedBlocks);
            title = "Let the study cigar recover";
            body = $"The cigar needs {remaining} more work block(s) before tasting. Inspect the finca or complete another meaningful task.";
            destination = "Any inspection or committed finca workstation";
            return;
        }

        TastingRecordState latest = development.Tastings.LastOrDefault();
        if (latest != null &&
            !development.Diagnoses.Any(diagnosis => diagnosis.prototypeId == latest.prototypeId))
        {
            title = "Turn observations into a hypothesis";
            body = "The tasting is evidence, not an answer. Decide which material, construction, rest, or combustion cause should be tested next.";
            destination = "Personal workshop — recipe notebook";
            return;
        }

        if (development.Tastings.Count >= 2)
        {
            title = "Compare the latest recipe versions";
            body = "Review construction and sensory tradeoffs. Choose which direction better serves the house intent; the game will not select a winner.";
            destination = "Personal workshop — recipe notebook";
            return;
        }

        title = "Review the house notebook";
        body = "Your current study is preserved. Use its evidence to decide whether another revision is worth the time and material.";
        destination = "Personal workshop";
    }

    private void RefreshModal()
    {
        bool visible = onboardingActive || glossaryActive;
        modalShade.SetActive(visible);
        if (!visible)
        {
            return;
        }

        if (glossaryActive)
        {
            modalEyebrow.text = "CRAFT REFERENCE  •  AVAILABLE ANY TIME";
            modalTitle.text = "Founding cigar glossary";
            modalBody.text =
                "SECO — Usually lower or middle priming leaf; often valued for aroma and easier combustion.\n\n" +
                "VISO — Middle-to-upper priming leaf that commonly contributes body, strength, and structure.\n\n" +
                "FILLER — The interior leaves that establish much of a cigar's blend and airflow.\n\n" +
                "BINDER — The structural leaf surrounding the filler bunch beneath the wrapper.\n\n" +
                "WRAPPER — The outer leaf; important to construction, aroma, finish, and presentation.\n\n" +
                "CONDITIONING — Bringing leaf to a workable moisture and elasticity before handling.\n\n" +
                "COMPRESSION — How tightly the filler bunch is formed. Tighter is not automatically better.\n\n" +
                "DRAW — The resistance felt while pulling air and smoke through the cigar.\n\n" +
                "PILÓN — A deliberately managed stack in which cured tobacco ferments.";
            modalFooter.text = "Esc or G: return to the finca";
            return;
        }

        modalEyebrow.text = $"FINCA ORIENTATION  •  {onboardingPage + 1} OF 3";
        switch (onboardingPage)
        {
            case 0:
                modalTitle.text = "Build a house through understanding";
                modalBody.text =
                    "Leaf & Ember is not about covering the map with enormous fields. Your work is to understand material, make deliberate cigars, preserve evidence, and develop a recognizable house style.\n\n" +
                    "Walking, looking, and reading are free. A meaningful craft or management action clearly states its cost before it advances the calendar.";
                break;
            case 1:
                modalTitle.text = "Read the finca as a working place";
                modalBody.text =
                    "The estate is organized around a central courtyard. The field and curing barn sit to the west; fermentation and leaf storage are uphill to the north; the workshop and aging room are to the east; the office and arrival court are to the south.\n\n" +
                    "Look at a station to see what it does, why it matters, and whether it consumes time or material. Press E only when you want to open it.";
                break;
            default:
                modalTitle.text = "Begin with intent, then gather evidence";
                modalBody.text =
                    "Your first objective is at the personal workshop. Record the experience you want, select a starting blend, and construct one study cigar.\n\n" +
                    "After it rests, use the shaded tasting patio. The tasting does not reveal objectively correct flavors; it gives observations that you compare with intent before choosing a hypothesis.\n\n" +
                    "Press G at any time for the craft glossary.";
                break;
        }

        modalFooter.text = onboardingPage < 2
            ? "Enter or Space: continue  •  Esc: skip orientation"
            : "Enter or Space: enter the finca  •  Esc: skip";
    }

    private GameObject CreatePanel(
        string name,
        Transform parent,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 anchoredPosition,
        Vector2 size,
        Vector2 pivot,
        Color color,
        bool stretch = false)
    {
        GameObject panel = new(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(parent, false);
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        if (stretch)
        {
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
        else
        {
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
        }

        Image image = panel.GetComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        return panel;
    }

    private Text CreateText(
        string name,
        Transform parent,
        Vector2 anchoredPosition,
        Vector2 size,
        string value,
        int fontSize,
        Color color,
        FontStyle style,
        TextAnchor alignment,
        Vector2? anchorMin = null,
        Vector2? anchorMax = null,
        Vector2? pivot = null)
    {
        GameObject textObject = new(
            name,
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Text));
        textObject.transform.SetParent(parent, false);
        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin ?? new Vector2(0f, 1f);
        rect.anchorMax = anchorMax ?? new Vector2(0f, 1f);
        rect.pivot = pivot ?? new Vector2(0f, 1f);
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = size;

        Text text = textObject.GetComponent<Text>();
        text.font = font;
        text.text = value;
        text.fontSize = fontSize;
        text.fontStyle = style;
        text.color = color;
        text.alignment = alignment;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
        return text;
    }

    private static string FormatBlock(DayBlock block)
    {
        return block switch
        {
            DayBlock.Morning => "Morning",
            DayBlock.Afternoon => "Afternoon",
            DayBlock.Evening => "Evening",
            _ => block.ToString(),
        };
    }

    private static string ClimateLabel(int month)
    {
        return month switch
        {
            1 or 2 or 3 or 12 => "Dry period — warm days, cooler nights",
            4 => "Transition toward the rains",
            5 or 6 or 7 => "Rainy period — humidity needs attention",
            8 => "Possible drier interval",
            9 or 10 => "Sustained heavy rain",
            11 => "Transition toward dry weather",
            _ => "Locally variable conditions",
        };
    }
}
}
