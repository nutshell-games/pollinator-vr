using Cogobyte.ProceduralLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralIndicators
{
    //Renders a SelectionIndicator as a broken path mesh
    [CreateAssetMenu(fileName = "BrokenSelectionIndicatorPath", menuName = "Cogobyte/ProceduralIndicators/Selection/BrokenSelectionIndicatorPath", order = 3)]
    public class BrokenSelectionIndicatorPath : NormalSelectionIndicatorPath
    {
        [Range(0.00001f, 1000)]
        public int numberOfBrokenLines = 1;
        [Range(0f, 1f)]
        public float percentageOfEmptySpace = 0.1f;

        public override void generate()
        {
            if (innerColor == null) prepareGradient(ref innerColor);
            if (outerColor == null) prepareGradient(ref outerColor);
            List<Vector3> path = pathArray.path;
            prepareOffset();
            float brokenLineLength = pathArray.maxPathLength / numberOfBrokenLines;
            float brakeDistance = percentageOfEmptySpace * brokenLineLength;
            brokenLineLength -= brakeDistance;
            //iterator for path
            int currentPathIndex = 1;
            //distance traveled ?since last brokenPath point
            float currentDistance = 0;
            //is it currently in brokenLine or brake
            bool drawStatus = false;
            //it will always start with line
            currentDistance = brakeDistance;
            float magnitudeOfPathPart = (path[1] - path[0]).magnitude;
            mesh = new Mesh();
            List<Vector3> lowerVertices = new List<Vector3>();
            List<Vector3> vertices = new List<Vector3>();

            List<int> triangles = new List<int>();
            List<Color32> colors = new List<Color32>();
            Quaternion rot = Quaternion.Lerp(Quaternion.LookRotation(path[0] - path[path.Count - 2], Vector3.up), Quaternion.LookRotation(path[1] - path[0], Vector3.up), 0.5f);
            float currentPathLength = 0;

            List<int> brokenOpenClosePath = new List<int>();
            Vector3 currentShapeLocation = path[0];

            while (currentPathIndex < path.Count)
            {
                //while we have points but still no ending for brokenline or brake
                while (currentDistance >= magnitudeOfPathPart)
                {
                    //add extrude point only if we have brokenline
                    if (drawStatus)
                    {
                        vertices.Add(path[currentPathIndex] + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                        lowerVertices.Add(path[currentPathIndex] + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                        colors.Add(outerColor.Evaluate(currentPathLength));
                        vertices.Add(path[currentPathIndex] + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                        lowerVertices.Add(path[currentPathIndex] + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                        colors.Add(innerColor.Evaluate(currentPathLength));
                        if (brokenOpenClosePath.Count == 0 || brokenOpenClosePath[brokenOpenClosePath.Count - 1] == 2)
                        {
                            brokenOpenClosePath.Add(0);
                        }
                        else
                        {
                            brokenOpenClosePath.Add(1);
                        }
                    }
                    currentPathLength += magnitudeOfPathPart;
                    currentDistance -= magnitudeOfPathPart;
                    currentShapeLocation = path[currentPathIndex];
                    currentPathIndex++;
                    if (currentPathIndex >= path.Count) break;
                    magnitudeOfPathPart = (path[currentPathIndex] - path[currentPathIndex - 1]).magnitude;
                    if (currentPathIndex < path.Count - 1)
                        rot = Quaternion.Lerp(Quaternion.LookRotation(path[currentPathIndex] - path[currentPathIndex - 1], Vector3.up), Quaternion.LookRotation(path[currentPathIndex + 1] - path[currentPathIndex], Vector3.up), 0.5f);
                }
                if (currentPathIndex >= path.Count) break;
                //switch line and brake in distance between two path points
                while (currentDistance < magnitudeOfPathPart)
                {
                    rot = Quaternion.LookRotation((path[currentPathIndex] - path[currentPathIndex - 1]).normalized, Vector3.up);
                    currentPathLength += currentDistance;
                    Vector3 bPathItem = currentShapeLocation + (path[currentPathIndex] - path[currentPathIndex - 1]).normalized * currentDistance;
                    vertices.Add(bPathItem + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                    lowerVertices.Add(bPathItem + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength) * (rot * Vector3.right) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                    colors.Add(outerColor.Evaluate(currentPathLength));
                    vertices.Add(bPathItem + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                    lowerVertices.Add(bPathItem + innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength) * (rot * Vector3.left) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                    colors.Add(innerColor.Evaluate(currentPathLength));

                    if (drawStatus)
                    {
                        brokenOpenClosePath.Add(2);
                        drawStatus = !drawStatus;
                        currentDistance += brakeDistance;
                    }
                    else
                    {
                        brokenOpenClosePath.Add(0);
                        drawStatus = !drawStatus;
                        currentDistance += brokenLineLength;
                    }
                }
            }
            if (pathArray.obstacleCheck == PathArray.ObstacleCheckMode.Parellel)
            {
                pathArray.ParralelObstacleCheck(vertices);
            }
            else if (pathArray.obstacleCheck == PathArray.ObstacleCheckMode.Projection)
            {
                pathArray.ProjectionObstacleCheck(vertices);
            }
            for (int i = 0; i < brokenOpenClosePath.Count - 1; i++)
            {
                if (brokenOpenClosePath[i] != 2)
                {
                    triangles.Add(i * 2);
                    triangles.Add(i * 2 + 1);
                    triangles.Add(i * 2 + 2);

                    triangles.Add(i * 2 + 1);
                    triangles.Add(i * 2 + 3);
                    triangles.Add(i * 2 + 2);
                }
            }
            if (is3D)
            {
                for (int i = 0; i < lowerVertices.Count; i++)
                {
                    vertices.Add(lowerVertices[i]);
                    colors.Add(colors[i]);
                }

                int startVertIndex = brokenOpenClosePath.Count * 2;
                for (int i = 0; i < brokenOpenClosePath.Count - 1; i++)
                {
                    if (brokenOpenClosePath[i] != 2)
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
                        if (brokenOpenClosePath[i] == 0)
                        {
                            triangles.Add(startVertIndex + i * 2);
                            triangles.Add(startVertIndex + i * 2 + 1);
                            triangles.Add(i * 2 + 1);
                            //RightSideTris
                            triangles.Add(startVertIndex + i * 2);
                            triangles.Add(i * 2 + 1);
                            triangles.Add(i * 2);
                        }
                    }
                    else
                    {
                        triangles.Add(startVertIndex + i * 2);
                        triangles.Add(i * 2 + 1);
                        triangles.Add(startVertIndex + i * 2 + 1);
                        //RightSideTris
                        triangles.Add(startVertIndex + i * 2);
                        triangles.Add(i * 2);
                        triangles.Add(i * 2 + 1);
                    }

                }
            }
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.uv = uvs;
            mesh.SetColors(colors);
            mesh.RecalculateNormals();
            mesh.name = "BrokenSelectionPath";
        }
    }
}