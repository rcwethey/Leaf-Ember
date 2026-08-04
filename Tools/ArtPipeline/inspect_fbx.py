import json
import os
import sys

import bpy


def script_args():
    if "--" not in sys.argv:
        return []
    return sys.argv[sys.argv.index("--") + 1 :]


def clear_scene():
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete(use_global=False)
    for datablocks in (
        bpy.data.meshes,
        bpy.data.curves,
        bpy.data.materials,
        bpy.data.images,
    ):
        for datablock in list(datablocks):
            if datablock.users == 0:
                datablocks.remove(datablock)


def inspect(path):
    clear_scene()
    bpy.ops.import_scene.fbx(filepath=path)
    meshes = []
    for obj in bpy.context.scene.objects:
        if obj.type != "MESH":
            continue
        dimensions = tuple(round(value, 4) for value in obj.dimensions)
        meshes.append(
            {
                "name": obj.name,
                "vertices": len(obj.data.vertices),
                "polygons": len(obj.data.polygons),
                "dimensions": dimensions,
                "materials": [
                    slot.material.name if slot.material else None
                    for slot in obj.material_slots
                ],
            }
        )
    return {
        "path": os.path.abspath(path),
        "objects": len(bpy.context.scene.objects),
        "meshes": meshes,
        "totals": {
            "vertices": sum(mesh["vertices"] for mesh in meshes),
            "polygons": sum(mesh["polygons"] for mesh in meshes),
        },
    }


def main():
    paths = script_args()
    if not paths:
        raise SystemExit("Pass one or more FBX paths after --")
    print("LEAF_EMBER_FBX_REPORT")
    print(json.dumps([inspect(path) for path in paths], indent=2))


if __name__ == "__main__":
    main()
