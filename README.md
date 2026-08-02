# Leaf & Ember

Leaf & Ember is an intimate craftsmanship and management RPG about building a respected cigar house. Players grow and source tobacco, guide it through curing, fermentation, and aging, design and construct cigars, train named artisans, and earn a reputation through the work they release.

The game values mastery, provenance, relationships, and a recognizable house identity over enormous farms or automated production. Read [`VISION.md`](VISION.md) before proposing gameplay features.

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

Read these files in order before changing gameplay systems:

1. [`VISION.md`](VISION.md)
2. [`GAMEPLAY.md`](GAMEPLAY.md)
3. [`docs/design/README.md`](docs/design/README.md)
4. [`PROJECT.md`](PROJECT.md)
5. [`ARCHITECTURE.md`](ARCHITECTURE.md)
6. [`ROADMAP.md`](ROADMAP.md)
7. [`CODING_STANDARDS.md`](CODING_STANDARDS.md)

System-specific notes live in `docs/`. The cigar-craft loop and market economy now have canonical direction. The next design focus is characters and relationships: the named people through whom teaching, employment, trade, feedback, and reputation operate.
