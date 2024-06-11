using System;

namespace ObjectType
{
    // Wrapper for drawing type as dropdown in inspector
    [Serializable]
    public class ObjectTypeEnum
    {
        public string typeName = "NoType";

        public static ObjectTypeEnum GetEnum(string type)
        {
            return new ObjectTypeEnum {typeName = type};
        }
    }

    // Editor class for drawing type as dropdown in inspector
}