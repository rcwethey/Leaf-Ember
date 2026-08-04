using System;
using LeafEmber.Prototype.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeafEmber.Prototype.Player
{

public sealed class PlayerInteractor : MonoBehaviour
{
    private const float InteractionRange = 4f;

    private readonly RaycastHit[] hits = new RaycastHit[8];
    private InputAction interactAction;
    private Camera playerCamera;
    private InteractionContext context;
    private Func<bool> isInputBlocked;
    private IInteractable focusedInteractable;

    public string CurrentPrompt => focusedInteractable?.InteractionPrompt;

    public InteractionPresentation CurrentPresentation =>
        focusedInteractable is IContextualInteractable contextual
            ? contextual.Presentation
            : null;

    public bool HasFocusedInteraction => focusedInteractable != null;

    public void Initialize(
        Camera camera,
        InteractionContext interactionContext,
        Func<bool> inputBlocked)
    {
        playerCamera = camera;
        context = interactionContext;
        isInputBlocked = inputBlocked;
    }

    private void Awake()
    {
        interactAction = new InputAction("Interact", InputActionType.Button);
        interactAction.AddBinding("<Keyboard>/e");
        interactAction.AddBinding("<Gamepad>/buttonSouth");
    }

    private void OnEnable()
    {
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    private void OnDestroy()
    {
        interactAction.Dispose();
    }

    private void Update()
    {
        focusedInteractable = null;
        if (playerCamera == null || context == null ||
            (isInputBlocked != null && isInputBlocked()))
        {
            return;
        }

        int hitCount = Physics.RaycastNonAlloc(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            hits,
            InteractionRange,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore);

        float nearestDistance = float.MaxValue;
        for (int index = 0; index < hitCount; index++)
        {
            IInteractable candidate = FindInteractable(hits[index].collider);
            if (candidate != null && hits[index].distance < nearestDistance)
            {
                focusedInteractable = candidate;
                nearestDistance = hits[index].distance;
            }
        }

        if (focusedInteractable != null && interactAction.WasPressedThisFrame())
        {
            focusedInteractable.Interact(context);
        }
    }

    private static IInteractable FindInteractable(Collider targetCollider)
    {
        MonoBehaviour[] behaviours = targetCollider.GetComponentsInParent<MonoBehaviour>();
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is IInteractable interactable)
            {
                return interactable;
            }
        }

        return null;
    }
}
}
