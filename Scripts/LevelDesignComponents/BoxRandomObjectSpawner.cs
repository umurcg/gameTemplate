
using UnityEngine;

namespace Helpers
{
    [RequireComponent(typeof(BoxCollider)), AddComponentMenu("*Reboot/LevelDesign/Box Random Object Spawner")]
    public class BoxRandomObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int numberOfObjects;
        public int numberOfTrials = 100;

        private void SpawnPrefabsRuntime()
        {
            if (!Application.isPlaying) return;

            Bounds selfBounds = GetComponent<BoxCollider>().bounds;
            VolumeFiller volumeFiller = new VolumeFiller(selfBounds, 100);

            for (int i = 0; i < numberOfObjects; i++)
            {
                var spawnedPrefab = Instantiate(prefab);
                var bounds = spawnedPrefab.GetComponent<Collider>().bounds;
                if (volumeFiller.FindPositionToBounds(bounds, out Vector3 pos))
                {
                    spawnedPrefab.transform.position = pos;
                }

                spawnedPrefab.transform.parent = transform;
            }

        }

#if UNITY_EDITOR
        private void SpawnPrefabsEditor()
        {
            if (Application.isPlaying) return;

            while (transform.childCount > 0)
                DestroyImmediate(transform.GetChild(0).gameObject);

            Bounds selfBounds = GetComponent<BoxCollider>().bounds;

            VolumeFiller volumeFiller = new VolumeFiller(selfBounds, numberOfTrials);

            int numberOfErrors = 0;

            for (int i = 0; i < numberOfObjects; i++)
            {
                var spawnedPrefab = Instantiate(prefab);
                var bounds = ConvertLocalBoundsToWorld(spawnedPrefab.GetComponentInChildren<MeshFilter>().sharedMesh.bounds, spawnedPrefab.transform);
                if (volumeFiller.FindPositionToBounds(bounds, out Vector3 pos))
                {
                    spawnedPrefab.transform.position = pos;
                }
                else
                {
                    numberOfErrors++;
                }

                spawnedPrefab.transform.parent = transform;
            }

            if (numberOfErrors != 0)
            {
                Debug.Log("<color=red>Error: Number of fit error: " + numberOfErrors + "</color>");
            }
            else
            {
                Debug.Log("<color=green>Error: All positions generated successfully</color>");
            }
        }

        private static Bounds ConvertLocalBoundsToWorld(Bounds localBounds, Transform worldTransform)
        {
            Bounds worldSelf = new Bounds(worldTransform.InverseTransformPoint(localBounds.center),
                Vector3.Scale(localBounds.size, worldTransform.localScale));
            return worldSelf;

        }

#endif
        [ContextMenu("Spawn Prefabs Runtime")]
        private void ContextMenuSpawnPrefabsRuntime()
        {
            SpawnPrefabsRuntime();
        }

        [ContextMenu("Spawn Prefabs Editor")]
        private void ContextMenuSpawnPrefabsEditor()
        {
            SpawnPrefabsEditor();
        }
    }
}
