using System;
using Godot;

public partial class SpringArm3d : Node3D
{

	[Export] public float MouseSensitivity = 0.3f;

	private SpringArm3D _springArm;

	public override void _Ready()
	{
		_springArm = GetNodeOrNull<SpringArm3D>("SpringArm3D");
		Logger.Debug($"SpringArm3D found: {_springArm != null}");
		
		// Ensure MouseSensitivity is not zero (editor might override it)
		if (MouseSensitivity <= 0.0f)
		{
			MouseSensitivity = 0.3f;
			Logger.Warning($"MouseSensitivity was zero, setting to default: {MouseSensitivity}");
		}
		
		Input.MouseMode = Input.MouseModeEnum.Captured;
		Logger.Info($"Mouse mode set to Captured. Current mode: {Input.MouseMode}. Sensitivity: {MouseSensitivity}");
	}

	public override void _UnhandledInput(InputEvent evt)
	{
		if (evt is InputEventMouseMotion motion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			var rotationDegrees = this.RotationDegrees;
			rotationDegrees.X -= motion.Relative.Y * MouseSensitivity;
			rotationDegrees.X = Mathf.Clamp(rotationDegrees.X, -90.0f, 45.0f);

			rotationDegrees.Y -= motion.Relative.X * MouseSensitivity;
			rotationDegrees.Y = Mathf.Wrap(rotationDegrees.Y, 0.0f, 360.0f);
			this.RotationDegrees = rotationDegrees;
			
			// Log only occasionally to avoid spam
			if (Math.Abs(motion.Relative.X) > 5 || Math.Abs(motion.Relative.Y) > 5)
			{
				Logger.Debug($"Applied rotation: {this.RotationDegrees} (sensitivity: {MouseSensitivity})");
			}
		}

		if (evt.IsActionPressed("wheel_up") && _springArm != null)
		{
			_springArm.SpringLength = Mathf.Max(_springArm.SpringLength - 0.5f, 1.0f);
			Logger.Debug($"Zoom in - SpringLength: {_springArm.SpringLength}");
		}
		else if (evt.IsActionPressed("wheel_down") && _springArm != null)
		{
			_springArm.SpringLength = Mathf.Min(_springArm.SpringLength + 0.5f, 10.0f);
			Logger.Debug($"Zoom out - SpringLength: {_springArm.SpringLength}");
		}

		if (evt.IsActionPressed("toggle_mouse_capture"))
		{
			if (Input.MouseMode == Input.MouseModeEnum.Captured)
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
			else
			{
				Input.MouseMode = Input.MouseModeEnum.Captured;
			}
		}
	}





	// [Export] public float turnRate = 100f;
	// [Export] public float MouseSensitivity = 0.005f;
	// [Export] public float MinPitch = -60f;
	// [Export] public float MaxPitch = 12f;
	// [Export] public NodePath CameraPath;

	// private float _pitch;
	// private Vector2 mouseInput = new Vector2();
	// private Camera3D _camera;
	// // Called when the node enters the scene tree for the first time.
	// public override void _Ready()
	// {
	// 	_camera = GetNode<Camera3D>(CameraPath);
	// 	this.SpringLength = _camera.Position.Z;

	// 	Input.MouseMode = Input.MouseModeEnum.Captured;
	// }

	// // Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// 	Vector2 input = Input.GetVector("view_left", "view_right", "view_down", "view_up");
	// 	input = input * (float)delta * turnRate;
	// 	input += mouseInput;
	// 	mouseInput = new Vector2();

	// 	var rotationDegrees = this.RotationDegrees;
	// 	rotationDegrees.X += input.Y;
	// 	rotationDegrees.Y += -input.X;
	// 	rotationDegrees.X = Mathf.Clamp(rotationDegrees.X, MinPitch, MaxPitch);
	// 	this.RotationDegrees = rotationDegrees;

	// }

	// public override void _Input(InputEvent e)
	// {
	// 	if (e is InputEventMouseMotion motion)
	// 	{
	// 		mouseInput = motion.Relative * MouseSensitivity;
	// 	}
	// 	else if (e is InputEventKey keyEvent)
	// 	{
	// 		if (keyEvent.Keycode == Key.Escape && keyEvent.Pressed)
	// 		{
	// 			Input.MouseMode = Input.MouseModeEnum.Visible;
	// 		}
	// 	}
	// 	else if (e is InputEventMouseButton mouseButtonEvent)
	// 	{
	// 		if (mouseButtonEvent.ButtonIndex == MouseButton.Right && Input.MouseMode == Input.MouseModeEnum.Visible)
	// 		{
	// 			Input.MouseMode = Input.MouseModeEnum.Captured;
	// 		}
	// 	}
	// }
}
