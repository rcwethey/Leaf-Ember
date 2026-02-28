using Godot;

[System.Serializable]
public partial class AnimationSet : Resource
{
    [Export] public string SetName = "";
    [Export] public string IdleAnimation = "idle";
    [Export] public string WalkAnimation = "walking";
    [Export] public string RunAnimation = "running";
    [Export] public string JumpAnimation = "jump";
    [Export] public AnimationLibrary AnimationLibrary;

    public AnimationSet() { }

    public AnimationSet(string name, AnimationLibrary library)
    {
        SetName = name;
        AnimationLibrary = library;
    }
}