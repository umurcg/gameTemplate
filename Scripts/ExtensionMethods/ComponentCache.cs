using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class ComponentCache
{
    private static ConditionalWeakTable<GameObject, Dictionary<System.Type, Component>> cachedComponents;
    private static ConditionalWeakTable<GameObject, Dictionary<System.Type, Component>> cachedChildComponents;

    static ComponentCache()
    {
        InitializeCache();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InitializeCache()
    {
        cachedComponents = new ConditionalWeakTable<GameObject, Dictionary<System.Type, Component>>();
        cachedChildComponents = new ConditionalWeakTable<GameObject, Dictionary<System.Type, Component>>();
    }

    public static T GetComponentCached<T>(GameObject gameObject) where T : Component
    {
        if (gameObject == null)
            return null;

        var cache = cachedComponents.GetOrCreateValue(gameObject);
        return GetFromCache<T>(gameObject, cache, gameObject.GetComponent<T>);
    }

    public static T GetComponentCachedInChildren<T>(GameObject gameObject, bool includeInactive = false) where T : Component
    {
        if (gameObject == null)
            return null;

        var cache = cachedChildComponents.GetOrCreateValue(gameObject);
        return GetFromCache<T>(gameObject, cache, () => gameObject.GetComponentInChildren<T>(includeInactive));
    }

    private static T GetFromCache<T>(GameObject gameObject, Dictionary<System.Type, Component> cache, System.Func<T> getComponentFunc) where T : Component
    {
        var type = typeof(T);

        if (cache.TryGetValue(type, out var component))
        {
            if (component != null && component)  // Unity's null check
            {
                return (T)component;
            }
            cache.Remove(type);
        }

        component = getComponentFunc();
        if (component != null)
        {
            cache[type] = component;
        }
        else
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning($"Component of type {type} not found on {gameObject.name} or its children");
            #endif
        }

        return (T)component;
    }

    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    private static void RegisterEditorUpdate()
    {
        UnityEditor.EditorApplication.update -= CleanupCache;
        UnityEditor.EditorApplication.update += CleanupCache;
    }

    private static void CleanupCache()
    {
        if (cachedComponents == null || cachedChildComponents == null)
        {
            InitializeCache();
            return;
        }

        CleanupCacheTable(cachedComponents);
        CleanupCacheTable(cachedChildComponents);
    }

    private static void CleanupCacheTable(ConditionalWeakTable<GameObject, Dictionary<System.Type, Component>> cacheTable)
    {
        var keysToRemove = new List<GameObject>();

        foreach (var entry in cacheTable)
        {
            if (entry.Key == null)
            {
                keysToRemove.Add(entry.Key);
            }
            else
            {
                var componentsToRemove = new List<System.Type>();
                foreach (var componentEntry in entry.Value)
                {
                    if (componentEntry.Value == null)
                    {
                        componentsToRemove.Add(componentEntry.Key);
                    }
                }
                foreach (var type in componentsToRemove)
                {
                    entry.Value.Remove(type);
                }
            }
        }

        foreach (var key in keysToRemove)
        {
            cacheTable.Remove(key);
        }
    }
    #endif

    public static void ClearCache()
    {
        InitializeCache();
    }
}

public static class GameObjectExtensions
{
    public static T GetCachedComponent<T>(this GameObject gameObject) where T : Component
    {
        return ComponentCache.GetComponentCached<T>(gameObject);
    }

    public static T GetCachedComponentInChildren<T>(this GameObject gameObject, bool includeInactive = false) where T : Component
    {
        return ComponentCache.GetComponentCachedInChildren<T>(gameObject, includeInactive);
    }
}