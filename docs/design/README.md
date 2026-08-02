# Design documentation

**Status:** Canonical index
**Last reviewed:** 2026-08-02

This directory turns the creative direction into implementation constraints. Design and engineering work should begin with the documents below rather than inferring the game from the current prototype.

## Reading order

1. [`VISION.md`](../../VISION.md) - product identity, pillars, and non-goals
2. [`GAMEPLAY.md`](../../GAMEPLAY.md) - the complete player loop
3. [`WORLD_AND_ESTATE.md`](WORLD_AND_ESTATE.md) - premise, setting, finca, city, and community
4. [`SETTING_AND_NARRATIVE.md`](SETTING_AND_NARRATIVE.md) - geography, period, founder, story structure, language, and cultural research
5. [`CHARACTERS_AND_RELATIONSHIPS.md`](CHARACTERS_AND_RELATIONSHIPS.md) - memory, commitments, artisans, friendship, conflict, and character agency
6. [`CRAFT_AND_GROWTH.md`](CRAFT_AND_GROWTH.md) - personal work, delegation, artisans, and expansion
7. [`TIME_AND_PIPELINE.md`](TIME_AND_PIPELINE.md) - calendar philosophy and overlapping production
8. [`FACILITIES_AND_ESTATE_PROGRESSION.md`](FACILITIES_AND_ESTATE_PROGRESSION.md) - restoration, facility abstraction, construction, capacity, and visible growth
9. [`KNOWLEDGE_AND_LONG_TERM_PROGRESSION.md`](KNOWLEDGE_AND_LONG_TERM_PROGRESSION.md) - evidence, technique, standards, opportunity, archive, and legacy
10. [`LEAF_ECONOMY.md`](LEAF_ECONOMY.md) - terroir, crop allocation, sourcing, and trade
11. [`PROCESS_TRANSFORMATION.md`](PROCESS_TRANSFORMATION.md) - curing, fermentation, resting, and aging
12. [`FLAVOR_AND_SENSORY.md`](FLAVOR_AND_SENSORY.md) - expression, perception, vocabulary, and evaluation
13. [`BLENDING_AND_RECIPES.md`](BLENDING_AND_RECIPES.md) - blend interactions, prototypes, specifications, and scaling
14. [`CONSTRUCTION.md`](CONSTRUCTION.md) - personal rolling, physical results, artisan skill, and quality control
15. [`TASTING_AND_DIAGNOSIS.md`](TASTING_AND_DIAGNOSIS.md) - tasting sessions, feedback, hypotheses, and revision
16. [`MARKET_AND_ECONOMY.md`](MARKET_AND_ECONOMY.md) - buyers, releases, contracts, pricing, cash flow, and recovery
17. [`REPUTATION.md`](REPUTATION.md) - audiences, relationships, and house identity
18. [`OPEN_QUESTIONS.md`](OPEN_QUESTIONS.md) - unresolved systems and the next design work

System implementation notes under `docs/` must conform to this design layer. `ROADMAP.md` determines sequence; it does not overrule the vision.

## Decision states

- **Canonical:** Approved direction. Implementations must preserve it unless the decision is deliberately revisited.
- **Directional:** Preferred approach that still needs detailed system design or prototyping.
- **Open:** A question that must not be silently treated as settled.

When a canonical decision changes, update every affected document in the same change. Do not leave conflicting versions of the game's direction in the repository.

## Current design boundary

The setting, narrative structure, estate structure, growth philosophy, time philosophy, production paths, facilities, long-term progression, leaf economy, process transformation, flavor and perception, blending, personal construction, tasting, market economy, character system, and high-level reputation direction are canonical.

Numerical formulas, exact thresholds, gesture details, and interface layouts require prototypes, but those prototypes must honor the canonical model. Calendar tuning and pacing are the next design focus.
