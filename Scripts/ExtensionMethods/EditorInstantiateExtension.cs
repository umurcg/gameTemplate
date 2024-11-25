using UnityEngine;

public static class EditorInstantiateExtension
{
    public static T Instantiate<T>(T prefabComponent) where T : MonoBehaviour
    {
        var prefab = prefabComponent.gameObject;
        #if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            var instance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance == null) return null;
            return instance.GetComponent<T>();
        }
        #endif
        
        return Object.Instantiate(prefabComponent).GetComponent<T>();
    }
    
    public static T Instantiate<T>(T prefabComponent, Transform parent) where T : MonoBehaviour
    {
        var prefab = prefabComponent.gameObject;
        #if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            var instance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
            if (instance == null) return null;
            return instance.GetComponent<T>();
        }
        #endif
        
        return Object.Instantiate(prefabComponent, parent).GetComponent<T>();
    }
    
    public static T Instantiate<T>(T prefabComponent, Vector3 position, Quaternion rotation) where T : MonoBehaviour
    {
        var prefab = prefabComponent.gameObject;
        #if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            var instance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance == null) return null;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance.GetComponent<T>();
        }
        #endif
        
        return Object.Instantiate(prefabComponent, position, rotation).GetComponent<T>();
    }
    
    public static T Instantiate<T>(T prefabComponent, Vector3 position, Quaternion rotation, Transform parent) where T : MonoBehaviour
    {
        var prefab = prefabComponent.gameObject;
        #if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            var instance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
            if (instance == null) return null;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance.GetComponent<T>();
        }
        #endif
        
        return Object.Instantiate(prefabComponent, position, rotation, parent).GetComponent<T>();
    }

    public static void Destroy<T>(T component) where T : MonoBehaviour
    {
        #if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            Object.DestroyImmediate(component.gameObject);
            return;
        }
        #endif
        
        Object.Destroy(component.gameObject);
        return;
    }
    
    public static GameObject Instantiate(GameObject prefab)
    {
        #if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            var instance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance == null) return null;
            return instance;
        }
        #endif
        
        return Object.Instantiate(prefab);
    }
    
    
    
}