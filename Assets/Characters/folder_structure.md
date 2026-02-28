# Character Asset Organization

## Recommended Folder Structure:

```
Assets/
├── Characters/
│   ├── Meshes/          # 3D models (.fbx, .gltf, .blend)
│   │   ├── Knight/
│   │   ├── Wizard/
│   │   ├── Archer/
│   │   └── ...
│   ├── Scenes/          # Complete character scenes (.tscn)
│   │   ├── Knight.tscn
│   │   ├── Wizard.tscn
│   │   └── ...
│   ├── Animations/      # Animation libraries and sets
│   │   ├── Sets/        # AnimationSet resources (.tres)
│   │   │   ├── KnightAnimations.tres
│   │   │   ├── HumanoidBasic.tres
│   │   │   └── CreatureAnimations.tres
│   │   ├── Libraries/   # AnimationLibrary resources (.tres)
│   │   │   ├── Combat.tres
│   │   │   ├── Movement.tres
│   │   │   └── Emotes.tres
│   │   └── Raw/         # Raw animation files (.fbx)
│   │       ├── Mixamo/
│   │       ├── Custom/
│   │       └── ...
│   ├── Textures/        # Character textures
│   │   ├── Knight/
│   │   │   ├── Diffuse.png
│   │   │   ├── Normal.png
│   │   │   └── ...
│   │   └── ...
│   └── Materials/       # Character materials (.tres)
│       ├── Knight_Material.tres
│       └── ...
├── UI/
├── Audio/
└── Environment/
```

## Usage with CharacterManager:
- **Available Meshes**: Point to files in `Assets/Characters/Scenes/`
- **Available Animation Sets**: Point to files in `Assets/Characters/Animations/Sets/`
- **Textures/Materials**: Organized by character for easy swapping