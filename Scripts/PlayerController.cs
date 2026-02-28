
using Godot;

public partial class PlayerController : CharacterBody3D
{
	[Export] public float MoveSpeed = 4.0f;
	[Export] public float Acceleration = 8.0f;
	[Export] public float RotationSpeed = 8.0f;
	[Export] public float Gravity = 9.8f;
	[Export] public float JumpVelocity = 4.5f;

	// private Node3D _cameraPivot;
	private Camera3D _camera;
	private double _logTimer = 0;

    private InputComponent _inputComponent;
    private MoveComponent _moveComponent;

	public override void _Ready()
	{
		_camera = GetNodeOrNull<Camera3D>("SpringArm3D/Camera3D");
		if (_camera == null)
		{
			GD.PrintErr("Camera3D node not found in SpringArm3D. Please ensure the scene has a Camera3D as a child of SpringArm3D.");
			return;		
		}

        // Add or get components
        _inputComponent = GetNodeOrNull<InputComponent>("InputComponent");
        if (_inputComponent == null)
        {
            _inputComponent = new InputComponent();
            AddChild(_inputComponent);
        }

        _moveComponent = GetNodeOrNull<MoveComponent>("MoveComponent");
        if (_moveComponent == null)
        {
            _moveComponent = new MoveComponent();
            AddChild(_moveComponent);
        }
	}


	public override void _PhysicsProcess(double delta)
	{
		if (!IsOnFloor())
			Velocity += Vector3.Down * Gravity * (float)delta;

		if (Input.IsActionPressed("jump") && IsOnFloor())
		{
			Velocity = new Vector3(Velocity.X, JumpVelocity, Velocity.Z);
		}

		// Update input component
		_inputComponent._Process(delta);

		// camera-relative basis
		Vector3 forward = -_camera.GlobalTransform.Basis.Z;
		Vector3 right = _camera.GlobalTransform.Basis.X;
		forward.Y = 0;
		right.Y = 0;
		forward = forward.Normalized();
		right = right.Normalized();

		Vector2 input = _inputComponent.MovementInput;
		Vector3 direction = forward * input.Y + right * input.X;
		if (direction.Length() > 1f)
		{
			direction = direction.Normalized();
		}

		// Use MoveComponent to move
		if (_moveComponent != null)
		{
			Velocity = _moveComponent.CalculateVelocity(Velocity, direction, (float)delta);
			MoveAndSlide();
		}
	}
}
