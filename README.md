# Leaf & Ember

Leaf & Ember is an intimate craftsmanship and management RPG about building a respected cigar house. Players grow and source tobacco, guide it through curing, fermentation, and aging, design and construct cigars, train named artisans, and earn a reputation through the work they release.

The game values mastery, provenance, relationships, and a recognizable house identity over enormous farms or automated production. Read [`VISION.md`](VISION.md) before proposing gameplay features.

## Project status

This repository is the Unity continuation of an earlier Godot prototype. The original prototype is preserved on the archive/godot-prototype branch:

https://github.com/rcwethey/Leaf-Ember/tree/archive/godot-prototype

The technical foundation and all pre-implementation design gates are complete. Milestone 1 now provides a playable embodied-finca prototype:

- Unity 6 LTS with the Universal Render Pipeline
- first-person movement and camera control
- a six-zone graybox production route plus finca office
- free inspection and confirmed block-costing focused work
- the three-block, 96-day calendar with scheduled checkpoints and work summaries
- provenance-aware leaf-lot inspection
- versioned JSON persistence for player, calendar, estate, and inventory state
- deterministic Edit Mode coverage and a Play Mode startup smoke test

Milestone 2, the cigar-development prototype, is now active.

## Requirements

- Unity 6000.5.4f1
- Git LFS
- Windows development environment

Clone with LFS enabled, then open the repository root through Unity Hub. The startup scene is Assets/LeafEmber/Scenes/Bootstrap.unity.

Enter Play Mode to walk the finca. Use WASD and the mouse, press E to interact, F5 to save, F9 to reload, and Escape to close focused views. See [`docs/implementation/MILESTONE_1.md`](docs/implementation/MILESTONE_1.md) for the current prototype contract and playtest notes.

## Documentation

Read these files in order before changing gameplay systems:

1. [`VISION.md`](VISION.md)
2. [`GAMEPLAY.md`](GAMEPLAY.md)
3. [`docs/design/README.md`](docs/design/README.md)
4. [`PROJECT.md`](PROJECT.md)
5. [`ARCHITECTURE.md`](ARCHITECTURE.md)
6. [`ROADMAP.md`](ROADMAP.md)
7. [`CODING_STANDARDS.md`](CODING_STANDARDS.md)
8. [`docs/implementation/MILESTONE_1.md`](docs/implementation/MILESTONE_1.md)

System-specific notes live in `docs/`. Canonical design documents dictate intent; implementation notes record what the current prototype actually proves and where it remains deliberately temporary.
