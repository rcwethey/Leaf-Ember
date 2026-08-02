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

GameBootstrap creates the persistent composition root before the first scene loads. It registers services in ServiceRegistry; consumers depend on interfaces and obtain references during initialization. GameEventBus carries cross-system notifications without coupling publishers to concrete consumers.

Static definitions must never contain mutable session state. Save data uses schema-versioned data-transfer objects so migrations can be introduced without coupling persistence to scene objects.

## Core systems

- Bootstrap
- Save
- Time
- Weather
- Farm
- Tobacco
- Inventory
- Economy

The Sprint 0 implementation includes Bootstrap, events, and the Save skeleton. Later systems should be introduced behind interfaces with Edit Mode tests for deterministic logic.
