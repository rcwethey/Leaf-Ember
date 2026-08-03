using System;
using System.Text;
using LeafEmber.Estate;
using LeafEmber.Events;
using LeafEmber.Inventory;
using LeafEmber.Prototype.Events;
using LeafEmber.Prototype.Player;
using LeafEmber.Save;
using LeafEmber.Time;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeafEmber.Prototype.UI
{

public sealed class PrototypeHud : MonoBehaviour
{
    private const string CalendarSection = "calendar";
    private const string InventorySection = "inventory";
    private const string EstateSection = "estate";
    private const string PlayerSection = "player";

    private enum ModalKind
    {
        None,
        FocusedWork,
        Information,
        LotInspection,
        Summary,
    }

    private IEventBus eventBus;
    private ICalendarService calendar;
    private IInventoryService inventory;
    private IEstateService estate;
    private ISaveService saveService;
    private Transform player;
    private PlayerInteractor interactor;
    private ModalKind modal;
    private FocusedWorkRequestedEvent pendingWork;
    private string modalTitle;
    private string modalBody;
    private string toast;
    private float toastUntil;
    private int selectedLotIndex;
    private GUIStyle titleStyle;
    private GUIStyle bodyStyle;
    private GUIStyle smallStyle;
    private GUIStyle centeredStyle;

    public bool IsModalOpen => modal != ModalKind.None;

    public void Initialize(
        IEventBus gameEventBus,
        ICalendarService calendarService,
        IInventoryService inventoryService,
        IEstateService estateService,
        ISaveService gameSaveService,
        Transform playerTransform,
        PlayerInteractor playerInteractor)
    {
        eventBus = gameEventBus;
        calendar = calendarService;
        inventory = inventoryService;
        estate = estateService;
        saveService = gameSaveService;
        player = playerTransform;
        interactor = playerInteractor;

        eventBus.Subscribe<FocusedWorkRequestedEvent>(OnFocusedWorkRequested);
        eventBus.Subscribe<InformationRequestedEvent>(OnInformationRequested);
        eventBus.Subscribe<LotInspectionRequestedEvent>(OnLotInspectionRequested);
    }

    private void OnDestroy()
    {
        if (eventBus == null)
        {
            return;
        }

        eventBus.Unsubscribe<FocusedWorkRequestedEvent>(OnFocusedWorkRequested);
        eventBus.Unsubscribe<InformationRequestedEvent>(OnInformationRequested);
        eventBus.Unsubscribe<LotInspectionRequestedEvent>(OnLotInspectionRequested);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        if (keyboard.f5Key.wasPressedThisFrame)
        {
            SavePrototype();
        }

        if (keyboard.f9Key.wasPressedThisFrame)
        {
            LoadPrototype();
        }

        if (!IsModalOpen)
        {
            return;
        }

        if (keyboard.escapeKey.wasPressedThisFrame)
        {
            CloseModal();
            return;
        }

        if (modal == ModalKind.FocusedWork &&
            (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame))
        {
            CommitFocusedWork();
            return;
        }

        if (modal == ModalKind.LotInspection && inventory.LeafLots.Count > 1)
        {
            if (keyboard.leftArrowKey.wasPressedThisFrame || keyboard.aKey.wasPressedThisFrame)
            {
                selectedLotIndex =
                    (selectedLotIndex - 1 + inventory.LeafLots.Count) % inventory.LeafLots.Count;
            }

            if (keyboard.rightArrowKey.wasPressedThisFrame || keyboard.dKey.wasPressedThisFrame)
            {
                selectedLotIndex = (selectedLotIndex + 1) % inventory.LeafLots.Count;
            }
        }
    }

    private void OnGUI()
    {
        EnsureStyles();
        DrawStatusPanel();
        DrawControls();
        DrawInteractionPrompt();

        if (IsModalOpen)
        {
            DrawModal();
        }
        else
        {
            DrawCrosshair();
        }

        if (!string.IsNullOrWhiteSpace(toast) && UnityEngine.Time.unscaledTime < toastUntil)
        {
            Rect toastRect = new((Screen.width - 420f) * 0.5f, 28f, 420f, 42f);
            GUI.Box(toastRect, toast);
        }
    }

    private void DrawStatusPanel()
    {
        CalendarSnapshot now = calendar.Current;
        Rect panel = new(18f, 18f, 310f, 98f);
        GUI.Box(panel, GUIContent.none);
        GUI.Label(
            new Rect(panel.x + 14f, panel.y + 10f, panel.width - 28f, 28f),
            "LEAF & EMBER — FINCA PROTOTYPE",
            titleStyle);
        GUI.Label(
            new Rect(panel.x + 14f, panel.y + 41f, panel.width - 28f, 24f),
            FormatCalendar(now),
            bodyStyle);
        GUI.Label(
            new Rect(panel.x + 14f, panel.y + 68f, panel.width - 28f, 20f),
            GetClimatePeriod(now.month),
            smallStyle);
    }

    private void DrawControls()
    {
        Rect panel = new(18f, Screen.height - 82f, 360f, 64f);
        GUI.Box(panel, GUIContent.none);
        GUI.Label(
            new Rect(panel.x + 12f, panel.y + 8f, panel.width - 24f, 22f),
            "WASD move  •  Mouse look  •  E interact",
            smallStyle);
        GUI.Label(
            new Rect(panel.x + 12f, panel.y + 33f, panel.width - 24f, 22f),
            "F5 save  •  F9 load  •  Esc close",
            smallStyle);
    }

    private void DrawInteractionPrompt()
    {
        if (IsModalOpen || interactor == null || string.IsNullOrWhiteSpace(interactor.CurrentPrompt))
        {
            return;
        }

        string prompt = $"[E] {interactor.CurrentPrompt}";
        Rect promptRect = new((Screen.width - 620f) * 0.5f, Screen.height - 110f, 620f, 44f);
        GUI.Box(promptRect, prompt);
    }

    private void DrawCrosshair()
    {
        Rect crosshair = new((Screen.width * 0.5f) - 8f, (Screen.height * 0.5f) - 12f, 16f, 24f);
        GUI.Label(crosshair, "+", centeredStyle);
    }

    private void DrawModal()
    {
        float width = Mathf.Min(720f, Screen.width - 80f);
        float height = Mathf.Min(520f, Screen.height - 100f);
        Rect panel = new((Screen.width - width) * 0.5f, (Screen.height - height) * 0.5f, width, height);
        GUI.Box(panel, GUIContent.none);

        GUI.Label(
            new Rect(panel.x + 28f, panel.y + 24f, panel.width - 56f, 36f),
            modalTitle,
            titleStyle);

        string body = modal == ModalKind.LotInspection ? FormatSelectedLot() : modalBody;
        GUI.Label(
            new Rect(panel.x + 28f, panel.y + 72f, panel.width - 56f, panel.height - 132f),
            body,
            bodyStyle);

        string footer = modal switch
        {
            ModalKind.FocusedWork => "Enter: commit the work and advance time  •  Esc: cancel",
            ModalKind.LotInspection => "Left/Right or A/D: change lot  •  Esc: close (inspection is free)",
            _ => "Esc: close",
        };
        GUI.Label(
            new Rect(panel.x + 28f, panel.yMax - 48f, panel.width - 56f, 24f),
            footer,
            smallStyle);
    }

    private void OnFocusedWorkRequested(FocusedWorkRequestedEvent request)
    {
        pendingWork = request;
        modalTitle = request.Title;
        modalBody =
            $"{request.Description}\n\nThis is meaningful committed work. " +
            $"It will use {request.BlockCost} calendar block" +
            (request.BlockCost == 1 ? "." : "s.");
        modal = ModalKind.FocusedWork;
    }

    private void OnInformationRequested(InformationRequestedEvent request)
    {
        modalTitle = request.Title;
        modalBody = request.Body +
            "\n\nLooking, reading, and diagnosing do not advance the clock.";
        modal = ModalKind.Information;
    }

    private void OnLotInspectionRequested(LotInspectionRequestedEvent request)
    {
        selectedLotIndex = Mathf.Clamp(selectedLotIndex, 0, inventory.LeafLots.Count - 1);
        modalTitle = "Leaf lot cabinet";
        modal = ModalKind.LotInspection;
    }

    private void CommitFocusedWork()
    {
        CalendarAdvanceResult result = calendar.AdvanceBlocks(
            pendingWork.BlockCost,
            pendingWork.Title);

        StringBuilder summary = new();
        summary.AppendLine($"Completed: {result.Reason}");
        summary.AppendLine();
        summary.AppendLine($"Time: {FormatCalendar(result.Before)} → {FormatCalendar(result.After)}");

        if (result.ReachedCheckpoints.Count > 0)
        {
            summary.AppendLine();
            summary.AppendLine("Checkpoint reached:");
            foreach (ScheduledCheckpoint checkpoint in result.ReachedCheckpoints)
            {
                summary.AppendLine($"• {checkpoint.title}");
                if (!string.IsNullOrWhiteSpace(checkpoint.description))
                {
                    summary.AppendLine($"  {checkpoint.description}");
                }
            }
        }
        else
        {
            summary.AppendLine();
            summary.AppendLine("No scheduled checkpoint interrupted this work block.");
        }

        modalTitle = "Work block summary";
        modalBody = summary.ToString();
        modal = ModalKind.Summary;
    }

    private string FormatSelectedLot()
    {
        if (inventory.LeafLots.Count == 0)
        {
            return "No leaf lots are currently recorded.";
        }

        LeafLotState lot = inventory.LeafLots[selectedLotIndex];
        return
            $"LOT {selectedLotIndex + 1} OF {inventory.LeafLots.Count}\n\n" +
            $"{lot.displayName}\n" +
            $"Quantity: {lot.quantityKilograms:0.##} kg\n\n" +
            $"Origin: {lot.origin}\n" +
            $"Grower: {lot.grower}\n" +
            $"Tobacco: {lot.tobaccoType}\n" +
            $"Harvest: {lot.harvestReference}\n\n" +
            $"Process history: {lot.processHistory}\n" +
            $"Current intended role: {lot.intendedRole}\n\n" +
            $"Recorded observations: {lot.observations}\n\n" +
            "These are provenance and house observations—not an omniscient quality score.";
    }

    private void SavePrototype()
    {
        try
        {
            SaveGameData saveGame = SaveGameData.CreateNew("finca-prototype");
            SaveSectionStore.Set(saveGame, CalendarSection, calendar.Current);
            SaveSectionStore.Set(saveGame, InventorySection, inventory.Capture());
            SaveSectionStore.Set(saveGame, EstateSection, estate.Capture());
            SaveSectionStore.Set(saveGame, PlayerSection, new PlayerPositionSnapshot
            {
                position = player.position,
                eulerAngles = player.eulerAngles,
            });
            saveService.Save(saveGame);
            ShowToast("Prototype saved: player, calendar, estate, and leaf lots.");
        }
        catch (Exception exception)
        {
            ShowToast($"Save failed: {exception.Message}");
        }
    }

    private void LoadPrototype()
    {
        try
        {
            if (!saveService.SaveExists)
            {
                ShowToast("No prototype save exists yet.");
                return;
            }

            SaveGameData saveGame = saveService.Load();
            if (SaveSectionStore.TryGet(saveGame, CalendarSection, out CalendarSnapshot calendarState))
            {
                calendar.Restore(calendarState);
            }

            if (SaveSectionStore.TryGet(saveGame, InventorySection, out InventorySnapshot inventoryState))
            {
                inventory.Restore(inventoryState);
            }

            if (SaveSectionStore.TryGet(saveGame, EstateSection, out EstateSnapshot estateState))
            {
                estate.Restore(estateState);
            }

            if (SaveSectionStore.TryGet(saveGame, PlayerSection, out PlayerPositionSnapshot playerState))
            {
                CharacterController controller = player.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.enabled = false;
                }

                player.SetPositionAndRotation(
                    playerState.position,
                    Quaternion.Euler(playerState.eulerAngles));

                if (controller != null)
                {
                    controller.enabled = true;
                }
            }

            CloseModal();
            ShowToast("Prototype loaded.");
        }
        catch (Exception exception)
        {
            ShowToast($"Load failed: {exception.Message}");
        }
    }

    private void CloseModal()
    {
        modal = ModalKind.None;
        modalTitle = null;
        modalBody = null;
    }

    private void ShowToast(string message)
    {
        toast = message;
        toastUntil = UnityEngine.Time.unscaledTime + 4f;
    }

    private void EnsureStyles()
    {
        if (titleStyle != null)
        {
            return;
        }

        titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 18,
            fontStyle = FontStyle.Bold,
            wordWrap = true,
        };
        bodyStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16,
            wordWrap = true,
            richText = false,
        };
        smallStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 13,
            wordWrap = true,
        };
        centeredStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 18,
            fontStyle = FontStyle.Bold,
        };
    }

    private static string FormatCalendar(CalendarSnapshot snapshot)
    {
        return $"Year {snapshot.year}  •  Month {snapshot.month}, Day {snapshot.day}  •  {snapshot.block}";
    }

    private static string GetClimatePeriod(int month)
    {
        return month switch
        {
            1 or 2 or 3 or 12 => "Climate: dry period",
            4 => "Climate: transition toward rain",
            5 or 6 or 7 => "Climate: rainy period",
            8 => "Climate: possible drier interval",
            9 or 10 => "Climate: sustained heavy rain",
            11 => "Climate: transition toward dry weather",
            _ => "Climate: locally variable",
        };
    }
}
}
