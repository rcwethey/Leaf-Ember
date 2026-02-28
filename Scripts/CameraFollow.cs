using Godot;
using System;

public partial class CameraFollow : Camera3D
{

	public Node3D _SpringPosition;
	public float LerpPower = 1.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_SpringPosition = GetNodeOrNull<Node3D>("../SpringArm3D/SpringPosition");
		Logger.Debug($"SpringPosition found: {_SpringPosition != null}");
		if (_SpringPosition == null)
		{
			Logger.Error("Could not find SpringPosition node at path: ../SpringArm3D/SpringPosition");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_SpringPosition != null)
		{
			Position = Position.Lerp(_SpringPosition.Position, (float)(delta * LerpPower));
		}
	}
}