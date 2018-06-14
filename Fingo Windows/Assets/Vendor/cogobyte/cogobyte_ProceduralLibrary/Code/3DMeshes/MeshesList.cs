using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    /**
     * List of all available 3D meshes
     * */
    [CreateAssetMenu(fileName = "MeshesList", menuName = "Cogobyte/ProceduralLibrary/3DMeshes/MeshesList", order = 3)]
    [System.Serializable]
    public class MeshesList : ScriptableObject
    {
        public List<SolidMesh> meshesList;
    }
}