using UnityEngine;

namespace CorePublic.Helpers
{
    // using Unity.AI.Navigation;


    // [RequireComponent(typeof(NavMeshSurface))]
    public class NavmeshBaker : MonoBehaviour
    {
        // NavMeshSurface surface;
        // public Action OnNavMeshBaked;
        //
        // public enum BakeType
        // {
        //     OnStart,
        //     OnNewLevelLoaded,
        //     OnGameStarted
        // }
        //
        // public BakeType bakeType;
        //
        // // Start is called before the first frame update
        // void Start()
        // {
        //     surface = GetComponent<NavMeshSurface>();
        //     if (bakeType == BakeType.OnStart)
        //         surface.BuildNavMesh();
        //     else if (bakeType == BakeType.OnNewLevelLoaded)
        //         ActionManager.Instance.OnNewLevelLoaded += BakeNavMesh;
        //     else if (bakeType == BakeType.OnGameStarted)
        //         ActionManager.Instance.OnGameStarted += BakeNavMesh;
        //
        // }
        //
        // private void BakeNavMesh()
        // {
        //     surface.BuildNavMesh();
        //     OnNavMeshBaked?.Invoke();
        // }

    }

}