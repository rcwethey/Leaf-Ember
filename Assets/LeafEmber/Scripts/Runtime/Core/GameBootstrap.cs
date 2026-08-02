using LeafEmber.Events;
using LeafEmber.Save;
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
        registry.Register<IEventBus>(eventBus);
        registry.Register<ISaveService>(new JsonSaveService());
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
