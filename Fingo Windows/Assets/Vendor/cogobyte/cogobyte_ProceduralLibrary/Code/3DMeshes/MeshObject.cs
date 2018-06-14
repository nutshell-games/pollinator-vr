using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to view 3D mesh object in play mode
//Drag this script on empty object and add a cogobyte_Pl_3DMesh mesh to pgMesh property
namespace Cogobyte.ProceduralLibrary
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshObject : MonoBehaviour
    {
        public SolidMesh pgMesh;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            pgMesh.generate();
            GetComponent<MeshFilter>().mesh.Clear();
            GetComponent<MeshFilter>().mesh = pgMesh.mesh;
            GetComponent<MeshFilter>().mesh.RecalculateNormals();
        }
    }
}