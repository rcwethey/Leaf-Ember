# Art and asset sourcing

**Status:** Active implementation policy
**Last reviewed:** 2026-08-02

Leaf & Ember should look cohesive, specific, and materially believable. Downloadable packs can accelerate broad environmental coverage, but they must not dictate the finca's identity or introduce licensing problems into the repository.

## Art direction boundary

Use external libraries primarily for:

- terrain foundations and stamps
- ground materials
- generic grasses, wildflowers, stones, and distant vegetation
- utility shaders that are compatible with the project's Unity and URP versions

Prefer handmade or commissioned assets for:

- tobacco plants at meaningful growth and harvest stages
- curing structures and hanging leaf
- pilones, leaf bales, sorting stations, and provenance storage
- chavetas, rolling boards, molds, presses, cigar gauges, and finished cigars
- aging shelves, production furniture, house packaging, and other signature props
- architecture whose construction and cultural context make it specific to the fictional northern Nicaraguan setting

The target is grounded stylized realism. Avoid mixing unrelated packs simply because they are available.

## Current handmade set

The Milestone 2 finca uses code-generated meshes and assembled primitives in:

- HandmadeFincaAssets.Geometry.cs
- HandmadeFincaAssets.Props.cs

They provide tobacco plants, curing racks, a pilón, bales, rolling tools, aging storage, tasting furniture, shade trees, a cistern, gabled roofs, distant ridges, and atmosphere. They are repository-owned prototype assets and can be freely replaced with authored prefabs later.

## Evaluated Unity Asset Store candidates

No Asset Store source files are currently vendored.

### Terrain Sample Asset Pack

- Publisher: Unity Technologies
- Listing: <https://assetstore.unity.com/packages/3d/environments/landscapes/terrain-sample-asset-pack-145808>
- Unity overview: <https://unity.com/blog/games/the-latest-unity-terrain-sample-pack-is-here>
- Potential use: terrain stamps, selected vegetation techniques, and a small approved set of PBR landscape materials
- Decision: do not import the entire package into the prototype. The listing is approximately 1.6 GB, which is disproportionate to the current slice. Evaluate it in a disposable project and migrate only genuinely required, license-compliant content.

### Grass Flowers Pack Free

- Publisher: ALP
- Listing: <https://assetstore.unity.com/packages/2d/textures-materials/nature/grass-flowers-pack-free-138810>
- Potential use: restrained courtyard and field-edge ground variation
- Decision: candidate for the first small local import trial after the cigar loop is playtested. Confirm URP appearance and performance in a disposable scene before integrating it.

### Other free environment candidates

Unity's current free-asset index is available at <https://assetstore.unity.com/top-assets/top-free>. Packs such as low-poly nature collections or starter terrain textures may be useful for experiments, but each must pass the same visual, technical, and licensing review. “Free” is a price, not permission to republish source assets.

## Import and licensing procedure

For each third-party package:

1. Record publisher, package name, exact version, source URL, acquisition date, license, and intended files in a manifest.
2. Acquire it through the developer's own Unity account and Package Manager or another publisher-authorized channel.
3. Test it in a disposable Unity project using the same Unity and URP versions.
4. Inspect dependencies, shaders, scripts, texture sizes, mesh scale, collision, LODs, and platform cost.
5. Import only the approved subset into a clearly named Assets/ThirdParty/Publisher/Package boundary when the license and repository visibility allow it.
6. Add a local README with the manifest and any required notices.
7. Build a game-player-facing prefab or material wrapper outside the third-party directory. Do not modify vendor source unnecessarily.
8. Verify the asset in the actual finca lighting and remove it if it breaks cohesion.

Unity Asset Store packages commonly use the Standard Unity Asset Store EULA. They can generally be embedded in a game, but raw source assets must not be redistributed as a standalone collection. A public source repository can itself constitute redistribution. Before committing any such package, confirm the current license and use private, access-controlled storage where required.

Official terms: <https://unity.com/legal/as-terms>

## Repository rules

- Do not commit a downloaded .unitypackage.
- Do not commit an entire large pack for one material or plant.
- Do not assume a migrated or downloaded file is distributable because Unity can import it.
- Keep source provenance and license notes beside every third-party boundary.
- Prefer CC0, bespoke, or repository-owned assets when public source distribution is important.
- Preserve the canonical finca layout and tobacco workflow; external art serves those decisions.
