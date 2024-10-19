using UnityEngine;

namespace CorePublic.Helpers
{
    public class ObjectInitializer: MonoBehaviour
    {
        private void Start()
        {
            var initializers = GetComponentsInChildren<IObjectInit>(true);
            foreach (IObjectInit init in initializers)
                init.Initialize();
        }
    }

   
}