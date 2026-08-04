import math
import os
import random
import sys

import bpy
from mathutils import Vector


MATERIAL_NAMES = (
    "LE_Plaster",
    "LE_PlasterAccent",
    "LE_Timber",
    "LE_RoofTile",
    "LE_Stone",
    "LE_Glass",
    "LE_DarkInterior",
    "LE_LivingLeaf",
    "LE_LivingLeafLight",
    "LE_CuredLeaf",
    "LE_Metal",
    "LE_Cloth",
    "LE_Ground",
    "LE_Earth",
    "LE_Backdrop",
)


def script_args():
    if "--" not in sys.argv:
        return []
    return sys.argv[sys.argv.index("--") + 1 :]


def ensure_materials():
    colors = {
        "LE_Plaster": (0.74, 0.66, 0.49, 1.0),
        "LE_PlasterAccent": (0.42, 0.22, 0.12, 1.0),
        "LE_Timber": (0.24, 0.10, 0.035, 1.0),
        "LE_RoofTile": (0.48, 0.12, 0.045, 1.0),
        "LE_Stone": (0.18, 0.16, 0.12, 1.0),
        "LE_Glass": (0.035, 0.10, 0.11, 1.0),
        "LE_DarkInterior": (0.045, 0.035, 0.025, 1.0),
        "LE_LivingLeaf": (0.10, 0.28, 0.045, 1.0),
        "LE_LivingLeafLight": (0.22, 0.44, 0.08, 1.0),
        "LE_CuredLeaf": (0.30, 0.11, 0.025, 1.0),
        "LE_Metal": (0.06, 0.065, 0.06, 1.0),
        "LE_Cloth": (0.46, 0.34, 0.18, 1.0),
        "LE_Ground": (0.27, 0.20, 0.09, 1.0),
        "LE_Earth": (0.34, 0.12, 0.045, 1.0),
        "LE_Backdrop": (0.08, 0.18, 0.10, 1.0),
    }
    for name in MATERIAL_NAMES:
        material = bpy.data.materials.get(name) or bpy.data.materials.new(name)
        material.diffuse_color = colors[name]


def clear_scene():
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete(use_global=False)
    for mesh in list(bpy.data.meshes):
        if mesh.users == 0:
            bpy.data.meshes.remove(mesh)


def assign_material(obj, material_name):
    obj.data.materials.clear()
    obj.data.materials.append(bpy.data.materials[material_name])


def smart_uv(obj):
    bpy.context.view_layer.objects.active = obj
    obj.select_set(True)
    bpy.ops.object.mode_set(mode="EDIT")
    bpy.ops.mesh.select_all(action="SELECT")
    bpy.ops.uv.smart_project(angle_limit=math.radians(66), island_margin=0.025)
    bpy.ops.object.mode_set(mode="OBJECT")
    obj.select_set(False)


def add_box(name, location, dimensions, material_name, bevel=0.035, segments=2):
    bpy.ops.mesh.primitive_cube_add(location=location)
    obj = bpy.context.object
    obj.name = name
    obj.dimensions = dimensions
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    if bevel > 0:
        modifier = obj.modifiers.new("Edge softness", "BEVEL")
        modifier.width = bevel
        modifier.segments = segments
        modifier.limit_method = "ANGLE"
        bpy.context.view_layer.objects.active = obj
        bpy.ops.object.modifier_apply(modifier=modifier.name)
    assign_material(obj, material_name)
    smart_uv(obj)
    return obj


def add_cylinder(
    name,
    location,
    radius,
    depth,
    material_name,
    vertices=12,
    rotation=(0.0, 0.0, 0.0),
    bevel=0.02,
):
    bpy.ops.mesh.primitive_cylinder_add(
        vertices=vertices,
        radius=radius,
        depth=depth,
        location=location,
        rotation=rotation,
    )
    obj = bpy.context.object
    obj.name = name
    if bevel > 0:
        modifier = obj.modifiers.new("Edge softness", "BEVEL")
        modifier.width = bevel
        modifier.segments = 1
        bpy.context.view_layer.objects.active = obj
        bpy.ops.object.modifier_apply(modifier=modifier.name)
    assign_material(obj, material_name)
    smart_uv(obj)
    return obj


def add_panel(name, vertices, triangles, material_name, uvs=None):
    mesh = bpy.data.meshes.new(f"{name}_Mesh")
    mesh.from_pydata(vertices, [], triangles)
    mesh.update()
    obj = bpy.data.objects.new(name, mesh)
    bpy.context.collection.objects.link(obj)
    assign_material(obj, material_name)
    if uvs:
        uv_layer = mesh.uv_layers.new(name="UVMap")
        for polygon in mesh.polygons:
            for loop_index in polygon.loop_indices:
                vertex_index = mesh.loops[loop_index].vertex_index
                uv_layer.data[loop_index].uv = uvs[vertex_index]
    else:
        smart_uv(obj)
    return obj


def join_meshes(name):
    meshes = [obj for obj in bpy.context.scene.objects if obj.type == "MESH"]
    bpy.ops.object.select_all(action="DESELECT")
    for obj in meshes:
        obj.select_set(True)
    bpy.context.view_layer.objects.active = meshes[0]
    bpy.ops.object.join()
    result = bpy.context.object
    result.name = name
    return result


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


def add_window_frame(x, y, z, yaw, material_name, lod):
    if lod > 0:
        return
    frame_width = 1.65
    frame_height = 1.35
    thickness = 0.11
    depth = 0.10
    parts = [
        ((x - frame_width / 2, y, z), (thickness, depth, frame_height)),
        ((x + frame_width / 2, y, z), (thickness, depth, frame_height)),
        ((x, y, z - frame_height / 2), (frame_width, depth, thickness)),
        ((x, y, z + frame_height / 2), (frame_width, depth, thickness)),
    ]
    for index, (location, dimensions) in enumerate(parts):
        part = add_box(
            f"WindowFrame_{index}",
            location,
            dimensions,
            material_name,
            bevel=0.018,
            segments=1,
        )
        part.rotation_euler[2] = yaw
    glass = add_box(
        "RecessedGlass",
        (x, y, z),
        (frame_width - 0.14, 0.045, frame_height - 0.14),
        "LE_Glass",
        bevel=0,
    )
    glass.rotation_euler[2] = yaw


def add_side_window(depth_side, x_side, width, depth, wall_height, lod):
    horizontal = depth if x_side else width
    window_width = min(2.0, horizontal * 0.22)
    window_height = 1.45
    sill = 1.05
    center_z = sill + window_height / 2
    thickness = 0.26
    if x_side:
        x = width / 2 * depth_side
        lower = add_box(
            "SideWallLower",
            (x, 0, sill / 2),
            (thickness, depth, sill),
            "LE_Plaster",
        )
        upper_height = wall_height - sill - window_height
        add_box(
            "SideWallUpper",
            (x, 0, sill + window_height + upper_height / 2),
            (thickness, depth, upper_height),
            "LE_Plaster",
        )
        pier_depth = (depth - window_width) / 2
        for side in (-1, 1):
            add_box(
                "SideWallPier",
                (x, side * (window_width / 2 + pier_depth / 2), center_z),
                (thickness, pier_depth, window_height),
                "LE_Plaster",
            )
        if lod == 0:
            for side in (-1, 1):
                add_box(
                    "SideShutter",
                    (
                        x + depth_side * 0.19,
                        side * (window_width / 2 + 0.34),
                        center_z,
                    ),
                    (0.08, 0.60, window_height),
                    "LE_Timber",
                    bevel=0.025,
                )
            add_box(
                "SideGlass",
                (x + depth_side * 0.15, 0, center_z),
                (0.05, window_width - 0.1, window_height - 0.1),
                "LE_Glass",
                bevel=0,
            )


def add_roof(width, depth, wall_height, rise, lod):
    angle = math.atan2(rise, width / 2)
    slope_length = math.sqrt((width / 2) ** 2 + rise**2) + 0.7
    roof_depth = depth + 1.5
    for side in (-1, 1):
        panel = add_box(
            "RoofPlane",
            (side * width / 4, 0, wall_height + rise / 2),
            (slope_length, roof_depth, 0.18),
            "LE_RoofTile",
            bevel=0.035,
            segments=1,
        )
        panel.rotation_euler[1] = side * angle
    add_cylinder(
        "RoofRidge",
        (0, 0, wall_height + rise + 0.08),
        0.17,
        roof_depth,
        "LE_RoofTile",
        vertices=12 if lod == 0 else 8,
        rotation=(math.pi / 2, 0, 0),
        bevel=0.015,
    )
    if lod == 0:
        for y in (-depth / 2 - 0.6, depth / 2 + 0.6):
            for side in (-1, 1):
                fascia = add_box(
                    "RoofFascia",
                    (side * width / 4, y, wall_height + rise / 2 - 0.08),
                    (slope_length, 0.11, 0.22),
                    "LE_Timber",
                    bevel=0.018,
                    segments=1,
                )
                fascia.rotation_euler[1] = side * angle


def create_building(asset_name, width, depth, timber_barn, accent, lod):
    clear_scene()
    wall_height = 3.7
    door_width = 2.6
    door_height = 2.65
    wall_material = "LE_Timber" if timber_barn else "LE_Plaster"
    bevel_segments = 2 if lod == 0 else 1
    add_box(
        "StoneFoundation",
        (0, 0, 0.18),
        (width + 0.45, depth + 0.45, 0.36),
        "LE_Stone",
        bevel=0.08 if lod == 0 else 0.035,
        segments=bevel_segments,
    )
    add_box(
        "InteriorFloor",
        (0, 0, 0.40),
        (width - 0.35, depth - 0.35, 0.16),
        "LE_Timber",
        bevel=0.015,
        segments=1,
    )
    front_y = -depth / 2
    side_width = (width - door_width) / 2
    for side in (-1, 1):
        add_box(
            "FrontWall",
            (side * (door_width / 2 + side_width / 2), front_y, wall_height / 2),
            (side_width, 0.28, wall_height),
            wall_material,
            bevel=0.025,
            segments=bevel_segments,
        )
    add_box(
        "DoorHeader",
        (0, front_y, door_height + (wall_height - door_height) / 2),
        (door_width, 0.28, wall_height - door_height),
        wall_material,
        bevel=0.025,
        segments=bevel_segments,
    )
    add_box(
        "BackWall",
        (0, depth / 2, wall_height / 2),
        (width, 0.28, wall_height),
        wall_material,
        bevel=0.025,
        segments=bevel_segments,
    )
    add_side_window(-1, True, width, depth, wall_height, lod)
    add_side_window(1, True, width, depth, wall_height, lod)
    if accent and not timber_barn:
        for y in (-depth / 2 - 0.15, depth / 2 + 0.15):
            add_box(
                "PlasterAccentBand",
                (0, y, 0.83),
                (width + 0.12, 0.06, 0.62),
                "LE_PlasterAccent",
                bevel=0.01,
                segments=1,
            )
    for x in (-width / 2, width / 2):
        for y in (-depth / 2, depth / 2):
            add_box(
                "StructuralPost",
                (x, y, wall_height / 2),
                (0.23, 0.23, wall_height + 0.22),
                "LE_Timber",
                bevel=0.025,
                segments=bevel_segments,
            )
    for side in (-1, 1):
        add_box(
            "DoorJamb",
            (side * door_width / 2, front_y - 0.16, door_height / 2),
            (0.18, 0.18, door_height),
            "LE_Timber",
            bevel=0.02,
            segments=1,
        )
    add_box(
        "DoorLintel",
        (0, front_y - 0.16, door_height),
        (door_width + 0.18, 0.18, 0.18),
        "LE_Timber",
        bevel=0.02,
        segments=1,
    )
    for side in (-1, 1):
        door = add_box(
            "OpenDoor",
            (side * (door_width / 2 + 0.34), front_y - 0.20, door_height / 2),
            (door_width / 2 - 0.10, 0.12, door_height - 0.10),
            "LE_Timber",
            bevel=0.035,
            segments=bevel_segments,
        )
        door.rotation_euler[2] = side * math.radians(24)
    add_box(
        "DarkInterior",
        (0, front_y + 0.10, door_height / 2),
        (door_width - 0.16, 0.06, door_height - 0.12),
        "LE_DarkInterior",
        bevel=0,
    )
    veranda_depth = 2.55
    add_box(
        "VerandaDeck",
        (0, front_y - veranda_depth / 2, 0.48),
        (width * 0.72, veranda_depth, 0.18),
        "LE_Timber",
        bevel=0.025,
        segments=1,
    )
    for x in (-width * 0.32, width * 0.32):
        add_box(
            "VerandaPost",
            (x, front_y - veranda_depth + 0.12, 1.75),
            (0.20, 0.20, 2.7),
            "LE_Timber",
            bevel=0.025,
            segments=1,
        )
    add_box(
        "VerandaBeam",
        (0, front_y - veranda_depth + 0.12, 3.05),
        (width * 0.72, 0.20, 0.24),
        "LE_Timber",
        bevel=0.025,
        segments=1,
    )
    if lod == 0:
        for x in (-width * 0.18, 0, width * 0.18):
            add_box(
                "VerandaRafter",
                (x, front_y - veranda_depth / 2, 3.18),
                (0.10, veranda_depth + 0.3, 0.13),
                "LE_Timber",
                bevel=0.012,
                segments=1,
            )
    add_roof(width, depth, wall_height, 1.65, lod)
    shell = join_meshes(f"{asset_name}_LOD{lod}")
    return shell


def build_architecture(output_root):
    definitions = (
        ("CuringBarn", 19.0, 16.0, True, False),
        ("FermentationHouse", 16.0, 15.0, False, True),
        ("LeafStorage", 17.0, 15.0, False, False),
        ("PersonalWorkshop", 19.0, 15.0, False, True),
        ("AgingRoom", 18.0, 14.0, False, False),
        ("FincaOffice", 17.0, 13.0, False, True),
        ("FounderHomestead", 19.0, 14.0, False, True),
    )
    for asset_name, width, depth, timber_barn, accent in definitions:
        for lod in (0, 1):
            shell = create_building(asset_name, width, depth, timber_barn, accent, lod)
            export_selected(
                os.path.join(
                    output_root,
                    "Architecture",
                    f"{asset_name}_LOD{lod}.fbx",
                ),
                shell,
            )


def terrain_height(x, y):
    edge = max(abs(x) / 72.0, abs(y) / 56.0)
    edge_factor = max(0.0, min(1.0, (edge - 0.42) / 0.58))
    undulation = (
        math.sin(x * 0.095)
        + math.sin(y * 0.082 + 1.7)
        + math.sin((x + y) * 0.047)
    ) * 0.13
    return undulation * edge_factor + edge_factor * edge_factor * 0.55 - 0.10


def create_terrain(output_root):
    clear_scene()
    x_segments = 36
    y_segments = 28
    width = 144.0
    depth = 112.0
    vertices = []
    uvs = []
    triangles = []
    for y_index in range(y_segments + 1):
        y = (y_index / y_segments - 0.5) * depth
        for x_index in range(x_segments + 1):
            x = (x_index / x_segments - 0.5) * width
            vertices.append((x, y, terrain_height(x, y)))
            uvs.append((x / 12.0, y / 12.0))
    row = x_segments + 1
    for y_index in range(y_segments):
        for x_index in range(x_segments):
            a = y_index * row + x_index
            b = a + 1
            c = a + row
            d = c + 1
            triangles.extend(((a, c, d), (a, d, b)))
    terrain = add_panel("FincaTerrain", vertices, triangles, "LE_Ground", uvs)
    export_selected(
        os.path.join(output_root, "Landscape", "FincaTerrain.fbx"),
        terrain,
    )


def create_backdrop(output_root):
    clear_scene()
    random.seed(41)
    vertices = []
    triangles = []
    uvs = []
    ridges = (
        (-95.0, 20.0, 128.0, 18.0),
        (96.0, 18.0, 118.0, 16.0),
        (0.0, 104.0, 150.0, 24.0),
    )
    for ridge_index, (center_x, center_y, length, height) in enumerate(ridges):
        start = len(vertices)
        samples = 25
        horizontal = ridge_index == 2
        for index in range(samples):
            t = index / (samples - 1)
            offset = (t - 0.5) * length
            peak = height * (
                0.42
                + 0.34 * math.sin(t * math.pi)
                + 0.10 * math.sin(t * math.pi * 5.0 + ridge_index)
            )
            if horizontal:
                x, y = offset, center_y
            else:
                x, y = center_x, offset
            vertices.append((x, y, -2.0))
            vertices.append((x, y, peak))
            uvs.extend(((t, 0.0), (t, 1.0)))
        for index in range(samples - 1):
            base = start + index * 2
            triangles.extend(((base, base + 1, base + 3), (base, base + 3, base + 2)))
    backdrop = add_panel("FincaBackdrop", vertices, triangles, "LE_Backdrop", uvs)
    export_selected(
        os.path.join(output_root, "Landscape", "FincaBackdrop.fbx"),
        backdrop,
    )


def create_field_plot(output_root):
    clear_scene()
    x_segments = 28
    y_segments = 31
    width = 28.0
    depth = 31.0
    vertices = []
    uvs = []
    triangles = []
    for y_index in range(y_segments + 1):
        y = (y_index / y_segments - 0.5) * depth
        for x_index in range(x_segments + 1):
            x = (x_index / x_segments - 0.5) * width
            row_rise = 0.06 * (0.5 + 0.5 * math.cos(x * math.pi / 2.0))
            edge = max(abs(x) / (width / 2), abs(y) / (depth / 2))
            berm = max(0.0, (edge - 0.88) / 0.12) * 0.12
            vertices.append((x, y, row_rise + berm))
            uvs.append((x / 5.0, y / 5.0))
    row = x_segments + 1
    for y_index in range(y_segments):
        for x_index in range(x_segments):
            a = y_index * row + x_index
            b = a + 1
            c = a + row
            d = c + 1
            triangles.extend(((a, c, d), (a, d, b)))
    field = add_panel("AuthoredFieldPlot", vertices, triangles, "LE_Earth", uvs)
    export_selected(
        os.path.join(output_root, "Landscape", "AuthoredFieldPlot.fbx"),
        field,
    )


def leaf_surface(name, angle, height, length, width, segments, material_name, droop):
    radial = Vector((math.cos(angle), math.sin(angle), 0.0))
    right = Vector((-radial.y, radial.x, 0.0))
    vertices = []
    uvs = []
    triangles = []
    for index in range(segments + 1):
        t = index / segments
        center = radial * (length * t)
        center.z = height + math.sin(t * math.pi) * length * 0.07 - droop * t * t
        half_width = width * math.sin(t * math.pi) * (0.78 + 0.22 * (1 - t))
        left = center - right * half_width
        right_vertex = center + right * half_width
        vertices.extend((tuple(left), tuple(right_vertex)))
        uvs.extend(((0.0, t), (1.0, t)))
    for index in range(segments):
        base = index * 2
        triangles.extend(
            (
                (base, base + 2, base + 3),
                (base, base + 3, base + 1),
                (base, base + 3, base + 2),
                (base, base + 1, base + 3),
            )
        )
    return add_panel(name, vertices, triangles, material_name, uvs)


def create_tobacco(output_root, lod):
    clear_scene()
    height = 1.65
    add_cylinder(
        "TobaccoStem",
        (0, 0, height * 0.46),
        0.045 if lod == 0 else 0.055,
        height * 0.92,
        "LE_LivingLeaf",
        vertices=10 if lod == 0 else 6,
        bevel=0.008,
    )
    levels = 5 if lod == 0 else 3
    leaf_segments = 7 if lod == 0 else 3
    for level in range(levels):
        z = height * (0.22 + level * (0.13 if lod == 0 else 0.20))
        length = height * (0.48 - level * 0.045)
        width = length * 0.30
        for side in range(2):
            angle = math.radians(level * 67 + side * 180)
            leaf_surface(
                f"TobaccoLeaf_{level}_{side}",
                angle,
                z,
                length,
                width,
                leaf_segments,
                "LE_LivingLeafLight" if (level + side) % 2 else "LE_LivingLeaf",
                droop=length * 0.18,
            )
    plant = join_meshes(f"TobaccoPlant_LOD{lod}")
    export_selected(
        os.path.join(output_root, "Vegetation", f"TobaccoPlant_LOD{lod}.fbx"),
        plant,
    )


def create_curing_rack(output_root, lod):
    clear_scene()
    width = 6.0
    depth = 3.2
    for x in (-width / 2, width / 2):
        for y in (-depth / 2, depth / 2):
            add_box(
                "RackPost",
                (x, y, 1.45),
                (0.16, 0.16, 2.9),
                "LE_Timber",
                bevel=0.025,
                segments=1,
            )
    rails = (-depth / 2, 0, depth / 2) if lod == 0 else (-depth / 2, depth / 2)
    for rail_index, y in enumerate(rails):
        add_box(
            "CuringRail",
            (0, y, 2.55),
            (width + 0.25, 0.13, 0.13),
            "LE_Timber",
            bevel=0.018,
            segments=1,
        )
        leaf_count = 9 if lod == 0 else 5
        for leaf_index in range(leaf_count):
            x = (leaf_index / (leaf_count - 1) - 0.5) * (width - 0.65)
            leaf = leaf_surface(
                "HangingCuredLeaf",
                -math.pi / 2,
                2.46,
                0.86,
                0.20,
                5 if lod == 0 else 2,
                "LE_CuredLeaf",
                droop=0.05,
            )
            leaf.location.x = x
            leaf.location.y = y
            leaf.rotation_euler[0] = math.pi / 2
    rack = join_meshes(f"CuringRack_LOD{lod}")
    export_selected(
        os.path.join(output_root, "Production", f"CuringRack_LOD{lod}.fbx"),
        rack,
    )


def create_pilon(output_root):
    clear_scene()
    add_box("Pallet", (0, 0, 0.10), (3.6, 2.6, 0.20), "LE_Timber", bevel=0.04)
    random.seed(17)
    for layer in range(6):
        z = 0.28 + layer * 0.34
        bundle = add_box(
            "FermentingLeafBundle",
            (random.uniform(-0.08, 0.08), random.uniform(-0.06, 0.06), z),
            (3.15 - layer * 0.06, 2.18 - layer * 0.04, 0.31),
            "LE_CuredLeaf",
            bevel=0.10,
            segments=2,
        )
        bundle.rotation_euler[2] = math.radians(random.uniform(-2.5, 2.5))
        for x in (-1.0, 1.0):
            add_box(
                "BundleStrap",
                (x, 0, z),
                (0.08, 2.24, 0.34),
                "LE_Cloth",
                bevel=0.015,
                segments=1,
            )
    pilon = join_meshes("PilonStack")
    export_selected(os.path.join(output_root, "Production", "PilonStack.fbx"), pilon)


def create_workbench(output_root):
    clear_scene()
    add_box("BenchTop", (0, 0, 0.94), (3.5, 1.55, 0.16), "LE_Timber", bevel=0.055)
    for x in (-1.48, 1.48):
        for y in (-0.58, 0.58):
            add_box("BenchLeg", (x, y, 0.46), (0.18, 0.18, 0.92), "LE_Timber", bevel=0.025)
    add_box("RollingBoard", (-0.45, 0, 1.055), (1.65, 1.08, 0.07), "LE_Timber", bevel=0.045)
    add_box("CigarMold", (0.98, 0.12, 1.07), (0.82, 0.82, 0.10), "LE_Timber", bevel=0.06)
    for index in range(4):
        add_cylinder(
            "StudyCigar",
            (0.70 + index * 0.18, -0.20, 1.16),
            0.035,
            0.58,
            "LE_CuredLeaf",
            vertices=10,
            rotation=(0, math.pi / 2, 0),
            bevel=0.008,
        )
    blade_vertices = [
        (-0.12, 0, 0),
        (0.35, 0, 0.02),
        (0.58, 0, 0.20),
        (0.18, 0, 0.28),
        (-0.24, 0, 0.18),
    ]
    blade = add_panel(
        "ChavetaBlade",
        [(x, y, z + 1.16) for x, y, z in blade_vertices],
        ((0, 1, 2), (0, 2, 3), (0, 3, 4)),
        "LE_Metal",
    )
    blade.location.x = -0.35
    blade.location.y = -0.47
    add_cylinder(
        "ChavetaHandle",
        (-0.52, -0.47, 1.23),
        0.07,
        0.32,
        "LE_Timber",
        vertices=10,
        rotation=(0, math.pi / 2, 0),
        bevel=0.015,
    )
    workbench = join_meshes("RollingWorkbench")
    export_selected(
        os.path.join(output_root, "Production", "RollingWorkbench.fbx"),
        workbench,
    )


def create_aging_shelf(output_root):
    clear_scene()
    width = 2.2
    height = 3.0
    for x in (-width / 2, width / 2):
        for y in (-0.34, 0.34):
            add_box("ShelfPost", (x, y, height / 2), (0.12, 0.12, height), "LE_Timber", bevel=0.02)
    for level in range(5):
        z = 0.32 + level * 0.62
        add_box("Shelf", (0, 0, z), (width + 0.16, 0.82, 0.10), "LE_Timber", bevel=0.018)
        for side in (-1, 1):
            add_box(
                "CedarCigarBox",
                (side * 0.52, 0, z + 0.18),
                (0.82, 0.58, 0.25),
                "LE_Timber",
                bevel=0.035,
            )
    shelf = join_meshes("AgingShelf")
    export_selected(os.path.join(output_root, "Production", "AgingShelf.fbx"), shelf)


def create_planning_desk(output_root):
    clear_scene()
    add_box("DeskTop", (0, 0, 0.92), (3.7, 1.45, 0.16), "LE_Timber", bevel=0.055)
    for x in (-1.52, 1.52):
        add_box("DeskPedestal", (x, 0, 0.46), (0.48, 1.18, 0.92), "LE_Timber", bevel=0.035)
        for level in range(3):
            add_box(
                "DeskDrawer",
                (x, -0.61, 0.26 + level * 0.28),
                (0.39, 0.05, 0.22),
                "LE_Timber",
                bevel=0.018,
            )
            add_cylinder(
                "DrawerPull",
                (x, -0.66, 0.26 + level * 0.28),
                0.025,
                0.08,
                "LE_Metal",
                vertices=8,
                rotation=(math.pi / 2, 0, 0),
                bevel=0.005,
            )
    add_box("OpenLedger", (-0.30, -0.10, 1.045), (1.15, 0.76, 0.035), "LE_Cloth", bevel=0.025)
    add_cylinder("InkBottle", (0.72, -0.22, 1.13), 0.075, 0.18, "LE_Glass", vertices=12, bevel=0.012)
    add_cylinder(
        "LedgerPencil",
        (0.16, -0.48, 1.11),
        0.018,
        0.72,
        "LE_Timber",
        vertices=8,
        rotation=(0, math.pi / 2, math.radians(8)),
        bevel=0.003,
    )
    desk = join_meshes("PlanningDesk")
    export_selected(os.path.join(output_root, "Production", "PlanningDesk.fbx"), desk)


def create_courtyard_set(output_root):
    clear_scene()
    width = 11.0
    depth = 7.0
    for x in (-width / 2, width / 2):
        for y in (-depth / 2, depth / 2):
            add_box("PergolaPost", (x, y, 1.65), (0.22, 0.22, 3.3), "LE_Timber", bevel=0.025)
    for y in (-depth / 2, depth / 2):
        add_box("PergolaBeam", (0, y, 3.25), (width + 0.4, 0.22, 0.24), "LE_Timber", bevel=0.025)
    for x in range(-5, 6):
        add_box("PergolaSlat", (x, 0, 3.46), (0.12, depth + 0.55, 0.12), "LE_Timber", bevel=0.012)
    add_cylinder("TastingTable", (0, 0, 0.78), 1.25, 0.14, "LE_Timber", vertices=24, bevel=0.045)
    add_cylinder("TablePedestal", (0, 0, 0.40), 0.18, 0.78, "LE_Timber", vertices=12, bevel=0.025)
    for angle in (0, math.pi / 2, math.pi, math.pi * 1.5):
        x = math.cos(angle) * 2.15
        y = math.sin(angle) * 2.15
        seat = add_box("TastingSeat", (x, y, 0.52), (1.2, 0.48, 0.14), "LE_Timber", bevel=0.045)
        seat.rotation_euler[2] = angle + math.pi / 2
        back = add_box("TastingSeatBack", (x, y, 0.95), (1.2, 0.14, 0.72), "LE_Timber", bevel=0.035)
        back.rotation_euler[2] = angle + math.pi / 2
    patio = join_meshes("TastingPergolaSet")
    export_selected(
        os.path.join(output_root, "Courtyard", "TastingPergolaSet.fbx"),
        patio,
    )


def create_cistern(output_root):
    clear_scene()
    add_cylinder("CisternBody", (0, 0, 0.90), 1.08, 1.80, "LE_PlasterAccent", vertices=24, bevel=0.06)
    add_cylinder("CisternRim", (0, 0, 1.78), 1.16, 0.16, "LE_Stone", vertices=24, bevel=0.035)
    add_cylinder("CisternWater", (0, 0, 1.86), 0.91, 0.05, "LE_Glass", vertices=24, bevel=0)
    cistern = join_meshes("CourtyardCistern")
    export_selected(os.path.join(output_root, "Courtyard", "CourtyardCistern.fbx"), cistern)


def create_boundary_assets(output_root):
    clear_scene()
    for index, x in enumerate((-5.0, -1.67, 1.67, 5.0)):
        post = add_cylinder(
            "FencePost",
            (x, 0, 0.86),
            0.095 + ((index % 2) * 0.012),
            1.72 + ((index % 3) * 0.08),
            "LE_Timber",
            vertices=8,
            bevel=0.012,
        )
        post.rotation_euler[1] = math.radians(-1.5 + index)
    for z in (0.65, 1.18):
        rail = add_box(
            "RoughFenceRail",
            (0, 0, z),
            (10.25, 0.13, 0.13),
            "LE_Timber",
            bevel=0.025,
            segments=1,
        )
        rail.rotation_euler[0] = math.radians(1.2 if z < 1 else -1.0)
    fence = join_meshes("BoundaryFenceSection")
    export_selected(
        os.path.join(output_root, "Landscape", "BoundaryFenceSection.fbx"),
        fence,
    )

    clear_scene()
    for x in (-2.45, 2.45):
        add_box("GatePier", (x, 0, 1.65), (0.34, 0.42, 3.3), "LE_Stone", bevel=0.055)
        add_box("GateTimberPost", (x, -0.02, 1.72), (0.20, 0.20, 3.35), "LE_Timber", bevel=0.025)
    add_box("GateHeader", (0, 0, 3.12), (5.35, 0.30, 0.32), "LE_Timber", bevel=0.045)
    add_box("GateSignBoard", (0, -0.08, 2.72), (2.75, 0.12, 0.72), "LE_Timber", bevel=0.065)
    for side in (-1, 1):
        x_center = side * 1.22
        add_box("GateLeafTop", (x_center, 0, 1.52), (2.25, 0.10, 0.11), "LE_Timber", bevel=0.018)
        add_box("GateLeafBottom", (x_center, 0, 0.42), (2.25, 0.10, 0.11), "LE_Timber", bevel=0.018)
        for x_offset in (-0.92, 0, 0.92):
            add_box(
                "GatePicket",
                (x_center + x_offset, 0, 0.97),
                (0.09, 0.09, 1.18),
                "LE_Timber",
                bevel=0.014,
            )
    gate = join_meshes("FincaEntryGate")
    export_selected(os.path.join(output_root, "Landscape", "FincaEntryGate.fbx"), gate)


def main():
    args = script_args()
    if len(args) != 1:
        raise SystemExit("Usage: blender --background --python build_authored_finca.py -- OUTPUT_ROOT")
    output_root = os.path.abspath(args[0])
    ensure_materials()
    build_architecture(output_root)
    create_terrain(output_root)
    create_backdrop(output_root)
    create_field_plot(output_root)
    for lod in (0, 1):
        create_tobacco(output_root, lod)
        create_curing_rack(output_root, lod)
    create_pilon(output_root)
    create_workbench(output_root)
    create_aging_shelf(output_root)
    create_planning_desk(output_root)
    create_courtyard_set(output_root)
    create_cistern(output_root)
    create_boundary_assets(output_root)
    print(f"LEAF_EMBER_AUTHORED_ASSETS_BUILT {output_root}")


if __name__ == "__main__":
    main()
