# Milestone 2 implementation: cigar development

**Status:** Implemented and automated-test validated
**Implemented:** 2026-08-02
**Unity:** 6000.5.4f1

This document records the first playable cigar-development loop. Canonical intent remains in the [design index](../design/README.md); this file defines what the current executable prototype proves and what it deliberately compresses.

## Play the loop

Open Assets/LeafEmber/Scenes/Bootstrap.unity and enter Play Mode.

1. Walk to the workshop and interact with the orange work point.
2. Choose an intent, starting composition, conditioning, compression, and filler arrangement.
3. Review the specification and commit one work block to construct a study cigar.
4. Read the immediate construction evidence.
5. Walk to the courtyard tasting table and conduct a focused tasting after the required rest.
6. Compare the staged observations with the recorded intent.
7. Choose a causal hypothesis. The notebook creates a new recipe version without erasing the old one.
8. Return to the workshop, construct the revision, and taste it.
9. Compare the two tasted versions as a set of tradeoffs rather than a ranked winner.

Focused-view controls:

- Left/Right or A/D: change the selected option
- Up/Down or W/S: change the active construction field or prototype
- Enter: continue or commit the displayed action
- Backspace: return to the previous setup page where supported
- D after a tasting: diagnose another revision when comparison is also available
- Escape: close without committing the pending action

The existing movement, inspection, calendar, save, and load controls remain unchanged.

## Authored material

The founding inventory now contains four fictional working lots:

- Finca Pilot Seco: restrained estate-grown aromatic filler
- Ortega Valley Viso: stronger sourced filler with warm-spice and roasted potential
- San Jerónimo Binder: sourced structural leaf with combustion support
- Las Lomas Colorado Wrapper: sourced wrapper with elasticity, aroma, and finish

Each lot preserves provenance, observations, quantity, a compact hidden potential state, and process state. The names and descriptions are development content, not a claim of finished regional authenticity; they still require the cultural and agricultural review defined by the canonical setting documents.

Loading an older prototype save keeps saved lot values, fills missing process data for known lots, and appends newly required authored definitions. This is a narrow migration bridge until formal save migrations replace section-level compatibility logic.

## Model boundary

The simulation separates:

- strength
- body
- sensory intensity
- sweetness and dryness
- smoothness or irritation
- finish
- smoke delivery and temperature behavior
- aroma families
- construction evidence

Recipe components contribute differently by role. Shared aroma families can reinforce one another, closely weighted families can layer, and a strong leading component can mask a quieter one. Conditioning, compression, filler arrangement, combustion support, and format modify the result.

The deterministic hidden expression exists so the same inputs create consistent evidence. It is never shown as an omniscient “actual flavor” answer. The player sees construction measurements, staged perceptions with confidence, intent comparison, and another maker's perspective.

There is no universal cigar-quality score and no flavor-point average presented as the result.

## Construction contract

The first interaction compresses the full handcraft sequence into three consequential decisions:

- leaf conditioning: dry, balanced, or supple
- bunch compression: light, balanced, or firm
- filler arrangement: parallel folds, layered book fold, or open airflow channels

Construction produces separate hard evidence:

- dimensions and weight
- draw behavior
- firmness
- wrapper condition
- seam and cap integrity
- moisture distribution
- expected combustion behavior

This is a decision prototype, not the final tactile rolling interaction. It proves that construction choices have diagnosable consequences without using timed prompts, tracing accuracy, or a rolling score.

## Tasting and revision contract

A focused tasting consumes one study cigar and one work block. The journal records:

- pre-light
- opening
- middle
- final portion
- finish

Each stage keeps construction evidence distinct from sensory perception and labels confidence. The report compares the perceived result with the prior intent. Independent feedback is explicitly another perspective, never a correction or objective reveal.

The diagnosis roster currently tests five useful causes:

- component dominance
- tobacco condition
- bunch compression
- insufficient rest
- combustion and format

A diagnosis is a hypothesis. It produces a separate version with a recorded rationale and revised target; the earlier recipe, cigar, tasting, and diagnosis remain in history. Once two versions have been tasted, comparison describes construction, strength, body, character, and intent fidelity without choosing a winner.

## Calendar and material costs

- constructing a study cigar costs one work block
- each recipe currently requires one rest block before tasting
- tasting consumes the cigar and one work block
- recording a diagnosis is included in the tasting analysis and costs no additional block

The prototype validates finite cigar consumption but does not yet deduct bulk leaf from inventory. Milestone 3 owns recipe commitment, finite leaf allocation, tiny-batch production, aging, packaging, and first sale.

## Persistence and architecture

CigarDevelopmentService owns deterministic recipe, prototype, tasting, and diagnosis state behind ICigarDevelopmentService. The domain is plain C#; it does not depend on scene objects or the temporary UI.

CigarWorkbench and TastingTable publish typed requests. CigarDevelopmentView translates those requests into a focused keyboard interface and invokes the domain and calendar services. PrototypeHud stores a typed cigar-development section alongside player, calendar, estate, and inventory sections.

The saved snapshot includes:

- every recipe version and its intent, composition, construction targets, and rationale
- constructed study cigars, their evidence, hidden expression, readiness, and consumed state
- staged tasting journals
- diagnosis history

## Scene presentation pass

Milestone 2 also replaces many graybox stand-ins with lightweight handmade runtime meshes and assembled primitives:

- broad-leaf tobacco plants
- hanging curing racks
- layered pilón and strapped leaf bales
- rolling table, wrapper leaves, study cigar, and chaveta
- aging shelves and resting boxes
- tasting table and ash dish
- cistern, shade trees, gabled roofs, distant hills, fog, and warmer light

These assets establish scale and identity without pretending to be final production art. Tobacco-specific props remain custom because generic farm packs rarely represent cigar production accurately.

The external-asset policy and evaluated Unity Asset Store candidates are recorded in the [asset-sourcing guide](../art/ASSET_SOURCING.md).

## Automated validation

The implementation passes:

- 23 Edit Mode tests covering deterministic craft simulation, authored lots, old-save compatibility, recipe history, construction evidence, rest enforcement, five-stage perspective tasting, diagnosis, version comparison, service snapshots, calendar, estate, events, and persistence
- one Play Mode smoke test confirming the rebuilt finca, player, workbench, tasting table, focused view, four authored lots, and registered domain services initialize together

The suites were run in an isolated copy because the primary project was open in Unity during implementation. Use the standard commands in [Milestone 1](MILESTONE_1.md) for repeat validation against the repository root once no editor instance has that project open.

## Deliberately temporary

- immediate-mode UI and text density are prototype quality
- construction choices stand in for later equivalent mouse, keyboard, and controller gestures
- tasting observations are generated; selecting and pinning personally significant impressions comes later
- the visiting maker is a perspective placeholder, not a finished named character scene
- values are deterministic examples awaiting repeated playtest calibration
- runtime-generated geometry will eventually give way to authored prefabs, materials, lighting, and environmental art
- no external Asset Store source files are committed

## Hands-on evaluation

Playtesting should answer:

- Does recording intent before choosing a composition make the tasting result easier to reason about?
- Are construction consequences legible without feeling predetermined?
- Does the five-stage journal provide enough evidence without becoming a wall of text?
- Are the diagnosis choices understandable to a player who is still learning tobacco craft?
- Does revision feel like testing a hypothesis rather than selecting an upgrade?
- Does version comparison expose a meaningful preference or tradeoff without needing a score?
- Is walking between workshop and tasting table atmospheric, or merely friction?
- Do the handmade tobacco spaces now communicate their actual purpose at a glance?
