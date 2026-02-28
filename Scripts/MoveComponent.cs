using Godot;

public partial class MoveComponent : Node
{
    [Export] public Node3D Model;
    [Export] public CharacterBody3D CharacterBody;
    [Export] public float Acceleration = 8.0f;
    [Export] public float Speed = 4.0f;
    [Export] public float Gravity = 9.8f;
    [Export] public float JumpVelocity = 4.5f;
    [Export] public float RotationSpeed = 8.0f;

    public Vector2 Direction = Vector2.Zero;
    public bool WantsToJump = false;

    public void Tick(double delta)
    {
        if (CharacterBody == null || Model == null)
        {
            GD.PrintErr("MoveComponent requires a reference to the CharacterBody3D and Model nodes.");
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
            // Rotate the model to face the movement direction
            float targetAngle = Mathf.Atan2(Direction.X, Direction.Y);
            float currentAngle = Model.Rotation.Y;
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, RotationSpeed * (float)delta);
            Model.Rotation = new Vector3(Model.Rotation.X, newAngle, Model.Rotation.Z);
        }
    }
}
