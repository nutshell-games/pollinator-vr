using Cogobyte.ProceduralLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralIndicators
{
    //Renders a SelectionIndicator as a full path mesh
    [CreateAssetMenu(fileName = "NormalSelectionIndicatorPath", menuName = "Cogobyte/ProceduralIndicators/Selection/NormalSelectionIndicatorPath", order = 1)]
    public class NormalSelectionIndicatorPath : SelectionIndicatorPath
    {
        public AnimationCurve outerWidthFunction = AnimationCurve.Linear(0, 0.85f, 1, 0.85f);
        public float outerWidthFunctionLength = 1f;
        public AnimationCurve innerWidthFunction = AnimationCurve.Linear(0, 0.85f, 1, 0.85f);
        public float innerWidthFunctionLength = 1f;
        public AnimationCurve heightFunction = AnimationCurve.Linear(0, 0.85f, 1, 0.85f);
        public float heightFunctionLength = 1f;
        public Gradient innerColor;
        public Gradient outerColor;
        public bool is3D = true;
        public AnimationCurve lowerBoundFunction = AnimationCurve.Linear(0, 0.85f, 1, 0.85f);
        public float lowerBoundFunctionLength = 1f;


        public override void generate()
        {
            if (innerColor == null) prepareGradient(ref innerColor);
            if (outerColor == null) prepareGradient(ref outerColor);
            List<Vector3> path = pathArray.path;
            prepareOffset();
            mesh = new Mesh();
            List<Vector3> lowerVertices = new List<Vector3>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Color32> colors = new List<Color32>();
            Quaternion rot = Quaternion.Lerp(Quaternion.LookRotation(path[0] - path[path.Count - 2], Vector3.up), Quaternion.LookRotation(path[1] - path[0], Vector3.up), 0.5f);
            float currentPathLength = 0;
            vertices.Add(path[0] + outerWidthFunction.Evaluate(0) * (rot * Vector3.right) + heightFunction.Evaluate(0) * (rot * Vector3.up));
            lowerVertices.Add(path[0] + outerWidthFunction.Evaluate(0) * (rot * Vector3.right) - lowerBoundFunction.Evaluate(0) * (rot * Vector3.up));
            colors.Add(outerColor.Evaluate(0));
            vertices.Add(path[0] + innerWidthFunction.Evaluate(0) * (rot * Vector3.left) + heightFunction.Evaluate(0) * (rot * Vector3.up));
            lowerVertices.Add(path[0] + innerWidthFunction.Evaluate(0) * (rot * Vector3.left) - lowerBoundFunction.Evaluate(0) * (rot * Vector3.up));
            colors.Add(innerColor.Evaluate(0));
            for (int i = 1; i < path.Count - 1; i++)
            {
                currentPathLength += (path[i] - path[i - 1]).magnitude;
                rot = Quaternion.Lerp(Quaternion.LookRotation(path[i] - path[i - 1], Vector3.up), Quaternion.LookRotation(path[i + 1] - path[i], Vector3.up), 0.5f);
                vertices.Add(path[i] + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                lowerVertices.Add(path[i] + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                colors.Add(outerColor.Evaluate(currentPathLength));
                vertices.Add(path[i] + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                lowerVertices.Add(path[i] + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                colors.Add(innerColor.Evaluate(currentPathLength));
            }
            vertices.Add(vertices[0]);
            lowerVertices.Add(lowerVertices[0]);
            colors.Add(outerColor.Evaluate(1));
            vertices.Add(vertices[1]);
            lowerVertices.Add(lowerVertices[1]);
            colors.Add(innerColor.Evaluate(1));
            if (pathArray.obstacleCheck == PathArray.ObstacleCheckMode.Parellel)
            {
                pathArray.ParralelObstacleCheck(vertices);
            }
            else if (pathArray.obstacleCheck == PathArray.ObstacleCheckMode.Projection)
            {
                pathArray.ProjectionObstacleCheck(vertices);
            }
            for (int i = 0; i < path.Count - 1; i++)
            {
                triangles.Add(i * 2);
                triangles.Add(i * 2 + 1);
                triangles.Add(i * 2 + 2);

                triangles.Add(i * 2 + 1);
                triangles.Add(i * 2 + 3);
                triangles.Add(i * 2 + 2);
            }
            if (is3D)
            {
                for (int i = 0; i < lowerVertices.Count; i++)
                {
                    vertices.Add(lowerVertices[i]);
                    colors.Add(colors[i]);
                }

                int startVertIndex = path.Count * 2;
                for (int i = 0; i < path.Count - 1; i++)
                {
                    //lower tris
                    triangles.Add(startVertIndex + i * 2);
                    triangles.Add(startVertIndex + i * 2 + 2);
                    triangles.Add(startVertIndex + i * 2 + 1);

                    triangles.Add(startVertIndex + i * 2 + 1);
                    triangles.Add(startVertIndex + i * 2 + 2);
                    triangles.Add(startVertIndex + i * 2 + 3);

                    //innersidetris

                    triangles.Add(i * 2);
                    triangles.Add(startVertIndex + i * 2 + 2);
                    triangles.Add(startVertIndex + i * 2);

                    triangles.Add(i * 2);
                    triangles.Add(i * 2 + 2);
                    triangles.Add(startVertIndex + i * 2 + 2);
                    //RightSideTris
                    triangles.Add(i * 2 + 1);
                    triangles.Add(startVertIndex + i * 2 + 1);
                    triangles.Add(startVertIndex + i * 2 + 3);

                    triangles.Add(i * 2 + 1);
                    triangles.Add(startVertIndex + i * 2 + 3);
                    triangles.Add(i * 2 + 3);
                }
            }
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.uv = uvs;
            mesh.SetColors(colors);
            mesh.RecalculateNormals();
            mesh.name = "MadeOfShapesSelectionPath";
        }
    }
}