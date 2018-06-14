using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //Custom mesh transfers the complete mesh to a cogobyte_Pl_3DMesh. Used mesh should have vertex colors
    [CreateAssetMenu(fileName = "CustomMesh", menuName = "Cogobyte/ProceduralLibrary/3DMeshes/CustomMesh", order = 1)]
    public class CustomMesh : SolidMesh
    {
        public Mesh customMesh;

        public override int getVertexCount()
        {
            return customMesh.vertices.Length;
        }

        public override int getTrisCount()
        {
            return customMesh.triangles.Length;
        }

        public override void generate()
        {
            base.generate();
            Vector3[] tempVertices = customMesh.vertices;
            for (int i = 0; i < tempVertices.Length; i++)
            {
                verts[i] = transformOffset.MultiplyPoint(tempVertices[i]);
            }
            uvs = customMesh.uv;
            tris = customMesh.triangles;
            colors = customMesh.colors32;
            setMesh("CustomMesh");
        }
    }
}
