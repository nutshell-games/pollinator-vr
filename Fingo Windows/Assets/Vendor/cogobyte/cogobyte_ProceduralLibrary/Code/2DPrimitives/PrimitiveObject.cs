using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //Used to view 2D primitive object in play mode
    //Drag this script on empty object and add a primitive to pgMesh property
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class PrimitiveObject : MonoBehaviour
    {
        public Primitive pgMesh;
        void Start()
        {
        }
        void Update()
        {
            pgMesh.generate();
            GetComponent<MeshFilter>().mesh.Clear();
            GetComponent<MeshFilter>().mesh = pgMesh.mesh;
            GetComponent<MeshFilter>().mesh.RecalculateNormals();
        }
    }
}