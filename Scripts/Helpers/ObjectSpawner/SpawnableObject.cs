using UnityEngine;

namespace CorePublic.Helpers.ObjectSpawner
{
    public class SpawnableObject: MonoBehaviour
    {
        public Spawner spawner=>_spawner;
        [HideInInspector] private Spawner _spawner;
        
        public void SetSpawner(Spawner spawnerToSet)
        {
            if (_spawner)
            {
                Debug.LogWarning("Spawner is already set");
                return;
            }
            _spawner = spawnerToSet;
        }
        
        public void Destroy()
        {
            if (_spawner)
            {
                _spawner.DestroyObject();   
            }
            else
            {
                Debug.LogWarning("Spawner is not set");
                Destroy(gameObject);
            }
        }
       
        public void RemoveFromSpawner()
        {
            if(_spawner==null) return;
            _spawner.RemoveObject(this);
            _spawner = null;
        }
        
    }
}