# Roadmap

**Status:** Directional; revisit after each design gate
**Last reviewed:** 2026-08-02

The roadmap serves the canonical vision. It should validate the cigar-craft loop before expanding agricultural scale or production volume.

## Completed - technical foundation

- [x] Unity 6 project and Universal Render Pipeline
- [x] Git and Git LFS configuration
- [x] Input System project configuration
- [x] Persistent bootstrap
- [x] Service registry and typed event bus
- [x] Versioned JSON save skeleton
- [x] Edit Mode foundation tests
- [x] Initial art and animation migration

## Completed - creative foundation

- [x] Product vision, pillars, and non-goals
- [x] Finca, city, community, and cultural direction
- [x] Growth-without-displacement progression
- [x] Action-based time and overlapping-pipeline philosophy
- [x] Terroir, crop allocation, sourcing, and leaf-economy direction
- [x] Audience-specific reputation direction

## Design gate 1 - flavor and sensory model

- [ ] Define the flavor-profile vocabulary and data model
- [ ] Separate strength, body, aroma, taste, construction, and subjective perception
- [ ] Define blend interactions and evolution through a cigar
- [ ] Define player learning, uncertainty, tasting records, and diagnostic feedback
- [ ] Define how characters perceive and describe the same cigar differently
- [ ] Publish a canonical flavor-system design document

Do not implement a final quality formula or blend evaluator before this gate is complete.

## Milestone 1 - embodied finca foundation

- [ ] Player controller and camera
- [ ] Graybox the finca's core production route
- [ ] Interaction framework and focused workbench transitions
- [ ] Calendar, time advancement, scheduled checkpoints, and summaries
- [ ] Provenance-aware inventory and lot inspection
- [ ] Save and reload the player, calendar, estate, and lot state

## Milestone 2 - cigar-development prototype

- [ ] Provide several authored leaf lots with distinct provenance
- [ ] Blend-intent and recipe notebook
- [ ] Prototype preparation and rolling interaction
- [ ] Construction evaluation
- [ ] Sensory evaluation based on the approved flavor model
- [ ] Revision history and comparison between prototypes

## Milestone 3 - first release

- [ ] Commit a recipe and finite leaf allocation
- [ ] Produce and age a tiny batch
- [ ] Package and present the release
- [ ] Sell or submit it to an initial named buyer
- [ ] Receive contextual feedback and update money, knowledge, access, and reputation

## Milestone 4 - estate crop to cured lot

- [ ] Farm, field, seed, crop, and weather definitions
- [ ] Plot preparation, planting, and crop observation
- [ ] Daily deterministic agricultural simulation
- [ ] Harvest decisions by condition and plant position
- [ ] Curing, sorting, grading, and crop disposition
- [ ] Sell, retain, or trade cured estate lots

## Milestone 5 - full process pipeline

- [ ] Fermentation with readable risk and intervention states
- [ ] Leaf resting and aging
- [ ] Cigar batch aging and readiness decisions
- [ ] Concurrent pipeline planning and multi-day advancement
- [ ] Named growers, finite lots, and relationship-based sourcing

## Milestone 6 - the growing house

- [ ] Named artisan recruitment and development
- [ ] Teaching, specifications, and quality tolerances
- [ ] Atelier and house production paths
- [ ] Facility improvement and environmental control
- [ ] Portfolio, contracts, repeat orders, and multiple reputation audiences

## Definition of vertical slice

The player can inhabit the finca, inspect estate or purchased leaf, form an intent, create and revise a prototype, commit a tiny batch, advance it to release, and receive a meaningful response. The slice preserves provenance and calendar state across save and reload.

Agricultural and aging durations may be accelerated or supplied through authored lots for the slice. It is more important to prove the complete craft-and-feedback loop than to simulate every upstream step first.
