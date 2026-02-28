using Godot;

public partial class MoveComponent : Node
{
    [Export] public Node3D Model;
    [Export] public CharacterBody3D CharacterBody;
    [Export] public AnimationPlayer AnimationPlayer;
    [Export] public string WalkAnimationName = "mixamo_com";
    [Export] public string IdleAnimationName = "Take 001";
    [Export] public float Acceleration = 8.0f;
    [Export] public float Speed = 4.0f;
    [Export] public float Gravity = 9.8f;
    [Export] public float JumpVelocity = 4.5f;
    [Export] public float RotationSpeed = 8.0f;

    // For modular animation system
    [Export] public bool UseModularAnimations = false;

    public Vector2 Direction = Vector2.Zero;
    public bool WantsToJump = false;
    private bool _isMoving = false;
    private Vector3 _modelStartPosition;

    public void Tick(double delta)
    {
        if (CharacterBody == null || Model == null)
        {
            Logger.Error("MoveComponent requires a reference to the CharacterBody3D and Model nodes.");
            return;
        }

        Vector3 velocity = CharacterBody.Velocity;
        velocity.X = Direction.X * Speed;
        velocity.Z = Direction.Y * Speed;

        if (!CharacterBody.IsOnFloor())
        {
            velocity += Vector3.Down * Gravity * (float)delta;
        }

        if (WantsToJump && CharacterBody.IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        CharacterBody.Velocity = velocity;
        CharacterBody.MoveAndSlide();

        if (Direction.LengthSquared() > 0.001f)
        {
            // Direction is already camera-relative from PlayerController, so just use it directly
            // No need to add camera rotation again (that was causing double-rotation)
            float targetAngle = Mathf.Atan2(Direction.X, Direction.Y);

            float currentAngle = Model.Rotation.Y;
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, RotationSpeed * (float)delta);
            Model.Rotation = new Vector3(Model.Rotation.X, newAngle, Model.Rotation.Z);

            // Log occasionally to avoid spam
            if (Mathf.Abs(newAngle - currentAngle) > 0.01f)
            {
                Logger.Debug($"Rotating model '{Model.Name}' - Direction: {Direction}, TargetAngle: {targetAngle:F2}, NewY: {newAngle:F2}");
            }
        }

        // Handle animations based on movement
        HandleMovementAnimations();
        
        // Handle root motion compensation (keep character in place)
        HandleRootMotion();
    }

    private void HandleRootMotion()
    {
        if (Model == null || !_isMoving) return;
        
        // Reset model position to prevent root motion from affecting character position
        // This keeps the character in place while the animation plays
        if (_modelStartPosition == Vector3.Zero)
        {
            _modelStartPosition = Model.Position;
        }
        
        // Keep the model at its starting position (compensate for root motion)
        Model.Position = _modelStartPosition;
    }

    private void HandleMovementAnimations()
    {
        if (AnimationPlayer == null) return;

        bool isCurrentlyMoving = Direction.LengthSquared() > 0.001f;

        // Only change animation state when movement changes
        if (isCurrentlyMoving != _isMoving)
        {
            _isMoving = isCurrentlyMoving;

            if (_isMoving)
            {
                // Store starting position when animation begins (for root motion compensation)
                if (_modelStartPosition == Vector3.Zero)
                {
                    _modelStartPosition = Model.Position;
                }
                
                // Start walking animation with looping
                if (AnimationPlayer.HasAnimation(WalkAnimationName))
                {
                    AnimationPlayer.Play(WalkAnimationName);
                    // Ensure the animation loops
                    var animation = AnimationPlayer.GetAnimation(WalkAnimationName);
                    if (animation != null)
                    {
                        animation.LoopMode = Animation.LoopModeEnum.Linear;
                    }
                    Logger.Debug($"Playing walking animation: {WalkAnimationName} (with root motion compensation)");
                }
                else
                {
                    Logger.Warning($"Walking animation '{WalkAnimationName}' not found");
                }
            }
            else
            {
                // Reset position tracking when stopped
                _modelStartPosition = Vector3.Zero;
                
                // Stop and play idle animation
                if (AnimationPlayer.HasAnimation(IdleAnimationName))
                {
                    AnimationPlayer.Play(IdleAnimationName);
                    // Ensure idle animation loops too
                    var animation = AnimationPlayer.GetAnimation(IdleAnimationName);
                    if (animation != null)
                    {
                        animation.LoopMode = Animation.LoopModeEnum.Linear;
                    }
                    Logger.Debug($"Playing idle animation: {IdleAnimationName} (looping)");
                }
                else
                {
                    // Just stop the current animation if no idle animation exists
                    AnimationPlayer.Stop();
                    Logger.Debug("Stopping animation (no idle animation found)");
                }
            }
        }
    }
}
