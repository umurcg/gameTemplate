using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;

namespace LeanHelpers
{
    public class LeanPoolEvents: MonoBehaviour, IPoolable
    {
        public UnityEvent OnSpawned;
        public UnityEvent OnDespawned;
        
        public void OnSpawn()
        {
            OnSpawned?.Invoke();
        }

        public void OnDespawn()
        {
            OnDespawned?.Invoke();
        }
    }
}