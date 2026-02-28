# Character Asset Organization Guide

## Folder Structure Created

I've set up a comprehensive character asset organization system that works perfectly with the modular CharacterManager system.

## 📁 **Directory Overview:**

### **Assets/Characters/Meshes/**
- **Purpose**: Store raw 3D model files (.fbx, .gltf, .blend)
- **Organization**: One subfolder per character
- **Example**: `Knight/knight_model.fbx`

### **Assets/Characters/Scenes/**  
- **Purpose**: Complete character scenes (.tscn) ready for use
- **Contents**: Mesh + AnimationPlayer + collision setup
- **Usage**: Reference these in CharacterManager's "Available Meshes" array

### **Assets/Characters/Animations/**
- **Sets/**: AnimationSet resources (.tres) - what CharacterManager uses
- **Libraries/**: AnimationLibrary resources (.tres) - grouped animations 
- **Raw/**: Raw animation files (.fbx) from Mixamo, etc.

### **Assets/Characters/Textures/**
- **Purpose**: All character textures (diffuse, normal, etc.)
- **Organization**: One subfolder per character

### **Assets/Characters/Materials/**
- **Purpose**: Godot material resources (.tres)
- **Benefits**: Reusable materials for different characters

## 🔧 **Workflow:**

### **1. Adding a New Character:**
```
1. Import mesh → Assets/Characters/Meshes/[CharacterName]/
2. Create textures → Assets/Characters/Textures/[CharacterName]/  
3. Create material → Assets/Characters/Materials/[CharacterName]_Material.tres
4. Build scene → Assets/Characters/Scenes/[CharacterName].tscn
5. Add to CharacterManager's Available Meshes array
```

### **2. Adding New Animations:**
```
1. Import animations → Assets/Characters/Animations/Raw/
2. Create AnimationLibrary → Assets/Characters/Animations/Libraries/
3. Create AnimationSet → Assets/Characters/Animations/Sets/
4. Add to CharacterManager's Available Animation Sets array
```

## 🎯 **Benefits:**
- **Clear Separation**: Meshes, animations, and materials are separate
- **Easy Swapping**: CharacterManager can mix and match easily
- **Team Friendly**: Artists know exactly where to put assets
- **Version Control**: Easy to track changes to specific assets
- **Scalable**: Works for 10 characters or 100+

## 💡 **Pro Tips:**
- Name your AnimationSets descriptively (e.g., "HumanoidBasic", "CreaturePack1")
- Keep texture folders organized by character for easy material creation
- Use the Raw animations folder as your "working" area before organizing into Libraries