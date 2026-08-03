using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeafEmber.Prototype.Player
{

[RequireComponent(typeof(CharacterController))]
public sealed class PrototypePlayerController : MonoBehaviour
{
    private const float MoveSpeed = 5f;
    private const float MouseSensitivity = 0.12f;
    private const float StickLookSpeed = 120f;

    private CharacterController characterController;
    private InputAction moveAction;
    private InputAction mouseLookAction;
    private InputAction stickLookAction;
    private Transform view;
    private Func<bool> isInputBlocked;
    private float pitch;
    private float verticalVelocity;

    public void Initialize(Transform cameraTransform, Func<bool> inputBlocked)
    {
        view = cameraTransform;
        isInputBlocked = inputBlocked;
        pitch = view.localEulerAngles.x;
        if (pitch > 180f)
        {
            pitch -= 360f;
        }
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.AddBinding("<Gamepad>/leftStick");

        mouseLookAction = new InputAction(
            "Mouse Look",
            InputActionType.Value,
            "<Mouse>/delta");
        stickLookAction = new InputAction(
            "Stick Look",
            InputActionType.Value,
            "<Gamepad>/rightStick");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        mouseLookAction.Enable();
        stickLookAction.Enable();
        LockCursor();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        mouseLookAction.Disable();
        stickLookAction.Disable();
    }

    private void OnDestroy()
    {
        moveAction.Dispose();
        mouseLookAction.Dispose();
        stickLookAction.Dispose();
    }

    private void Update()
    {
        if (view == null)
        {
            return;
        }

        bool inputBlocked = isInputBlocked != null && isInputBlocked();
        if (inputBlocked)
        {
            UnlockCursor();
            ApplyGravity(Vector2.zero);
            return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            LockCursor();
        }

        ApplyLook();
        ApplyGravity(moveAction.ReadValue<Vector2>());
    }

    private void ApplyLook()
    {
        Vector2 mouseLook = mouseLookAction.ReadValue<Vector2>() * MouseSensitivity;
        Vector2 stickLook =
            stickLookAction.ReadValue<Vector2>() * StickLookSpeed * UnityEngine.Time.deltaTime;
        Vector2 look = mouseLook + stickLook;

        transform.Rotate(Vector3.up, look.x, Space.World);
        pitch = Mathf.Clamp(pitch - look.y, -75f, 80f);
        view.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void ApplyGravity(Vector2 input)
    {
        Vector3 horizontal =
            ((transform.right * input.x) + (transform.forward * input.y)) * MoveSpeed;

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += Physics.gravity.y * UnityEngine.Time.deltaTime;
        Vector3 velocity = horizontal + (Vector3.up * verticalVelocity);
        characterController.Move(velocity * UnityEngine.Time.deltaTime);
    }

    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
}
