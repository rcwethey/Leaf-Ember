# Architecture

## Principles

- SOLID design and one responsibility per class
- Composition over inheritance
- Event-driven collaboration between systems
- ScriptableObjects for static definitions and tuning data
- Plain serializable objects for runtime and save state
- Explicit interfaces at system boundaries
- No scene-wide service searches or God objects

## Runtime composition

GameBootstrap creates the persistent composition root before the first scene loads. It registers event, save, calendar, inventory, estate, and cigar-development services in ServiceRegistry; consumers depend on interfaces and obtain references during initialization. GameEventBus carries cross-system notifications without coupling publishers to concrete consumers.

Static definitions must never contain mutable session state. Save data uses schema-versioned data-transfer objects so migrations can be introduced without coupling persistence to scene objects.

## Prototype composition

FincaPrototypeLauncher is the temporary Milestone 1-3 composition layer. It builds the finca prototype at runtime, creates the founder controller and camera, and injects service references into the guidance HUD, focused views, and interaction context. This keeps prototype geometry disposable while deterministic domain systems remain reusable.

The calendar, inventory, estate, and cigar-development services are plain deterministic C# classes. MonoBehaviours translate physical interaction into typed requests. The prototype HUD and focused cigar view confirm meaningful work before invoking calendar advancement. SaveSectionStore serializes each domain snapshot into the existing versioned save envelope.

CigarDevelopmentService preserves versioned intent, recipes, study cigars, construction evidence, hidden expression, perspective tasting records, and diagnosis history. CigarWorkbench and TastingTable only publish requests; they do not own craft state. Hidden expression supports deterministic consistency but is never presented as an objective sensory answer.

FincaExperienceHud is a retained-mode guidance layer. It owns opening orientation, calendar context, state-driven objectives, contextual interaction explanations, explicit costs, and the craft glossary. PrototypeHud now only owns temporary focused-work confirmations, summaries, and toasts. CigarDevelopmentView remains a focused prototype panel until the later production UI pass.

## Core systems

- Bootstrap
- Save
- Time
- Estate
- Weather
- Farm
- Tobacco
- Cigar development
- Inventory
- Economy

Implemented systems remain behind interfaces with Edit Mode tests for deterministic logic. Play Mode smoke tests verify that the runtime composition produces an inhabitable scene without coupling domain tests to scene objects.
