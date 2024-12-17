using System;
using CorePublic.Managers;
using Lean.Pool;
using UnityEngine;

namespace CorePublic.Helpers.ObjectSpawner
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private SpawnableObject objectToSpawn;
        [SerializeField] private Transform spawnParent;
        [SerializeField] private bool poolObject;
        [SerializeField] private bool autoSpawn;
        [SerializeField] private float spawnDelay;

        public SpawnableObject CurrentSpawnedObject => _spawnedObject;
        private SpawnableObject _spawnedObject;

        protected void Start()
        {
            if (objectToSpawn == null)
            {
                throw new Exception("Object to spawn is not set");
            }

            if (spawnParent == null) spawnParent = transform;
            GlobalActions.OnNewLevelLoaded += OnNewLevelLoaded;
        }

        private void OnNewLevelLoaded()
        {
            if (autoSpawn) Invoke(nameof(SpawnObject), spawnDelay);

        }

        public void SpawnObject()
        {
            if (_spawnedObject != null)
            {
                throw new Exception("Object already spawned");
            }

            if (poolObject)
            {
                _spawnedObject = LeanPool.Spawn(objectToSpawn.gameObject, spawnParent).GetComponent<SpawnableObject>();
            }
            else
            {
                _spawnedObject = Instantiate(objectToSpawn.gameObject, spawnParent).GetComponent<SpawnableObject>();
            }
            
            _spawnedObject.SetSpawner(this);
        }
        
        
        public void DestroyObject()
        {
            if (_spawnedObject != null)
            {
                if (poolObject)
                {
                    LeanPool.Despawn(_spawnedObject.gameObject);
                }
                else
                {
                    Destroy(_spawnedObject.gameObject);
                }
                
                _spawnedObject = null;
                if (autoSpawn) Invoke(nameof(SpawnObject), spawnDelay);
            }
            else
            {
                Debug.LogWarning("Object to destroy is not spawned by this spawner");
            }
        }

        public void RemoveObject(SpawnableObject obj)
        {
            if (_spawnedObject != null && _spawnedObject == obj)
            {
                _spawnedObject = null;
                if (autoSpawn) Invoke(nameof(SpawnObject), spawnDelay);
            }
            else
            {
                Debug.LogWarning("Object to remove is not spawned by this spawner");
            }
        }
    }
}