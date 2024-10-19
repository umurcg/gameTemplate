
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CorePublic.GameEvents
{
    public class MultiGameEventListener : MonoBehaviour, IGameEventListener
    {
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetGameEvents))]
#endif
        public GameEvent[] gameEvent;
        public UnityEvent response;
        public int registerDelay = 0;

        public void Start()
        {
            if (registerDelay == 0)
            {
                foreach (var ge in gameEvent)
                {
                    ge.RegisterListener(this);
                }
            }
            else
            {
                StartCoroutine(LateRegister());
            }
        }

        public IEnumerator LateRegister()
        {
            for (int i = 0; i < registerDelay; i++)
                yield return new WaitForFixedUpdate();
            foreach (var ge in gameEvent)
                ge.RegisterListener(this);
        }

        public void OnDestroy()
        {
            foreach (var ge in gameEvent)
                ge.UnregisterListener(this);
        }

        public void OnEventRaise()
        {
            response?.Invoke();
        }

#if ODIN_INSPECTOR
        private GameEvent[] GetGameEvents()
        {
            return GameEvent.GetAllGameEvents();
        }
#endif
    }
}
