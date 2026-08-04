import math
import os
import random
import sys

import bpy
from mathutils import Vector


def script_args():
    if "--" not in sys.argv:
        return []
    return sys.argv[sys.argv.index("--") + 1 :]


def clear_scene():
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete(use_global=False)
    for mesh in list(bpy.data.meshes):
        if mesh.users == 0:
            bpy.data.meshes.remove(mesh)


def import_fbx(path):
    clear_scene()
    bpy.ops.import_scene.fbx(filepath=path)
    return [obj for obj in bpy.context.scene.objects if obj.type == "MESH"]


def join_meshes(objects, name):
    bpy.ops.object.select_all(action="DESELECT")
    for obj in objects:
        obj.select_set(True)
    bpy.context.view_layer.objects.active = objects[0]
    bpy.ops.object.join()
    result = bpy.context.object
    result.name = name
    return result


def apply_decimate(obj, ratio):
    modifier = obj.modifiers.new("Game-ready reduction", "DECIMATE")
    modifier.ratio = ratio
    modifier.use_collapse_triangulate = True
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.modifier_apply(modifier=modifier.name)


def export_selected(path, obj):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    bpy.ops.object.select_all(action="DESELECT")
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    bpy.ops.export_scene.fbx(
        filepath=path,
        use_selection=True,
        object_types={"MESH"},
        apply_scale_options="FBX_SCALE_UNITS",
        axis_forward="-Z",
        axis_up="Y",
        bake_anim=False,
        add_leaf_bones=False,
        path_mode="AUTO",
    )


def process_tree(source_root, output_root):
    source = os.path.join(source_root, "island_tree_02", "island_tree_02.fbx")
    destination = os.path.join(output_root, "island_tree_02")
    for lod, ratio in ((0, 0.035), (1, 0.010)):
        objects = import_fbx(source)
        tree = join_meshes(objects, f"island_tree_02_LOD{lod}")
        apply_decimate(tree, ratio)
        export_selected(os.path.join(destination, f"island_tree_02_LOD{lod}.fbx"), tree)


def process_jacaranda(source_root, output_root):
    source = os.path.join(source_root, "jacaranda_tree", "jacaranda_tree.fbx")
    destination = os.path.join(output_root, "jacaranda_tree")
    objects = import_fbx(source)
    tree = join_meshes(objects, "jacaranda_tree_LOD0")
    apply_decimate(tree, 0.018)
    leaf_material = bpy.data.materials.get("jacaranda_tree_leaves")
    canopy = create_jacaranda_leaf_cards(leaf_material, 130, 731)
    tree = join_meshes([tree, canopy], "jacaranda_tree_LOD0")
    export_selected(os.path.join(destination, "jacaranda_tree_LOD0.fbx"), tree)
    create_jacaranda_proxy(destination)


def create_jacaranda_leaf_cards(material, cluster_count, seed):
    generator = random.Random(seed)
    vertices = []
    faces = []
    uv_by_vertex = []
    clusters = 0
    while clusters < cluster_count:
        x = generator.uniform(-10.4, 10.4)
        y = generator.uniform(-7.8, 7.8)
        z = generator.uniform(8.4, 17.0)
        ellipsoid = (
            (x / 10.8) ** 2
            + (y / 8.2) ** 2
            + ((z - 12.7) / 4.8) ** 2
        )
        if ellipsoid > 1.0:
            continue

        width = generator.uniform(1.25, 2.05)
        height = generator.uniform(1.10, 1.85)
        yaw = generator.uniform(0.0, math.pi)
        center = Vector((x, y, z))
        up = Vector((0.0, 0.0, height * 0.5))
        for offset in (0.0, math.pi * 0.5):
            angle = yaw + offset
            right = Vector((math.cos(angle), math.sin(angle), 0.0)) * (width * 0.5)
            start = len(vertices)
            vertices.extend(
                (
                    tuple(center - right - up),
                    tuple(center + right - up),
                    tuple(center + right + up),
                    tuple(center - right + up),
                )
            )
            uv_by_vertex.extend(((0.0, 0.0), (1.0, 0.0), (1.0, 1.0), (0.0, 1.0)))
            faces.append((start, start + 1, start + 2, start + 3))
        clusters += 1

    mesh = bpy.data.meshes.new("JacarandaLeafCards_Mesh")
    mesh.from_pydata(vertices, [], faces)
    mesh.update()
    cards = bpy.data.objects.new("JacarandaLeafCards", mesh)
    bpy.context.collection.objects.link(cards)
    mesh.materials.append(material)
    uv_layer = mesh.uv_layers.new(name="UVMap")
    for polygon in mesh.polygons:
        for loop_index in polygon.loop_indices:
            vertex_index = mesh.loops[loop_index].vertex_index
            uv_layer.data[loop_index].uv = uv_by_vertex[vertex_index]
    return cards


def create_jacaranda_proxy(destination):
    clear_scene()
    trunk_material = bpy.data.materials.new("jacaranda_tree_trunk")
    leaves_material = bpy.data.materials.new("jacaranda_tree_leaves")

    bpy.ops.mesh.primitive_cylinder_add(
        vertices=10,
        radius=0.72,
        depth=8.8,
        location=(0.0, 0.0, 4.4),
    )
    trunk = bpy.context.object
    trunk.name = "JacarandaProxyTrunk"
    trunk.data.materials.append(trunk_material)

    create_jacaranda_leaf_cards(leaves_material, 84, 733)

    proxy = join_meshes(
        [obj for obj in bpy.context.scene.objects if obj.type == "MESH"],
        "jacaranda_tree_LOD1",
    )
    bpy.context.scene.cursor.location = (0.0, 0.0, 0.0)
    bpy.context.view_layer.objects.active = proxy
    bpy.ops.object.origin_set(type="ORIGIN_CURSOR")
    export_selected(os.path.join(destination, "jacaranda_tree_LOD1.fbx"), proxy)


def enhance_existing_jacaranda(path):
    objects = import_fbx(path)
    tree = join_meshes(objects, "jacaranda_tree_LOD0")
    leaf_material = bpy.data.materials.get("jacaranda_tree_leaves")
    canopy = create_jacaranda_leaf_cards(leaf_material, 130, 731)
    tree = join_meshes([tree, canopy], "jacaranda_tree_LOD0")
    export_selected(path, tree)


def process_calathea(source_root, output_root):
    source = os.path.join(
        source_root,
        "calathea_orbifolia_01",
        "calathea_orbifolia_01.fbx",
    )
    destination = os.path.join(output_root, "calathea_orbifolia_01")
    for lod, ratio in ((0, 0.80), (1, 0.32)):
        objects = import_fbx(source)
        selected = sorted(objects, key=lambda obj: obj.name)[:3]
        placements = ((0.0, 0.0), (0.38, 0.12), (-0.30, 0.18))
        for obj, (x, y) in zip(selected, placements):
            obj.location.x = x
            obj.location.y = y
        cluster = join_meshes(selected, f"calathea_orbifolia_01_LOD{lod}")
        apply_decimate(cluster, ratio)
        export_selected(
            os.path.join(destination, f"calathea_orbifolia_01_LOD{lod}.fbx"),
            cluster,
        )


def process_grass(source_root, output_root):
    source = os.path.join(source_root, "grass_bermuda_01", "grass_bermuda_01.fbx")
    destination = os.path.join(output_root, "grass_bermuda_01")
    wanted_by_lod = {
        0: (
            "grass_bermuda_01_seedling_a",
            "grass_bermuda_01_seedling_b",
            "grass_bermuda_01_medium_a",
            "grass_bermuda_01_medium_d",
            "grass_bermuda_01_dead_a",
            "grass_bermuda_01_flattened_a",
        ),
        1: (
            "grass_bermuda_01_seedling_a",
            "grass_bermuda_01_medium_a",
            "grass_bermuda_01_dead_a",
        ),
    }
    placements = (
        (-0.42, -0.25),
        (0.28, -0.18),
        (-0.12, 0.34),
        (0.43, 0.30),
        (-0.48, 0.24),
        (0.12, 0.02),
    )
    for lod in (0, 1):
        objects = import_fbx(source)
        by_name = {obj.name: obj for obj in objects}
        selected = []
        for name, (x, y) in zip(wanted_by_lod[lod], placements):
            obj = by_name[name]
            obj.location.x = x
            obj.location.y = y
            obj.scale *= 2.4
            selected.append(obj)
        cluster = join_meshes(selected, f"grass_bermuda_01_LOD{lod}")
        export_selected(
            os.path.join(destination, f"grass_bermuda_01_LOD{lod}.fbx"),
            cluster,
        )


def process_prop(source_root, output_root, asset_id, ratio=1.0):
    source = os.path.join(source_root, asset_id, f"{asset_id}.fbx")
    objects = import_fbx(source)
    prop = join_meshes(objects, asset_id)
    if ratio < 1.0:
        apply_decimate(prop, ratio)
    export_selected(os.path.join(output_root, asset_id, f"{asset_id}.fbx"), prop)


def main():
    args = script_args()
    if len(args) < 2:
        raise SystemExit(
            "Usage: blender --background --python process_cc0_models.py -- "
            "SOURCE_ROOT OUTPUT_ROOT [ASSET_ID ...]"
        )
    source_root = os.path.abspath(args[0])
    output_root = os.path.abspath(args[1])
    requested = set(args[2:])
    process_all = not requested
    if process_all or "island_tree_02" in requested:
        process_tree(source_root, output_root)
    if process_all or "jacaranda_tree" in requested:
        process_jacaranda(source_root, output_root)
    if process_all or "calathea_orbifolia_01" in requested:
        process_calathea(source_root, output_root)
    if process_all or "grass_bermuda_01" in requested:
        process_grass(source_root, output_root)
    if process_all or "wooden_crate_01" in requested:
        process_prop(source_root, output_root, "wooden_crate_01", 0.75)
    if process_all or "painted_wooden_bench" in requested:
        process_prop(source_root, output_root, "painted_wooden_bench")
    if process_all or "wooden_ladder" in requested:
        process_prop(source_root, output_root, "wooden_ladder", 0.65)
    print(f"LEAF_EMBER_CC0_MODELS_PROCESSED {output_root}")


if __name__ == "__main__":
    main()
