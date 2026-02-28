using Godot;

public partial class InputComponent : Node
{
    public Vector2 MoveDirection = Vector2.Zero;
    public bool IsJumpPressed = false;

    public void UpdateInput()
    {
        MoveDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        IsJumpPressed = Input.IsActionPressed("jump");
    }

}
