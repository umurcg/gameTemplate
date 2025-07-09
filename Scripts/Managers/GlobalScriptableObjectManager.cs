using System;
using System.Collections.Generic;
using System.Linq;
using CorePublic.Helpers;
using CorePublic.ScriptableObjects;
using UnityEngine;

namespace CorePublic.Managers
{
    public class GlobalScriptableObjectManager : BaseManager<GlobalScriptableObjectManager>
    {
        public List<ScriptableObject> GlobalScriptableObjects;




        public T FindGlobalSO<T>() where T : GlobalSO<T>
        {
            var globalSO = GlobalScriptableObjects.FirstOrDefault(x => x.GetType() == typeof(T));
            if (globalSO == null)
            {
                Debug.LogError($"GlobalSO of type {typeof(T).Name} not found. Please ensure you have created a GlobalSO of type {typeof(T).Name} and assigned it to the GlobalScriptableObjectManager.");
                return null;
            }
            return globalSO as T;
        }
        
#if UNITY_EDITOR

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#else
        [ContextMenu("Find All GlobalSOs")]
#endif
        public void FindAllGlobalSOs()
        {
            //Find all globalSOs in the project
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(ScriptableObject).Name}");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

                //If the asset is a GlobalSO, add it to the GlobalSOs list
                if (IsGlobalSO(asset) && GlobalScriptableObjects.Contains(asset) == false)
                {
                    GlobalScriptableObjects.Add(asset);
                }
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }

        private bool IsGlobalSO(ScriptableObject asset)
        {
            if (asset == null) return false;

            var type = asset.GetType();
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition().Name.StartsWith("GlobalSO"))
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }
#endif
    }
}