# Milestone 3A implementation: finca and player experience

**Status:** Automated-test validated; hands-on visual gate failed
**Started:** 2026-08-03
**Unity:** 6000.5.4f1

Milestone 3A is a quality gate between the cigar-development prototype and the first commercial release. It responds directly to hands-on feedback that the finca remained too small, primitive, visually grayboxed, and difficult to understand.

The first-release economy will not be layered on top of an unreadable prototype. The existing deterministic domain systems remain, while their spatial and player-facing presentation is rebuilt.

The first 2026-08-03 hands-on review rejected this pass. The expanded layout and guidance do not compensate for primitive-derived architecture and props that still read as graybox art. The same review exposed an unacceptable performance regression caused by an excessive renderer count multiplied through shadow-casting point lights. The performance fault has been corrected and regression-tested; the visual gate remains failed.

## Success criteria

The overhaul succeeds when a new player can:

- understand the purpose of Leaf & Ember before making a decision
- read the finca as a believable working estate with distinct landmarks
- identify the next meaningful objective and its physical destination
- look at an interaction and understand what it does, why it matters, and what it costs
- learn foundational tobacco terms without leaving the game
- complete the intent, construction, tasting, diagnosis, and revision loop without unexplained reports
- recognize the field, curing, fermentation, storage, workshop, aging, office, home, and tasting spaces by their contents rather than floating labels

## Spatial contract

The playable estate expands from a tightly packed 52 by 46 meter prototype pad to an approximately 144 by 112 meter finca.

Its structure is:

- south: entry gate, arrival road, finca office, and founder homestead
- center: compacted-earth courtyard, shaded tasting patio, cistern, and main path junction
- west: estate observation plot and curing barn
- north: fermentation house and provenance-aware leaf storage
- east: personal workshop and aging room
- perimeter: working fence, windbreak plants, shade trees, and distant ridges

Travel should make the estate feel inhabited without turning routine movement into wasted time. The central courtyard remains the orientation anchor.

## Visual contract

The new presentation uses:

- custom terrain, curved path, courtyard, leaf, roof, and hill meshes
- more detailed plaster-and-timber buildings with foundations, verandas, open doors, framed windows, deep roof overhangs, physical signs, and warm lamps
- direct interaction with recognizable work objects instead of glowing placeholder blocks
- denser tobacco rows, hanging cure racks, multiple pilones, leaf bales, provenance storage, rolling equipment, aging shelves, crates, sacks, barrels, benches, and landscape vegetation
- original generated and visually reviewed ground-cover, red-clay, lime-plaster, aged-hardwood, and clay-roof textures

The target remains grounded stylized realism. These assets establish a cohesive indie art foundation; they are not presented as final production architecture or culturally reviewed reference.

## Guidance contract

A retained-mode game interface replaces the persistent prototype status and prompt overlays.

It provides:

- a three-page opening orientation
- current calendar and climate context
- a dynamic craft objective derived from actual recipe, prototype, tasting, and diagnosis state
- a physical destination for the next objective
- contextual interaction cards with location, category, action, explanation, and explicit time or material cost
- an always-available craft glossary on the G key
- a restrained center marker and consistent visual hierarchy

The cigar-development view also explains:

- why intent is recorded before leaf selection
- the likely direction and risk of each starting composition
- how conditioning, compression, and filler arrangement can affect construction and combustion
- why generated observations are evidence rather than an objectively correct flavor report

An unresolved tasting can be reopened from the workshop notebook, preventing a player from losing the diagnosis path by closing the report.

## Deliberate boundaries

- Milestone 3A does not yet create a commercial batch or buyer interaction.
- The retained guidance interface coexists with temporary focused-work and cigar-development panels; those panels receive clarity improvements now and a complete production UI conversion later.
- Asset Store source files remain outside the public repository unless a license-safe source pipeline is established.
- Generated surface images are project-owned visual foundations; tobacco-specific three-dimensional assets remain code-authored prototypes.
- Cultural, architectural, and agricultural authenticity still require the research and review process defined by the canonical setting documents.

## Gate to Milestone 3B

Milestone 3B may begin once automated validation passes and hands-on play confirms:

- the finca no longer feels like one small graybox yard
- the first objective is obvious without external instruction
- interaction costs and consequences are understandable before confirmation
- the craft terminology is teachable in context
- the workshop-to-tasting route feels atmospheric rather than confusing

## Validation evidence

- Unity imports and compiles the rebuilt runtime presentation under Unity 6000.5.4f1 and URP.
- All 23 Edit Mode tests pass.
- The Play Mode startup smoke test passes and verifies composition, guidance, direct interaction stations, landmark separation, authored surface loading, and registered domain state.
- Render review covers the entry approach, central courtyard, estate overview, workshop side, and opening guidance.
- The procedural scene is capped below 1,000 renderers and permits no shadow-casting point lights. The current validated build creates 910 renderers and zero shadowed point lights.

Automated evidence prevents the layout, surface foundation, and guidance layer from silently disappearing. It cannot decide whether movement, atmosphere, readability, and pacing feel right in hand. That judgment remains the explicit hands-on gate before Milestone 3B.

## Performance guardrails

Repeated tobacco and landscape foliage must be combined into a small number of meshes rather than emitted as one renderer per leaf. Decorative lanterns may provide restrained local light but must not cast real-time shadows. The prototype caps directional-shadow distance at 55 meters with two cascades.

Any future environment pass must keep the Play Mode performance-budget assertions green. A visually richer finca is not acceptable if it makes the interactive build unsafe or uncomfortable to run.

The next visual attempt must use authored modular environment assets, production terrain and foliage, a coherent lighting setup, and measured LOD/culling. Runtime primitive assembly may remain only for invisible scaffolding and temporary interaction markers, not as the dominant visible art layer.
