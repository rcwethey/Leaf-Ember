# Milestone 1 implementation: embodied finca foundation

**Status:** Implemented and automated-test validated
**Implemented:** 2026-08-02
**Unity:** 6000.5.4f1

This document records what the first playable Leaf & Ember implementation proves. Canonical design intent remains in [`../design/README.md`](../design/README.md); this file describes the current executable prototype and its deliberate limits.

## Play it

Open the repository root in Unity and load `Assets/LeafEmber/Scenes/Bootstrap.unity`. Enter Play Mode. The persistent bootstrap constructs the prototype finca at runtime.

Controls:

- WASD: move
- Mouse: look
- E: interact with the centered station
- Enter: confirm focused work and spend its displayed calendar cost
- Left/Right or A/D: browse leaf lots while the cabinet is open
- Escape: close a focused view without spending time
- F5: save player, calendar, estate, and leaf-lot state
- F9: reload that state

## What exists

The graybox physically arranges the founding production route:

1. field-edge pilot plot
2. curing barn
3. fermentation room
4. leaf storage and provenance cabinet
5. personal workshop
6. aging room
7. finca office

The field, curing rack, and aging ledger demonstrate free observation. The pilón, rolling bench, and planning desk demonstrate focused transitions that show their cost and require confirmation before time advances.

The two initial leaf lots are intentionally authored and provenance-aware. Each records identity, origin, grower, tobacco type, harvest reference, process history, intended role, house observations, and current quantity. The presentation explicitly avoids a universal quality number or omniscient flavor truth.

## Calendar contract

The deterministic calendar implements the approved prototype structure:

- morning, afternoon, and evening blocks
- eight playable days per month
- twelve months and 96 playable days per year
- block advancement only through a meaningful reason
- day, month, and year rollover
- scheduled checkpoints reached within an advancement window
- a post-work summary identifying elapsed time and reached checkpoints
- climate-period language rather than temperate seasons

The first scheduled checkpoints demonstrate a curing-barn inspection and Elena Ortega's promised finca visit. They are proof events, not final narrative content.

## Persistence contract

F5 writes the existing schema-versioned JSON save envelope with typed sections for:

- player position and orientation
- calendar date and block
- estate name, facilities, condition, and operational state
- complete leaf-lot inventory and provenance

F9 restores every available section. Graybox geometry itself is deterministic and reconstructed at startup; mutable estate meaning lives in the estate service rather than scene objects.

## Architecture map

- `GameBootstrap` owns service composition.
- `CalendarService`, `InventoryService`, and `EstateService` own deterministic runtime state.
- `FincaPrototypeLauncher` and `FincaWorldBuilder` construct the disposable graybox.
- `PrototypePlayerController` and `PlayerInteractor` provide embodied navigation and interaction.
- interaction components publish typed work, inspection, or lot requests.
- `PrototypeHud` presents prompts, confirms time costs, displays summaries, and coordinates prototype save/load.
- `SaveSectionStore` maps typed domain snapshots into `SaveGameData` sections.

No static definition asset contains mutable session state. The runtime builder does not become the long-term estate simulation.

## Automated validation

The implementation passes:

- 16 Edit Mode tests covering services, calendar boundaries, checkpoints, defensive snapshots, provenance inventory, estate state, typed save sections, events, and JSON persistence
- one Play Mode smoke test confirming the founder, camera controller, finca, HUD, focused stations, inspection stations, lot cabinet, and registered domain services initialize together

Use these commands without an additional `-quit` argument; Unity's test runner exits on completion:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.5.4f1\Editor\Unity.exe' -batchmode -nographics -projectPath '.' -runTests -testPlatform EditMode -testResults 'Logs\EditModeResults.xml' -logFile 'Logs\UnityValidation.log'
```

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.5.4f1\Editor\Unity.exe' -batchmode -nographics -projectPath '.' -runTests -testPlatform PlayMode -testResults 'Logs\PlayModeResults.xml' -logFile 'Logs\UnityPlayModeValidation.log'
```

## Deliberately temporary

- geometry, materials, signage, and layout are graybox quality
- the immediate-mode HUD is a usability probe, not production UI
- movement numbers and interaction range have not received hands-on tuning
- workstations currently prove transitions and calendar costs; they do not yet simulate tobacco transformation
- scheduled checkpoint summaries do not yet offer branching responses
- no accessibility remapping or full controller interface exists yet
- authored lot observations are content examples, not calibrated flavor-model output

## Hands-on evaluation questions

Before polishing Milestone 1, play should answer:

- Is first-person movement appropriate for both estate navigation and intimate craft?
- Is the route legible without feeling like a factory line?
- Does free inspection feel clearly different from committing a work block?
- Is confirmation reassuring or obstructive during repeated work?
- Does provenance feel useful at the cabinet, or merely verbose?
- Are calendar checkpoint summaries prominent enough without feeling punitive?

These answers may tune controls, spatial layout, and interface presentation. They should not change the canonical rule that ordinary movement and inspection are free while meaningful committed work advances time.

## Next implementation slice

Milestone 2 should turn the workshop from a time-cost proof into the first cigar-development loop: define intent, select from authored lots, save a versioned recipe, construct a study cigar, receive construction evidence and contextual tasting observations, diagnose likely causes, and compare revisions without a universal quality score.
