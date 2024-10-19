using System.Collections;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace CorePublic.GameEvents
{
    public class GameEventListener : MonoBehaviour, IGameEventListener
    {

#if ODIN_INSPECTOR
        public bool customEvent;
        [ValueDropdown(nameof(GetGameEvents))] public GameEvent dropDownGameEvent;
        [ShowIf(nameof(customEvent))]
#endif
        
        public GameEvent gameEvent;
        public UnityEvent response;
        public int registerDelay = 0;

        public enum CallType
        {
            Everytime,
            SpecificCall,
            AfterSpecificCall,
            BeforeSpecificCall
        }

        public bool checkPersistantEventsAtStart = false;
        public CallType callType;
        public int callIndex;


        private void Awake()
        {
            #if ODIN_INSPECTOR
            if(!customEvent)
                gameEvent = dropDownGameEvent;
            #endif
        }

        public void Start()
        {
            if (checkPersistantEventsAtStart)
            {
                int numberOfInvocations = gameEvent.numberOfInvocations;
                for (int i = 0; i < numberOfInvocations; i++)
                {
                    Check(i);
                }
            }

            if (registerDelay == 0)
            {
                Register();
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
            Register();
        }

        private void Register()
        {
            gameEvent.RegisterListener(this);
        }

        public void OnDestroy()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaise()
        {
            Check(gameEvent.numberOfInvocations);
        }

        private bool Check(int index)
        {
            switch (callType)
            {
                case CallType.Everytime:
                    if (gameEvent.numberOfInvocations > 0)
                    {
                        response?.Invoke();
                        return true;
                    }

                    break;
                case CallType.SpecificCall:
                    if (gameEvent.numberOfInvocations == callIndex)
                    {
                        response?.Invoke();
                        return true;
                    }
                    break;
                case CallType.AfterSpecificCall:
                    if (gameEvent.numberOfInvocations >= callIndex)
                    {
                        response?.Invoke();
                        return true;
                    }
                    break;
                case CallType.BeforeSpecificCall:
                    if (gameEvent.numberOfInvocations < callIndex)
                    {
                        response?.Invoke();
                        return true;
                    }
                    break;

            }

            return false;

        }
        
#if ODIN_INSPECTOR
        private GameEvent[] GetGameEvents()
        {
            return GameEvent.GetAllGameEvents();
        }
#endif
    }
}
