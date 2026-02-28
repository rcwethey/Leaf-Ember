using Godot;

public partial class InputComponent : Node
{
    public Vector2 MoveDirection = Vector2.Zero;
    public bool IsJumpPressed = false;

    public void UpdateInput()
    {
        MoveDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        IsJumpPressed = Input.IsActionPressed("jump");
    }

}
