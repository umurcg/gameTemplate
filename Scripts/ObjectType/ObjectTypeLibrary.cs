using System;
using UnityEngine;

namespace ObjectType
{
    public class ObjectTypeLibrary: ScriptableObject
    {
        public ObjectTypeController[] prefabs;
        public Type[] objectTypes;
        public string DefaultType => objectTypes != null && objectTypes.Length > 0 ? objectTypes[0].typeName : "NoType";

        public static ObjectTypeLibrary Find()
        {
            var library = Resources.Load<ObjectTypeLibrary>(nameof(ObjectTypeLibrary));
            
#if UNITY_EDITOR
            if (library == null)
            {
                //Create the library
                library = CreateInstance<ObjectTypeLibrary>();
                library.objectTypes = Array.Empty<Type>();
                
                //Save the library
                var path = "Assets/Resources/" + nameof(ObjectTypeLibrary) + ".asset";
                UnityEditor.AssetDatabase.CreateAsset(library, path);
                
                //Refresh the database
                UnityEditor.AssetDatabase.Refresh();
            }
#endif
            
            return library;
        }

        public Type FindObjectType(string typeName)
        {
            foreach (var objectType in objectTypes)
            {
                if (objectType.typeName == typeName)
                {
                    return objectType;
                }
            }

            return new Type();
        }
        
        public string[] GetObjectTypeNames()
        {
            var names = new string[objectTypes.Length];
            for (var i = 0; i < objectTypes.Length; i++)
            {
                names[i] = objectTypes[i].typeName;
            }

            return names;
        }
    }
}