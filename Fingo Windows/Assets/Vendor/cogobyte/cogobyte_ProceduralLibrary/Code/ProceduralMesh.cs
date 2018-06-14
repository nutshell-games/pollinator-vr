using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //The main abstract class for used shapes and meshes
    //Meshes that will be used for extrudes in arrows must derive from these
    public abstract class ProceduralMesh : ScriptableObject
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale = new Vector3(1, 1, 1);
        [System.NonSerialized]
        public Matrix4x4 transformOffset;
        [System.NonSerialized]
        public Mesh mesh;
        [System.NonSerialized]
        public int vertexCount;
        [System.NonSerialized]
        public int trisCount;
        [System.NonSerialized]
        public Vector3[] verts;
        [System.NonSerialized]
        public int[] tris;
        [System.NonSerialized]
        public Vector2[] uvs;
        [System.NonSerialized]
        public Color32[] colors;

        //Returns expected vertex count
        public virtual int getVertexCount()
        {
            if (mesh != null) return mesh.vertices.Length;
            return 0;
        }

        //Returns expected triangle count
        public virtual int getTrisCount()
        {
            if (mesh != null) return mesh.triangles.Length;
            return 0;
        }

        //Any mesh can be set with default position, rotation and scale within the mesh
        public void prepareOffset()
        {
            transformOffset = new Matrix4x4();
            transformOffset.SetTRS(position, Quaternion.Euler(rotation), scale);
        }

        //Initializes required arrays, all of these must be filled in generate
        public void prepareMesh()
        {
            mesh = new Mesh();
            vertexCount = getVertexCount();
            trisCount = getTrisCount();
            verts = new Vector3[vertexCount];
            tris = new int[trisCount];
            uvs = new Vector2[vertexCount];
            colors = new Color32[vertexCount];
        }

        public virtual void generate()
        {
            prepareOffset();
            prepareMesh();
        }

        //Default color values
        protected void prepareColorGradient(ref Gradient[] gradient)
        {
            if (gradient == null)
            {
                gradient = new Gradient[1];
            }
            if (gradient.Length == 0)
            {
                gradient = new Gradient[1];
            }
            if (gradient[0] == null)
            {
                gradient[0] = new Gradient();
                GradientColorKey[] gck;
                GradientAlphaKey[] gak;
                gck = new GradientColorKey[2];
                gck[0].color = Color.white;
                gck[0].time = 0.0F;
                gck[1].color = Color.white;
                gck[1].time = 1.0F;
                gak = new GradientAlphaKey[2];
                gak[0].alpha = 1.0F;
                gak[0].time = 0.0F;
                gak[1].alpha = 1.0F;
                gak[1].time = 1.0F;
                gradient[0].SetKeys(gck, gak);
            }
        }

        //Sets the mesh to calculated vertices
        protected void setMesh(string name)
        {
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.uv = uvs;
            mesh.colors32 = colors;
            mesh.name = name;
        }
    }
}