using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    /**
     * Contains all primitive shapes available. 
     */
    [CreateAssetMenu(fileName = "PrimitiveList", menuName = "Cogobyte/ProceduralLibrary/Primitives/PrimitiveList", order = 3)]
    [System.Serializable]
    public class PrimitivesList : ScriptableObject
    {
        public List<Primitive> primitivesList;
    }
}