# Leaf & Ember

Leaf & Ember is an intimate craftsmanship and management RPG about building a respected cigar house. Players grow and source tobacco, guide it through curing, fermentation, and aging, design and construct cigars, train named artisans, and earn a reputation through the work they release.

The game values mastery, provenance, relationships, and a recognizable house identity over enormous farms or automated production. Read [`VISION.md`](VISION.md) before proposing gameplay features.

## Project status

This repository is the Unity continuation of an earlier Godot prototype. The original prototype is preserved on the archive/godot-prototype branch:

https://github.com/rcwethey/Leaf-Ember/tree/archive/godot-prototype

The technical foundation and all pre-implementation design gates are complete. Milestones 1 and 2 provide the embodied cigar-development loop, and Milestone 3A rebuilds the finca and player experience around it:

- Unity 6 LTS with the Universal Render Pipeline
- first-person movement and camera control
- a landmark-driven 144 by 112 meter finca with separated production, residential, social, field, and arrival areas
- imported Blender-authored architecture, terrain, tobacco-production props, CC0 PBR surfaces and vegetation, and direct object interaction
- opening orientation, dynamic craft objectives, contextual what/why/cost guidance, and an in-game glossary
- free inspection and confirmed block-costing focused work
- the three-block, 96-day calendar with scheduled checkpoints and work summaries
- four provenance-aware estate and sourced leaf lots
- blend intent, versioned recipe construction, hard quality-control evidence, and required rest
- staged perspective tasting, intent comparison, causal diagnosis, revision, and tradeoff comparison
- versioned JSON persistence for player, calendar, estate, inventory, and cigar-development state
- 23 passing Edit Mode tests and a passing Play Mode startup smoke test

Milestone 3A's replacement environment is implemented and automated-test/render validated after the first hands-on review rejected the earlier performance and graybox-derived presentation. The new candidate uses imported authored art, measured LODs, and hard rendering budgets. Milestone 3B remains blocked until this candidate clears hands-on play review.

## Requirements

- Unity 6000.5.4f1
- Git LFS
- Windows development environment

Clone with LFS enabled, then open the repository root through Unity Hub. The startup scene is Assets/LeafEmber/Scenes/Bootstrap.unity.

Enter Play Mode to walk the finca. Use WASD and the mouse, press E to interact, G to open the craft glossary, F5 to save, F9 to reload, and Escape to close focused views. The opening orientation and current objective introduce the route. The east-side workshop starts cigar development; the central tasting patio continues it after the required rest. See [`docs/implementation/MILESTONE_3A.md`](docs/implementation/MILESTONE_3A.md) for the rebuilt experience and [`docs/implementation/MILESTONE_2.md`](docs/implementation/MILESTONE_2.md) for the complete cigar loop.

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
9. [`docs/implementation/MILESTONE_2.md`](docs/implementation/MILESTONE_2.md)
10. [`docs/implementation/MILESTONE_3A.md`](docs/implementation/MILESTONE_3A.md)
11. [`docs/art/ASSET_SOURCING.md`](docs/art/ASSET_SOURCING.md)

System-specific notes live in `docs/`. Canonical design documents dictate intent; implementation notes record what the current prototype actually proves and where it remains deliberately temporary.
