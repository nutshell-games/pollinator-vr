using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //Abstract class for 2D shapes
    [System.Serializable]
    public abstract class Primitive : ProceduralMesh
    {
        public Outline[] outline;
        [System.NonSerialized]
        public float outlineMaxDistance;

        public Outline[] getOutline()
        {
            if (outline == null)
            {
                updateOutline();
            }
            return outline;
        }

        public virtual void updateOutline()
        {
        }
    }
}