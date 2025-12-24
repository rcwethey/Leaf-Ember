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

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("SpringArm3D/Camera3D");
	}


	public override void _PhysicsProcess(double delta)
	{
		// _logTimer += delta;
		// if (_logTimer >= 0.2) // every 0.2s
		// {
		// 	_logTimer = 0;
		// 	GD.Print($"pos={GlobalPosition}  vel={Velocity}  onFloor={IsOnFloor()}");
		// }

		if (!IsOnFloor())
			Velocity += Vector3.Down * Gravity * (float)delta;

		if (Input.IsActionPressed("jump") && IsOnFloor())
		{
			Velocity = new Vector3(Velocity.X, JumpVelocity, Velocity.Z);
		}

		Vector2 input = Input.GetVector("move_left", "move_right", "move_back", "move_forward");

		// camera-relative basis (use Camera3D if you can, else pivot)
		Vector3 forward = -_camera.GlobalTransform.Basis.Z;
		Vector3 right = _camera.GlobalTransform.Basis.X;

		forward.Y = 0;
		right.Y = 0;

		forward = forward.Normalized();
		right = right.Normalized();

		Vector3 direction = forward * input.Y + right * input.X;
		if (direction.Length() > 1f)
		{
			direction = direction.Normalized();
		}
		else
		{
			direction.MoveToward(Vector3.Zero, MoveSpeed * (float)delta);
		}

		Vector3 targetVelocity = direction * MoveSpeed;
		Velocity = Velocity.Lerp(new Vector3(targetVelocity.X, Velocity.Y, targetVelocity.Z), Acceleration * (float)delta);

		MoveAndSlide();
	}
}
