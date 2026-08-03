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

## Completed - design gate 1: cigar craft

- [x] Define the flavor vocabulary and conceptual data model
- [x] Separate strength, body, aroma, taste, construction, expression, and perception
- [x] Define transformation through curing, fermentation, resting, and aging
- [x] Define blend interactions, recipes, predictions, and scaling
- [x] Define personal construction, artisan execution, and quality control
- [x] Define tasting, learning, uncertainty, feedback, diagnosis, and revision
- [x] Publish and cross-link the canonical cigar-system documents

Numerical calibration remains prototype work and must preserve the canonical separation of technical quality, intent, perception, consistency, and preference.

## Completed - design gate 2: market and economy

- [x] Define sales channels and named buyer roles
- [x] Define the release, allocation, pricing, order, and payment lifecycle
- [x] Define operating costs, cash flow, credit, aging inventory, and financial pressure
- [x] Define demand, scarcity, unsold stock, repeat orders, and market feedback
- [x] Connect crop sales and sourced-leaf purchasing to house production
- [x] Decide the abstraction level for export, logistics, taxation, and regulation
- [x] Publish a canonical market-and-economy document

## Completed - design gate 3: characters and relationships

- [x] Define relationship dimensions and memory
- [x] Define artisan recruitment, training, specialties, ambitions, conflict, and departure
- [x] Define grower, buyer, critic, competitor, and community relationship patterns
- [x] Define authored versus systemic character content
- [x] Support close friendship and family-like bonds while excluding romance mechanics
- [x] Publish a canonical character-and-relationship document

## Completed - design gate 4: facilities and estate progression

- [x] Define the finca's initial condition and restoration sequence
- [x] Define facility roles, layout effects, environmental control, and capacity
- [x] Define construction, repair, upkeep, utilities, and staffing requirements
- [x] Define expansion paths that do not require an enormous estate
- [x] Define how estate changes become physically and socially visible
- [x] Establish the game-first compression and vertical-slice boundaries
- [x] Publish a canonical facilities-and-progression document

## Completed - design gate 5: knowledge and long-term progression

- [x] Separate player knowledge, character skill, house capability, access, and reputation
- [x] Define how capabilities are learned, taught, discovered, purchased, or relationship-gated
- [x] Define progression without a linear tree of strictly superior upgrades
- [x] Define how crops, aging stock, recipes, artisans, and records accumulate into legacy
- [x] Keep the base game founder-centered without required succession or a lifespan timer
- [x] Publish a canonical knowledge-and-progression document

## Completed - design gate 6: setting and narrative specifics

- [x] Place the fictional valley and local town within real northern Nicaragua
- [x] Define an undated contemporary period and stable modern technology
- [x] Define founder backgrounds and the financed acquisition circumstances
- [x] Define a founding spine, character arcs, and emergent house history
- [x] Establish English-first text without character voice-over
- [x] Define cultural research and consultation requirements
- [x] Publish a canonical setting-and-narrative document

## Completed - design gate 7: calendar tuning and pacing

- [x] Define workday blocks, action costs, and the role of ordinary movement
- [x] Define season and year length
- [x] Define travel time and scheduling
- [x] Define safe multi-day advancement, interruptions, and summaries
- [x] Define crop, fermentation, aging, market, and relationship time compression
- [x] Define long-term historical presentation across many in-game years
- [x] Publish canonical calendar-and-pacing direction

All required pre-implementation design gates are complete. Interface, accessibility, and numerical calibration continue through prototype work rather than additional blocking gates.

## Completed - Milestone 1: embodied finca foundation

- [x] Player controller and camera
- [x] Graybox the finca's core production route
- [x] Interaction framework and focused workbench transitions
- [x] Calendar, time advancement, scheduled checkpoints, and summaries
- [x] Provenance-aware inventory and lot inspection
- [x] Save and reload the player, calendar, estate, and lot state

The runtime-generated graybox and its implementation contract are documented in [`docs/implementation/MILESTONE_1.md`](docs/implementation/MILESTONE_1.md). Hands-on evaluation may change control feel, layout, interface treatment, and tuning without reopening the approved design direction.

## Completed - Milestone 2: cigar-development prototype

- [x] Provide several authored leaf lots with distinct provenance
- [x] Implement the compact first flavor and process data model
- [x] Blend-intent and versioned recipe notebook
- [x] Prototype preparation and rolling interaction
- [x] Construction evaluation and hard quality-control evidence
- [x] Focused tasting, intent comparison, diagnosis, and revision
- [x] Compare two prototype versions without a universal quality score

The playable contract, deliberate abstractions, validation evidence, and playtest questions are documented in [docs/implementation/MILESTONE_2.md](docs/implementation/MILESTONE_2.md).

## Active - Milestone 3: first release

- [ ] Commit a recipe and finite leaf allocation
- [ ] Produce and age a tiny batch
- [ ] Package and present the release
- [ ] Sell or submit it to an initial named buyer using the approved economy model
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
