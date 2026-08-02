# Leaf & Ember

Leaf & Ember is a craftsmanship simulation about growing tobacco and producing premium cigars. Players manage the full journey from seed and soil through curing, fermentation, aging, blending, rolling, packaging, and sale.

## Project status

This repository is the Unity continuation of an earlier Godot prototype. The original prototype is preserved on the archive/godot-prototype branch:

https://github.com/rcwethey/Leaf-Ember/tree/archive/godot-prototype

Sprint 0 establishes:

- Unity 6 LTS with the Universal Render Pipeline
- Input System project configuration
- Persistent application bootstrap
- Service registry and typed event bus
- Versioned JSON save-system skeleton
- Edit Mode tests for the foundational services
- Migrated character and farming-animation source assets

## Requirements

- Unity 6000.5.4f1
- Git LFS
- Windows development environment

Clone with LFS enabled, then open the repository root through Unity Hub. The startup scene is Assets/LeafEmber/Scenes/Bootstrap.unity.

## Documentation

Read these files before changing gameplay systems:

1. PROJECT.md
2. ARCHITECTURE.md
3. GAMEPLAY.md
4. ROADMAP.md
5. CODING_STANDARDS.md

System-specific notes live in docs/.
