
using UnityEngine;

namespace CustomTypes
{
    [System.Serializable]
    public class Vector3Boolean
    {
        public bool x;
        public bool y;
        public bool z;

        public Vector3Boolean()
        {
            x = false;
            y = false;
            z = false;
        }

        public Vector3Boolean(bool x, bool y, bool z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x ? 1f : 0f, y ? 1f : 0f, z ? 1f : 0f);
        }
    }
}
