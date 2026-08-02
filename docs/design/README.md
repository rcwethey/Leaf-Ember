# Design documentation

**Status:** Canonical index
**Last reviewed:** 2026-08-02

This directory turns the creative direction into implementation constraints. Design and engineering work should begin with the documents below rather than inferring the game from the current prototype.

## Reading order

1. [`VISION.md`](../../VISION.md) - product identity, pillars, and non-goals
2. [`GAMEPLAY.md`](../../GAMEPLAY.md) - the complete player loop
3. [`WORLD_AND_ESTATE.md`](WORLD_AND_ESTATE.md) - premise, setting, finca, city, and community
4. [`CRAFT_AND_GROWTH.md`](CRAFT_AND_GROWTH.md) - personal work, delegation, artisans, and expansion
5. [`TIME_AND_PIPELINE.md`](TIME_AND_PIPELINE.md) - calendar philosophy and overlapping production
6. [`LEAF_ECONOMY.md`](LEAF_ECONOMY.md) - terroir, crop allocation, sourcing, and trade
7. [`REPUTATION.md`](REPUTATION.md) - audiences, relationships, and house identity
8. [`OPEN_QUESTIONS.md`](OPEN_QUESTIONS.md) - unresolved systems and the next design work

System implementation notes under `docs/` must conform to this design layer. `ROADMAP.md` determines sequence; it does not overrule the vision.

## Decision states

- **Canonical:** Approved direction. Implementations must preserve it unless the decision is deliberately revisited.
- **Directional:** Preferred approach that still needs detailed system design or prototyping.
- **Open:** A question that must not be silently treated as settled.

When a canonical decision changes, update every affected document in the same change. Do not leave conflicting versions of the game's direction in the repository.

## Current design boundary

The setting, estate structure, growth philosophy, time philosophy, production paths, leaf economy, and high-level reputation direction are canonical.

The detailed flavor-profile and sensory-learning system is intentionally unresolved. It is the next design focus and should be documented before implementing blend evaluation or cigar-quality scoring.
