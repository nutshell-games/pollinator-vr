using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Swarm
{
    [CustomEditor(typeof(SwarmController), true)]
    public class SwarmControllerEditor : Editor
    {

        void OnSceneGUI()
        {
            var sc = target as SwarmController;
            foreach (var t in sc.avoid)
            {
                Handles.Label(t.position, "Avoid");
                t.position = Handles.PositionHandle(t.position, t.rotation);
                sc.avoidDistance = Mathf.Max(0, Handles.ScaleSlider(sc.avoidDistance, t.position, Vector3.left, t.rotation, 1, 0));
            }
            foreach (var t in sc.attract)
            {
                Handles.Label(t.position, "Attract");
                t.position = Handles.PositionHandle(t.position, t.rotation);
                sc.maxAttractionRadius = Mathf.Max(sc.minAttractionRadius, Handles.ScaleSlider(sc.maxAttractionRadius, t.position, Vector3.left, t.rotation, 1, 0));
                sc.minAttractionRadius = Mathf.Max(0, Handles.ScaleSlider(sc.minAttractionRadius, t.position, Vector3.right, t.rotation, 1f, 0));
            }

            Handles.Label(sc.focus.position + Vector3.right * sc.swarmRadius, "Swarm Radius");
            sc.swarmRadius = Mathf.Max(0, Handles.ScaleSlider(sc.swarmRadius, sc.focus.position, Vector3.right, Quaternion.identity, 1f, 0));

            Handles.Label(sc.floorTransform.position, "Floor Transform");
            sc.floorTransform.position = Handles.PositionHandle(sc.floorTransform.position, Quaternion.identity);

            var relativeFloorPos = sc.focus.position;
            relativeFloorPos.y = sc.floorTransform.position.y;

            var ceilingPos = relativeFloorPos + (Vector3.up * sc.relativeCeilingHeight);
            Handles.Label(ceilingPos, "Ceiling Height");
            ceilingPos = Handles.PositionHandle(ceilingPos, Quaternion.identity);
            sc.relativeCeilingHeight = Mathf.Max(sc.relativeFloorHeight, ceilingPos.y - relativeFloorPos.y);

            var floorPos = relativeFloorPos + (Vector3.up * sc.relativeFloorHeight);
            Handles.Label(floorPos, "Floor Height");
            floorPos = Handles.PositionHandle(floorPos, Quaternion.identity);
            sc.relativeFloorHeight = Mathf.Min(sc.relativeCeilingHeight, floorPos.y - relativeFloorPos.y);

            Handles.Label(sc.focus.position, "Focus");
            sc.focus.position = Handles.PositionHandle(sc.focus.position, Quaternion.identity);


        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}