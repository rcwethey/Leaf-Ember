using LeafEmber.Estate;
using LeafEmber.Events;
using LeafEmber.Inventory;
using LeafEmber.Prototype;
using LeafEmber.Save;
using LeafEmber.Time;
using UnityEngine;

namespace LeafEmber.Core
{

[DefaultExecutionOrder(-10000)]
public sealed class GameBootstrap : MonoBehaviour
{
    private static GameBootstrap instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        instance = null;
        GameServices.Reset();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateCompositionRoot()
    {
        if (instance != null)
        {
            return;
        }

        GameObject root = new("[Leaf & Ember] Services");
        DontDestroyOnLoad(root);
        instance = root.AddComponent<GameBootstrap>();
        root.AddComponent<FincaPrototypeLauncher>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        ServiceRegistry registry = new();
        GameEventBus eventBus = new();
        CalendarService calendarService = new();
        calendarService.Schedule(new ScheduledCheckpoint
        {
            id = "curing-humidity-check",
            title = "Curing barn humidity check",
            description = "The pilot leaf is ready for another hands-on inspection.",
            elapsedBlock = 1,
        });
        calendarService.Schedule(new ScheduledCheckpoint
        {
            id = "ortega-visit",
            title = "Elena Ortega visits the finca",
            description = "A promised conversation about the sourced viso lot is now due.",
            elapsedBlock = 3,
        });
        registry.Register<IEventBus>(eventBus);
        registry.Register<ISaveService>(new JsonSaveService());
        registry.Register<ICalendarService>(calendarService);
        registry.Register<IInventoryService>(new InventoryService());
        registry.Register<IEstateService>(new EstateService());
        GameServices.Initialize(registry);
        eventBus.Publish(new GameStartedEvent(Application.version));
    }

    private void OnDestroy()
    {
        if (instance != this)
        {
            return;
        }

        instance = null;
        GameServices.Reset();
    }
}
}
