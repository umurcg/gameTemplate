using CorePublic.Interfaces;
using CorePublic.UI;
using UnityEngine;

namespace CorePublic.Helpers
{
    /// <summary>
    /// A component that automatically handles registration/unregistration of IStats implementations with StatsCanvas.
    /// Attach this component to any GameObject that has a MonoBehaviour implementing IStats.
    /// It will automatically find the IStats component and register it with the StatsCanvas system.
    /// This approach doesn't require inheritance, so your classes can inherit from whatever they need.
    /// </summary>
    public class StatsProvider : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour statsComponent;
        private IStats _statsInterface;
        private bool _isRegistered = false;

        private void Start()
        {
            if (FindStatsComponent())
            {
                RegisterWithStatsCanvas();
            }
        }

        private void OnEnable()
        {
            if (_statsInterface != null)
            {
                RegisterWithStatsCanvas();
            }
        }

        private void OnDisable()
        {
            UnregisterFromStatsCanvas();
        }

        private void OnDestroy()
        {
            UnregisterFromStatsCanvas();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Automatically try to find IStats component in editor
            if (statsComponent == null)
            {
                FindStatsComponent();
                if(statsComponent != null){
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
            else
            {
                // Validate that the assigned component implements IStats
                if (!(statsComponent is IStats))
                {
                    Debug.LogWarning($"Component {statsComponent.GetType().Name} on {gameObject.name} does not implement IStats interface!", this);
                    statsComponent = null;
                }
            }
        }
#endif

        private bool FindStatsComponent()
        {
            if (statsComponent != null && statsComponent is IStats)
            {
                _statsInterface = statsComponent as IStats;
                return true;
            }

            // Try to find any IStats component on this GameObject
            var statsComponents = GetComponents<MonoBehaviour>();
            foreach (var component in statsComponents)
            {
                if (component is IStats stats)
                {
                    statsComponent = component;
                    _statsInterface = stats;
                    return true;
                }
            }

            Debug.LogWarning($"No IStats implementation found on GameObject {gameObject.name}. Make sure you have a MonoBehaviour that implements IStats.", this);
            return false;
        }

        private void RegisterWithStatsCanvas()
        {
            if (!_isRegistered && _statsInterface != null)
            {
                StatsCanvas.RegisterStats(_statsInterface);
                _isRegistered = true;
            }
        }

        private void UnregisterFromStatsCanvas()
        {
            if (_isRegistered && _statsInterface != null)
            {
                StatsCanvas.UnregisterStats(_statsInterface);
                _isRegistered = false;
            }
        }

        /// <summary>
        /// Manually set the stats component reference (useful for runtime setup)
        /// </summary>
        public void SetStatsComponent(MonoBehaviour component)
        {
            if (component is IStats stats)
            {
                // Unregister old component if any
                UnregisterFromStatsCanvas();
                
                statsComponent = component;
                _statsInterface = stats;
                
                // Register new component
                RegisterWithStatsCanvas();
            }
            else
            {
                Debug.LogError($"Component {component.GetType().Name} does not implement IStats interface!", this);
            }
        }
    }
} 