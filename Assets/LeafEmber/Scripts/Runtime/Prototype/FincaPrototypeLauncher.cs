using LeafEmber.Core;
using LeafEmber.Estate;
using LeafEmber.Events;
using LeafEmber.Inventory;
using LeafEmber.Prototype.Interaction;
using LeafEmber.Prototype.Player;
using LeafEmber.Prototype.UI;
using LeafEmber.Save;
using LeafEmber.Time;
using UnityEngine;

namespace LeafEmber.Prototype
{

[DefaultExecutionOrder(-9000)]
public sealed class FincaPrototypeLauncher : MonoBehaviour
{
    private void Start()
    {
        ServiceRegistry services = GameServices.Registry;
        IEventBus eventBus = services.Resolve<IEventBus>();
        ICalendarService calendar = services.Resolve<ICalendarService>();
        IInventoryService inventory = services.Resolve<IInventoryService>();
        IEstateService estate = services.Resolve<IEstateService>();
        ISaveService saveService = services.Resolve<ISaveService>();

        GameObject world = new("[Leaf & Ember] Finca Prototype");
        FincaWorldBuilder.BuildEnvironment(world.transform);

        PrototypeHud hud = world.AddComponent<PrototypeHud>();
        Camera playerCamera = CreatePlayerCamera();
        GameObject player = CreatePlayer(playerCamera, hud, eventBus, out PlayerInteractor interactor);
        player.transform.SetParent(world.transform);

        hud.Initialize(
            eventBus,
            calendar,
            inventory,
            estate,
            saveService,
            player.transform,
            interactor);
    }

    private static Camera CreatePlayerCamera()
    {
        Camera playerCamera = Camera.main;
        if (playerCamera == null)
        {
            GameObject cameraObject = new("Player Camera");
            cameraObject.tag = "MainCamera";
            playerCamera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
        }

        playerCamera.fieldOfView = 68f;
        playerCamera.nearClipPlane = 0.08f;
        return playerCamera;
    }

    private static GameObject CreatePlayer(
        Camera playerCamera,
        PrototypeHud hud,
        IEventBus eventBus,
        out PlayerInteractor interactor)
    {
        GameObject player = new("Founder");
        player.transform.position = new Vector3(-2f, 0.4f, -4f);

        CharacterController characterController = player.AddComponent<CharacterController>();
        characterController.height = 1.8f;
        characterController.radius = 0.34f;
        characterController.center = new Vector3(0f, 0.9f, 0f);
        characterController.stepOffset = 0.35f;

        playerCamera.transform.SetParent(player.transform, false);
        playerCamera.transform.localPosition = new Vector3(0f, 1.62f, 0f);
        playerCamera.transform.localRotation = Quaternion.identity;

        PrototypePlayerController controller = player.AddComponent<PrototypePlayerController>();
        interactor = player.AddComponent<PlayerInteractor>();
        controller.Initialize(playerCamera.transform, () => hud.IsModalOpen);
        interactor.Initialize(
            playerCamera,
            new InteractionContext(eventBus),
            () => hud.IsModalOpen);

        return player;
    }
}
}
