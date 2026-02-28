using Godot;

public partial class CharacterManager : Node3D
{
    [Export] public PackedScene[] AvailableMeshes;
    [Export] public AnimationSet[] AvailableAnimationSets;
    [Export] public int CurrentMeshIndex = 0;
    [Export] public int CurrentAnimationSetIndex = 0;

    private Node3D _currentMesh;
    private AnimationPlayer _currentAnimationPlayer;
    private MoveComponent _moveComponent;

    public override void _Ready()
    {
        _moveComponent = GetParent().GetNodeOrNull<MoveComponent>("MoveComponent");
        LoadCharacterMesh(CurrentMeshIndex);
        LoadAnimationSet(CurrentAnimationSetIndex);
    }

    public void LoadCharacterMesh(int meshIndex)
    {
        if (meshIndex < 0 || meshIndex >= AvailableMeshes.Length) return;

        // Remove old mesh
        if (_currentMesh != null)
        {
            _currentMesh.QueueFree();
        }

        // Load new mesh
        var meshScene = AvailableMeshes[meshIndex].Instantiate<Node3D>();
        AddChild(meshScene);
        _currentMesh = meshScene;

        // Find AnimationPlayer in the mesh
        _currentAnimationPlayer = FindAnimationPlayer(_currentMesh);

        // Update MoveComponent references
        if (_moveComponent != null)
        {
            _moveComponent.Model = _currentMesh;
            _moveComponent.AnimationPlayer = _currentAnimationPlayer;
        }

        Logger.Info($"Loaded character mesh: {meshScene.Name}");
    }

    public void LoadAnimationSet(int animationSetIndex)
    {
        if (animationSetIndex < 0 || animationSetIndex >= AvailableAnimationSets.Length) return;
        if (_currentAnimationPlayer == null) return;

        var animationSet = AvailableAnimationSets[animationSetIndex];
        
        // Clear old animation libraries
        var libraries = _currentAnimationPlayer.GetAnimationLibraryList();
        foreach (var libName in libraries)
        {
            if (libName != "default") // Keep default library if it exists
            {
                _currentAnimationPlayer.RemoveAnimationLibrary(libName);
            }
        }

        // Add new animation library
        if (animationSet.AnimationLibrary != null)
        {
            _currentAnimationPlayer.AddAnimationLibrary(animationSet.SetName, animationSet.AnimationLibrary);
        }

        // Update MoveComponent animation names
        if (_moveComponent != null)
        {
            _moveComponent.WalkAnimationName = $"{animationSet.SetName}/{animationSet.WalkAnimation}";
            _moveComponent.IdleAnimationName = $"{animationSet.SetName}/{animationSet.IdleAnimation}";
        }

        Logger.Info($"Loaded animation set: {animationSet.SetName}");
    }

    private AnimationPlayer FindAnimationPlayer(Node node)
    {
        if (node is AnimationPlayer animPlayer) 
            return animPlayer;

        foreach (Node child in node.GetChildren())
        {
            var result = FindAnimationPlayer(child);
            if (result != null) return result;
        }

        return null;
    }

    // Runtime character/animation swapping methods
    public void SwitchToNextMesh()
    {
        CurrentMeshIndex = (CurrentMeshIndex + 1) % AvailableMeshes.Length;
        LoadCharacterMesh(CurrentMeshIndex);
        LoadAnimationSet(CurrentAnimationSetIndex); // Reapply animations
    }

    public void SwitchToNextAnimationSet()
    {
        CurrentAnimationSetIndex = (CurrentAnimationSetIndex + 1) % AvailableAnimationSets.Length;
        LoadAnimationSet(CurrentAnimationSetIndex);
    }

    // Input handling for testing
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("switch_character")) // You'll need to define this input
        {
            SwitchToNextMesh();
        }
        else if (@event.IsActionPressed("switch_animations")) // You'll need to define this input
        {
            SwitchToNextAnimationSet();
        }
    }
}