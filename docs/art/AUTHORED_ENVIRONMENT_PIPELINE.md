# Authored environment pipeline

**Status:** Automated and screenshot validated; hands-on re-review pending
**Started:** 2026-08-03

The first Milestone 3A environment pass failed hands-on review. Its larger layout and surface textures did not prevent runtime primitive architecture and props from reading as graybox art. It also shipped an unacceptable lighting and renderer configuration before the urgent performance correction.

The replacement pass treats visible environment art as imported, authored content.

## Asset boundaries

- Blender-authored FBX assets provide the finca buildings, terrain, backdrop, tobacco plants, curing racks, pilones, workshop equipment, aging storage, courtyard furniture, and other cigar-specific forms.
- A small Poly Haven subset provides CC0 vegetation, generic furnishings, 1K PBR material maps, and a rural lighting environment.
- Runtime code may assemble imported modules, attach gameplay components, create invisible collision scaffolding, and place temporary interaction markers. It may not use Unity primitives as the dominant visible art layer.
- Unity Asset Store source packages remain excluded from the public repository unless their redistribution terms are independently confirmed.

## Source and license manifest

All selected Poly Haven assets are licensed CC0. Poly Haven permits commercial use, modification, and redistribution. Source assets are downloaded at 1K resolution and processed into game-ready derivatives.

| Asset ID | Type | Intended use |
| --- | --- | --- |
| `rural_landscape` | HDRI | restrained rural sky and ambient reference |
| `red_laterite_soil_stones` | PBR texture | estate soil and paths |
| `grass_path_2` | PBR texture | mixed ground cover |
| `white_rough_plaster` | PBR texture | lime-plaster architecture |
| `clay_roof_tiles_02` | PBR texture | weathered tile roofs |
| `wood_planks_dirt` | PBR texture | timber, shutters, decks, and work surfaces |
| `island_tree_02` | model | processed tropical shade-tree LODs |
| `jacaranda_tree` | model | dense shade canopy with a 122,612-polygon close mesh and 180-polygon distant proxy |
| `grass_bermuda_01` | model | processed ground-cover clusters |
| `calathea_orbifolia_01` | model | processed broadleaf planting |
| `wooden_crate_01` | model | generic storage prop |
| `painted_wooden_bench` | model | courtyard and veranda seating |
| `wooden_ladder` | model | curing and maintenance prop |

Source: <https://polyhaven.com/>
License: <https://polyhaven.com/license>
Acquired: 2026-08-03

No Unity Asset Store source files are included. The public repository contains only project-authored assets and scripts plus the documented CC0 Poly Haven subset.

## Import requirements

- Unity-facing textures are limited to 1K for this prototype pass.
- Imported foliage must have at least two mesh LODs or an equivalent distance-culling strategy.
- Repeated assets must reuse shared meshes and instanced materials.
- Buildings use imported shell meshes plus simple invisible colliders.
- Decorative point lights do not cast real-time shadows.
- Directional shadows remain capped at 55 meters and two cascades.
- The Play Mode performance-budget test remains a release gate.

## Reproducibility

Scripts under `Tools/ArtPipeline` inspect and build the Blender-authored FBX set. The CC0 processor accepts optional asset IDs so one model can be rebuilt without reprocessing the entire library. Temporary original downloads remain outside the repository; the repository contains processed CC0 derivatives, project-authored sources or build scripts, Unity-ready output, and this provenance record.

## Current validation evidence

- 23 Edit Mode tests pass.
- The Play Mode finca smoke test passes with 404 renderers, 145 LOD groups, eight visible built-in primitive meshes, and zero shadowed point lights.
- Imported architecture, terrain, production props, vegetation, normal maps, alpha-clipped foliage, and the rural HDR sky were rendered in isolated arrival, courtyard, overview, workshop exterior/interior, HUD, and close-canopy captures.
- The first capture exposed an FBX root-axis defect; imported Blender root transforms are now preserved beneath placement roots.
- The second capture exposed sparse uniform tree decimation; the dense Jacaranda now combines a reduced close mesh with a purpose-built 180-polygon distant canopy proxy.

These checks establish an implementation candidate, not hands-on acceptance. Milestone 3B remains blocked until the player-facing review confirms comfort, legibility, atmosphere, and movement feel.
