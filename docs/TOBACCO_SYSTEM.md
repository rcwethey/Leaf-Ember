# Tobacco system

**Status:** Canonical data and sensory principles
**Last reviewed:** 2026-08-02

## Core principle

Every meaningful tobacco lot and cigar has provenance. Quality and identity emerge from agricultural origin, accumulated process history, blend and construction choices, storage, and maker.

## Provenance

Track at minimum where applicable:

- Seed definition and seed lot
- Grower, finca, field, and region
- Crop, season, harvest date, and plant position
- Cultivation and material condition
- Curing history
- Sorting and grade history
- Fermentation history
- Resting and aging history
- Ownership, contracts, reservations, and allocation
- Blend and construction specification
- Prototype or production batch
- Roller or responsible artisan
- Cigar aging and release history

Transformations create traceable descendants rather than erasing their inputs. A cigar must be able to identify every source lot in its blend.

## Granularity

The fiction treats leaves and cigars as individual physical objects; the implementation may safely aggregate them into lots and batches.

Aggregation is valid only when every gameplay-relevant property is compatible, including provenance, process state, grade, condition, ownership, and reservation. Splitting a lot preserves history on every child lot. Merging is prohibited when it would erase a meaningful distinction.

## Process stages

The broad material flow is:

```text
seed -> crop -> harvested leaf -> curing -> sorting and grading
     -> fermentation -> resting or aging -> blend allocation
     -> construction -> cigar batch -> cigar aging -> release
```

Stage history must be data, not flavor text. Time, environment, interventions, responsible characters, and exceptional events may affect later behavior.

## Use and grade

Wrapper, binder, and filler suitability should emerge from cultivar, cultivation intent, plant position, physical condition, and grading. The system should not assume that every harvested leaf achieves its intended role.

Grade and price are not universal quality scores. A visually imperfect leaf may be valuable in filler; an attractive wrapper may be wrong for a particular blend.

## Cigars

No two cigars should be assumed identical merely because they share a recipe. Individual or batch results can vary through material condition, construction, responsible artisan, storage, and tolerances.

An approved cigar specification records intent and acceptable variation. It does not overwrite the actual history of a batch.

## Sensory authority

Flavor, perception, blend interaction, and diagnosis are defined by:

- [`design/FLAVOR_AND_SENSORY.md`](design/FLAVOR_AND_SENSORY.md)
- [`design/PROCESS_TRANSFORMATION.md`](design/PROCESS_TRANSFORMATION.md)
- [`design/BLENDING_AND_RECIPES.md`](design/BLENDING_AND_RECIPES.md)
- [`design/CONSTRUCTION.md`](design/CONSTRUCTION.md)
- [`design/TASTING_AND_DIAGNOSIS.md`](design/TASTING_AND_DIAGNOSIS.md)

Numerical calibration remains open, but implementations must preserve separate experience dimensions, hidden expression versus taster perception, staged evolution, diagnosis, and audience-specific judgment. Do not introduce a single generic flavor or cigar-quality value.

See [`design/LEAF_ECONOMY.md`](design/LEAF_ECONOMY.md) for sourcing and allocation.
