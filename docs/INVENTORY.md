# Inventory

**Status:** Directional implementation notes
**Last reviewed:** 2026-08-02

## Data model

- `ItemDefinition` is immutable authored data stored as a ScriptableObject.
- `ItemInstance` is runtime data with a stable identifier.
- `LotInstance` represents safely aggregated provenance-bearing material.
- `BatchInstance` represents cigars produced together against a specification.

Runtime instances reference definitions; they never mutate authored definitions.

## Initial categories

- Seeds and seed lots
- Harvested leaf
- Cured and graded leaf lots
- Fermenting, resting, and aged leaf lots
- Tools and workshop materials
- Packaging materials
- Prototypes
- Production batches
- Finished and released cigars

## Aggregation rule

Stacks or lots are allowed only when every gameplay-relevant property is compatible. Provenance-bearing objects may be aggregated for performance, but aggregation must preserve source, crop, process history, grade, condition, ownership, and allocation.

Splitting a lot creates child instances that retain the parent's history. A merge must be refused when it would erase a distinction the player could use to make a decision.

## Reservations and allocation

Inventory must distinguish material that is:

- Available
- Reserved for a prototype
- Committed to an approved cigar
- Bound to a contract
- In active processing
- Offered for sale or trade
- Held as an estate or experimental selection

The same units cannot be promised to several future uses. Allocation is a central design decision, not bookkeeping hidden from the player.

## Presentation constraint

Inventory views should emphasize provenance, condition, intended use, and observations. Do not sort the entire tobacco system around one generic quality number.
