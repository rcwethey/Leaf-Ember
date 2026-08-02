# Farm simulation

**Status:** Directional implementation notes
**Last reviewed:** 2026-08-02

## Purpose

The farm produces expressive, provenance-rich tobacco for the cigar-craft loop. It is not an acreage-maximization game. The player manages a modest number of meaningful plots, learns the finca's tendencies, and eventually delegates routine care.

Canonical design constraints are defined in [`design/LEAF_ECONOMY.md`](design/LEAF_ECONOMY.md) and [`design/TIME_AND_PIPELINE.md`](design/TIME_AND_PIPELINE.md).

## Entities

- **Finca:** Estate identity, location, facilities, environmental context, and ownership state
- **Field plot:** Soil, exposure, drainage, microclimate tendencies, improvements, and crop history
- **Crop:** Seed lot, planting cohort, cultivation plan, growth state, and accumulated stresses
- **Plant or plant group:** Simulation unit for growth and harvest where finer granularity is meaningful
- **Harvested leaf lot:** Provenance-preserving result grouped by compatible field, seed, plant position, timing, and condition
- **Farm task:** Work requirement, timing window, assigned character, completion quality, and observed result

The simulation may aggregate homogeneous plants and leaves for performance, but it must never merge units whose gameplay-relevant provenance or condition differs.

## Plot tendencies

Plots influence ranges rather than prescribing a single outcome. Relevant tendencies may include:

- Soil and nutrient behavior
- Sun and shade exposure
- Drainage and water stress
- Disease pressure
- Yield range
- Leaf thickness, elasticity, strength, combustion, and broad sensory tendencies

Exact sensory attributes remain dependent on the future flavor-system design.

## Daily tick

The deterministic daily simulation order is:

1. Calendar and weather
2. Plot water, soil, and facility environment
3. Crop growth and accumulated stress
4. Disease and risk progression
5. Assigned labor and interventions
6. Harvest-window and readiness evaluation
7. Warnings, summaries, and state-change events

Each stage consumes the previous stage's results and publishes state changes only after its transaction completes. Simulation results must be reproducible from saved state and controlled randomness.

## Player attention

Routine work can be assigned once the player has people, tools, and clear standards. Personal intervention remains meaningful for diagnosis, experiments, exceptional plots, unusual weather, disease response, and harvest timing.

Problems should progress through readable warning states. Missing one invisible instant must not silently destroy a crop.

## Crop disposition

Harvest is the beginning of allocation, not an automatic sale. Leaves proceed to curing and grading, then become lots that can be retained, sold, traded, contracted, or processed further.

## Acceptance for the first farm subsystem

The player can prepare a modest plot, plant a defined seed lot, assign or perform care, advance time with visible forecasts, observe growth and warnings, harvest into provenance-rich lots, save, quit, reload, and recover the same simulation state.

This is a subsystem acceptance target, not the definition of the game's vertical slice.
