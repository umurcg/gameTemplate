using UnityEngine;
using UnityEngine.Events;

namespace CorePublic.Helpers
{
    public class KeyEvent : MonoBehaviour
    {
        public UnityEvent keyPress=new UnityEvent();
        public KeyCode[] keys;

        // Update is called once per frame
        void Update()
        {
            
            int numberOfKeys = 0;
            bool upKeyExists = false;
            foreach (KeyCode code in keys)
            {
                if (Input.GetKey(code) || Input.GetKeyUp(code))
                    numberOfKeys++;
                upKeyExists |= Input.GetKeyUp(code);
            }

            if (numberOfKeys == keys.Length && upKeyExists)
                keyPress.Invoke();
         
        }

        
    }
}