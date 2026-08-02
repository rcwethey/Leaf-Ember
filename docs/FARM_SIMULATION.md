# Farm simulation

## Entities

- Farm
- Field
- Plant
- Leaf

## Daily tick

The deterministic daily simulation order is:

1. Weather
2. Soil
3. Growth
4. Disease
5. Harvest evaluation

Each stage consumes the previous stage's results and publishes state-change events after its transaction completes.

## Acceptance

The player can prepare a field, plant, sleep, grow, harvest, save, and reload without losing provenance or simulation state.
