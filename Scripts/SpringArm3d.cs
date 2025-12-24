using Godot;

public partial class SpringArm3d : SpringArm3D
{

	[Export] public float turnRate = 120f;
	[Export] public float MouseSensitivity = 0.7f;
	[Export] public float MinPitch = -60f;
	[Export] public float MaxPitch = 12f;
	[Export] public NodePath CameraPath;

	private float _pitch;
	private Vector2 mouseInput = new Vector2();
	private Camera3D _camera;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_camera = GetNode<Camera3D>(CameraPath);
		this.SpringLength = _camera.Position.Z;

		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 input = Input.GetVector("view_left", "view_right", "view_down", "view_up");
		input = input * (float)delta * turnRate;
		input += mouseInput;
		mouseInput = new Vector2();

		var rotationDegrees = this.RotationDegrees;
		rotationDegrees.X += input.Y;
		rotationDegrees.Y += -input.X;
		rotationDegrees.X = Mathf.Clamp(rotationDegrees.X, MinPitch, MaxPitch);
		this.RotationDegrees = rotationDegrees;

	}

	public override void _Input(InputEvent e)
	{
		if (e is InputEventMouseMotion motion)
		{
			mouseInput = motion.Relative * MouseSensitivity;
		}
		else if (e is InputEventKey keyEvent)
		{
			if (keyEvent.Keycode == Key.Escape && keyEvent.Pressed)
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
		}
		else if (e is InputEventMouseButton mouseButtonEvent)
		{
			if (mouseButtonEvent.ButtonIndex == MouseButton.Right && Input.MouseMode == Input.MouseModeEnum.Visible)
			{
				Input.MouseMode = Input.MouseModeEnum.Captured;
			}
		}
	}
}
