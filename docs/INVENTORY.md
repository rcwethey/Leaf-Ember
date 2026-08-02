# Inventory

## Data model

- ItemDefinition is immutable authored data stored as a ScriptableObject.
- ItemInstance is runtime data with a stable instance identifier.

## Initial categories

- Seeds
- Leaves
- Tools
- Materials
- Finished cigars

Stacks are allowed only when every gameplay-relevant property is identical. Provenance-bearing leaves and cigars are individual instances unless a later design explicitly defines safe aggregation rules.
