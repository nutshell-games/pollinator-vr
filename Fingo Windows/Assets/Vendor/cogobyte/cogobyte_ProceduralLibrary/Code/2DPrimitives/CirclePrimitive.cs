using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //Circle shape
    [CreateAssetMenu(fileName = "CirclePrimitive", menuName = "Cogobyte/ProceduralLibrary/Primitives/CirclePrimitive", order = 2)]
    [System.Serializable]
    public class CirclePrimitive : Primitive
    {
        [Range(0, 20000)]
        public float halfDiameter = 1;
        //Level of detail
        [Range(3, 250)]
        public int numberOfSections = 3;
        [Range(1, 250)]
        public int numberOfSectionsByRange = 1;
        //Coloring
        public Gradient[] gradient;

        public override int getVertexCount()
        {
            return numberOfSections * numberOfSectionsByRange + 1;
        }

        public override int getTrisCount()
        {
            return numberOfSections * 3 + (numberOfSections * 2 * (numberOfSectionsByRange - 1)) * 3;
        }

        public override void generate()
        {
            base.generate();
            prepareColorGradient(ref gradient);
            int uvIndex = 0;
            int vertexIndex = 0;
            float angle = -Mathf.PI * 1 / 2;
            float maxAngle = 2 * Mathf.PI;
            verts[0] = transformOffset.MultiplyPoint(new Vector3(0, 0, 0));
            colors[0] = gradient[0].Evaluate(0);
            vertexIndex++;
            uvs[uvIndex] = new Vector2(0.5f, 0.5f);
            uvIndex++;
            while (vertexIndex < numberOfSections + 1)
            {
                for (int i = 0; i < numberOfSectionsByRange; i++)
                {
                    verts[vertexIndex + i * numberOfSections] = transformOffset.MultiplyPoint(new Vector3((i + 1) * halfDiameter / numberOfSectionsByRange * Mathf.Cos(angle), 0, (i + 1) * halfDiameter / numberOfSectionsByRange * Mathf.Sin(angle)));
                    uvs[vertexIndex + i * numberOfSections] = new Vector2(0.5f + (i + 1) * 1f / numberOfSectionsByRange * 0.5f * Mathf.Cos(angle), 0.5f + (i + 1) * 1f / numberOfSectionsByRange * 0.5f * Mathf.Sin(angle));
                    colors[vertexIndex + i * numberOfSections] = gradient[i % gradient.Length].Evaluate((((float)vertexIndex - 1) / numberOfSections));
                }
                angle += maxAngle / numberOfSections;
                uvIndex++;
                vertexIndex++;
            }
            int triangleIndex = 0;
            for (int i = 0; i < numberOfSections - 1; i++)
            {
                tris[triangleIndex] = 0;
                triangleIndex++;
                tris[triangleIndex] = i + 2;
                triangleIndex++;
                tris[triangleIndex] = i + 1;
                triangleIndex++;
            }
            tris[triangleIndex] = 0;
            triangleIndex++;
            tris[triangleIndex] = 1;
            triangleIndex++;
            tris[triangleIndex] = numberOfSections;
            triangleIndex++;
            for (int k = 0; k < numberOfSectionsByRange - 1; k++)
            {
                for (int i = 0; i < numberOfSections - 1; i++)
                {
                    tris[triangleIndex] = (k + 1) * numberOfSections + i + 1;
                    triangleIndex++;
                    tris[triangleIndex] = (k) * numberOfSections + i + 1;
                    triangleIndex++;
                    tris[triangleIndex] = (k) * numberOfSections + i + 2;
                    triangleIndex++;
                    //second
                    tris[triangleIndex] = (k + 1) * numberOfSections + i + 2;
                    triangleIndex++;
                    tris[triangleIndex] = (k + 1) * numberOfSections + i + 1;
                    triangleIndex++;
                    tris[triangleIndex] = (k) * numberOfSections + i + 2;
                    triangleIndex++;
                }
                tris[triangleIndex] = (k + 1) * numberOfSections + numberOfSections;
                triangleIndex++;
                tris[triangleIndex] = (k) * numberOfSections + numberOfSections;
                triangleIndex++;
                tris[triangleIndex] = (k) * numberOfSections + 1;
                triangleIndex++;
                tris[triangleIndex] = (k) * numberOfSections + numberOfSections + 1;
                triangleIndex++;
                tris[triangleIndex] = (k + 1) * numberOfSections + numberOfSections;
                triangleIndex++;
                tris[triangleIndex] = (k) * numberOfSections + 1;
                triangleIndex++;
            }
            setMesh("2DPrimitiveCircle");
        }

        public override void updateOutline()
        {
            outline = new Outline[numberOfSections];
            int outlineIndex = 0;
            outlineMaxDistance = 0;
            int vertexCount = getVertexCount();
            for (int i = numberOfSections; i >= 1; i--)
            {
                outline[outlineIndex] = new Outline(vertexCount - i, vertexCount - i + 1, 0);
                outlineIndex++;
            }
            outline[outlineIndex - 1].secondIndex = vertexCount - numberOfSections;
            outline[outlineIndex - 1].closingVertice = vertexCount - numberOfSections;
            Vector3[] tempVerts = mesh.vertices;
            for (int i = 0; i < outlineIndex; i++)
            {
                outlineMaxDistance += (tempVerts[outline[i].secondIndex] - tempVerts[outline[i].firstIndex]).magnitude;
            }
        }
    }
}