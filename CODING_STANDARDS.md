# Coding standards

- Give each class one clear responsibility.
- Define interfaces at system boundaries.
- Prefer composition over inheritance.
- Do not introduce global mutable state or God objects.
- Do not hardcode gameplay tuning values; use ScriptableObject definitions.
- Keep runtime state separate from definitions.
- Keep deterministic domain logic independent of MonoBehaviour when practical.
- Communicate between major systems through typed events or explicit interfaces.
- Make new systems testable in Edit Mode.
- Treat compiler warnings and failing tests as defects.
- Use the LeafEmber root namespace and block-scoped namespace declarations.
