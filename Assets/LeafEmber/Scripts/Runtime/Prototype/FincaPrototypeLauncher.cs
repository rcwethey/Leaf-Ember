using System;
using LeafEmber.Cigar;
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
        ICigarDevelopmentService cigarDevelopment =
            services.Resolve<ICigarDevelopmentService>();
        ISaveService saveService = services.Resolve<ISaveService>();

        GameObject world = new("[Leaf & Ember] Finca Prototype");
        FincaWorldBuilder.BuildEnvironment(world.transform);

        PrototypeHud hud = world.AddComponent<PrototypeHud>();
        CigarDevelopmentView cigarView = world.AddComponent<CigarDevelopmentView>();
        FincaExperienceHud experienceHud = world.AddComponent<FincaExperienceHud>();
        Camera playerCamera = CreatePlayerCamera();
        Func<bool> inputBlocked = () =>
            hud.IsModalOpen || cigarView.IsOpen || experienceHud.IsModalOpen;
        GameObject player = CreatePlayer(
            playerCamera,
            inputBlocked,
            eventBus,
            out PlayerInteractor interactor);
        player.transform.SetParent(world.transform);

        hud.Initialize(
            eventBus,
            calendar,
            inventory,
            estate,
            cigarDevelopment,
            saveService,
            player.transform,
            interactor);
        cigarView.Initialize(eventBus, calendar, cigarDevelopment);
        experienceHud.Initialize(
            calendar,
            cigarDevelopment,
            interactor,
            () => hud.IsModalOpen || cigarView.IsOpen);
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
        Func<bool> inputBlocked,
        IEventBus eventBus,
        out PlayerInteractor interactor)
    {
        GameObject player = new("Founder");
        player.transform.position = new Vector3(0f, 0.55f, -45f);

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
        controller.Initialize(playerCamera.transform, inputBlocked);
        interactor.Initialize(
            playerCamera,
            new InteractionContext(eventBus),
            inputBlocked);

        return player;
    }
}
}
