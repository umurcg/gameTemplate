using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CorePublic.Helpers
{
    public class DelayedCall : MonoBehaviour
    {
        public UnityEvent call;
        public float delay = 2f;
    
        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            call.Invoke();
        }

    
    }
}
