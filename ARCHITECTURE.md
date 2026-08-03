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

GameBootstrap creates the persistent composition root before the first scene loads. It registers event, save, calendar, inventory, and estate services in ServiceRegistry; consumers depend on interfaces and obtain references during initialization. GameEventBus carries cross-system notifications without coupling publishers to concrete consumers.

Static definitions must never contain mutable session state. Save data uses schema-versioned data-transfer objects so migrations can be introduced without coupling persistence to scene objects.

## Prototype composition

FincaPrototypeLauncher is the temporary Milestone 1 composition layer. It builds the graybox at runtime, creates the founder controller and camera, and injects service references into the HUD and interaction context. This keeps graybox geometry disposable while the deterministic domain systems remain reusable.

The calendar, inventory, and estate services are plain deterministic C# classes. MonoBehaviours translate physical interaction into typed requests; the prototype HUD confirms meaningful work before invoking calendar advancement. SaveSectionStore serializes each domain snapshot into the existing versioned save envelope.

The immediate-mode prototype HUD is intentionally temporary. It exists to evaluate information hierarchy, action costs, and focused transitions before committing to a production UI framework.

## Core systems

- Bootstrap
- Save
- Time
- Estate
- Weather
- Farm
- Tobacco
- Inventory
- Economy

Implemented systems remain behind interfaces with Edit Mode tests for deterministic logic. Play Mode smoke tests verify that the runtime composition produces an inhabitable scene without coupling domain tests to scene objects.
