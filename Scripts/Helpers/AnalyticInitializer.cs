using System.Collections;
using CorePublic.Interfaces;
using UnityEngine;

namespace CorePublic.Helpers
{
    public abstract class AnalyticInitializer: MonoBehaviour, IPrerequisite
    {
        public AnalyticInitializer[] dependencies;
        public bool initializeAtStart = true;
        public abstract bool IsInitialized();
        public abstract void Initialize();

        private IEnumerator Start()
        {
            if (initializeAtStart)
            {
                foreach (AnalyticInitializer dependency in dependencies)
                {
                    while (!dependency.IsInitialized())
                    {
                        yield return null;
                    }
                }
                Initialize();
            }
        }

        public bool IsMet()
        {
            return IsInitialized();
        }
    }
}