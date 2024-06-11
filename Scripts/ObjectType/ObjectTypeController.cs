using Lean.Pool;
using Unity.VisualScripting;
using UnityEngine;

namespace ObjectType
{
    public class ObjectTypeController : MonoBehaviour
    {
        public bool pooling;
        public ObjectTypeEnum testType;
        
        [SerializeField]private bool setInactiveListeners;

        public string TypeName
        {
            get
            {
                if (Type.IsNull())
                {
                    return null;
                }

                return Type.typeName;
            }
        }
        
        public Type Type
        {
            get;
            private set;

        }
        public bool Pooled { get; private set; }

        public virtual void SetObjectType(Type type)
        {
            if (type.IsNull())
            {
                Debug.LogError("Type is null");
                return;
            }

            this.Type = type;
            var listeners = GetComponentsInChildren<IObjectTypeListener>(setInactiveListeners);
            foreach (var listener in listeners)
            {
                listener.OnObjectTypeChanged(type);
            }
        }
        
        public virtual void Destroy()
        {
            if (Pooled)
            {
                LeanPool.Despawn(gameObject);
            }
            else
            {
                if(Application.isPlaying)
                    Destroy(gameObject);
                else
                    DestroyImmediate(gameObject);
            }
        }
        
        public static ObjectTypeController Spawn(string typeName, int prefabIndex)
        {
            var objectType = ObjectTypeLibrary.Find().FindObjectType(typeName);
            if (objectType.IsNull())
            {
                return null;
            }

            var prefabs = ObjectTypeLibrary.Find().prefabs;
            if (prefabIndex < 0 || prefabIndex >= prefabs.Length) return null;
            var prefab = prefabs[prefabIndex];
            ObjectTypeController instance = null;

#if UNITY_EDITOR

            if (!Application.isPlaying)
            {
                instance = (UnityEditor.PrefabUtility.InstantiatePrefab(prefab.gameObject) as GameObject)?.GetComponent<ObjectTypeController>();
            }
            
#endif
            if (instance == null)
            {
                if (prefab.pooling)
                {
                    instance = LeanPool.Spawn(prefab.gameObject).GetComponent<ObjectTypeController>();
                    instance.Pooled=true;
                }
                else
                {
                    instance = Instantiate(prefab).GetComponent<ObjectTypeController>();
                }
            }

            instance.SetObjectType(objectType);
            return instance;
        }
    }
}