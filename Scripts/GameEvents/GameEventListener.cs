
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameEvents
{
    public class GameEventListener : MonoBehaviour, IGameEventListener
    {
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

        // Removed the dependency on Odin Inspector for getting the GameEvents
        // Ensure you implement this method to retrieve all GameEvents
        // for example, by referencing them directly or using a Resource folder to load them
        public GameEvent[] GetGameEvents()
        {
            // Implement logic to retrieve all GameEvents if required
            return new GameEvent[]{};
        }
    }
}
