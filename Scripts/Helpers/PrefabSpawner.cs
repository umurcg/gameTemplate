
using Lean.Pool;
using UnityEngine;

namespace Helpers
{
    public class PrefabSpawner : MonoBehaviour
    {
        [SerializeField] public GameObject prefab;
        [SerializeField] private bool hasKillTimer;
        [SerializeField] private float lifeTime = 1f;
        public bool spawnAsChild = false;

        public void Spawn()
        {
            Spawn(transform.position);
        }
    
        public void Spawn(Vector3 position)
        {
            var particleInstance = LeanPool.Spawn(prefab);
            particleInstance.transform.position = position;
            if (spawnAsChild)
            {
                particleInstance.transform.SetParent(transform);
            }
            if (hasKillTimer)
            {
                LeanPool.Despawn(particleInstance, lifeTime);
            }
        }
    
        public void SetLifeTime(float lifeTime)
        {
            this.lifeTime = lifeTime;
        }
    
#if UNITY_EDITOR
        // Method for editor functionality, if needed
        private void OnValidate()
        {
            // Here you can add some editor-only checks or functionality
            // for example, to enforce certain conditions.
        }
#endif
    }
}
