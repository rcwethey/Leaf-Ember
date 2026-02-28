
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
		Logger.Info("Initializing PlayerController");

		_camera = GetNodeOrNull<Camera3D>("SpringArmPivot/Camera3D");
		if (_camera == null)
		{
			Logger.Error("Camera3D node not found in SpringArm3D. Please ensure the scene has a Camera3D as a child of SpringArm3D.");
			return;
		}
		Logger.Debug("Camera3D found and assigned successfully");

		// Add or get components
		_inputComponent = GetNodeOrNull<InputComponent>("InputComponent");
		if (_inputComponent == null)
		{
			Logger.Warning("InputComponent not found in scene, creating new instance");
			_inputComponent = new InputComponent();
			AddChild(_inputComponent);
		}
		Logger.Debug("InputComponent found in scene");


		_moveComponent = GetNodeOrNull<MoveComponent>("MoveComponent");
		if (_moveComponent == null)
		{
			Logger.Warning("MoveComponent not found in scene, creating new instance");
			_moveComponent = new MoveComponent();
			AddChild(_moveComponent);
		}
		Logger.Debug("MoveComponent found in scene");


		// Set up MoveComponent references
		_moveComponent.CharacterBody = this; // This PlayerController is the CharacterBody3D

		// Check if using modular character system
		var characterManager = GetNodeOrNull<CharacterManager>("CharacterManager");
		if (characterManager != null)
		{
			Logger.Info("Using modular character system");
			_moveComponent.UseModularAnimations = true;
			// CharacterManager will handle setting up Model and AnimationPlayer
		}
		else
		{
			// Use legacy direct references
			_moveComponent.Model = GetNodeOrNull<Node3D>("Knight"); // The actual visible character model

			if (_moveComponent.Model == null)
			{
				Logger.Error("Knight node not found");	
				return;
			}
			Logger.Debug($"MoveComponent model set to: {_moveComponent.Model.Name} (visible: {_moveComponent.Model.Visible})");

			// Set up AnimationPlayer reference
			_moveComponent.AnimationPlayer = GetNodeOrNull<AnimationPlayer>("Knight/AnimationPlayer");
			if (_moveComponent.AnimationPlayer == null)
			{
				Logger.Warning("AnimationPlayer not found. Character animations will not play.");
			}
			else
			{
				Logger.Debug($"AnimationPlayer found: {_moveComponent.AnimationPlayer.Name}");
			}
		}
		Logger.Info("PlayerController initialization complete");
	}


	public override void _PhysicsProcess(double delta)
	{
		_inputComponent.UpdateInput();

		// Get raw input direction
		Vector2 inputDirection = _inputComponent.MoveDirection;

		// Transform movement direction based on camera rotation for camera-relative movement
		if (inputDirection.LengthSquared() > 0.001f)
		{
			float cameraYRotation = _camera.GlobalTransform.Basis.GetEuler().Y;
			// Convert 2D input to 3D, rotate based on camera Y, then back to 2D
			Vector3 direction3D = new Vector3(inputDirection.X, 0, inputDirection.Y);
			direction3D = direction3D.Rotated(Vector3.Up, cameraYRotation);
			inputDirection = new Vector2(direction3D.X, direction3D.Z);
		}

		_moveComponent.Direction = inputDirection;
		_moveComponent.WantsToJump = _inputComponent.IsJumpPressed;
		_moveComponent.Tick(delta);
	}
}
