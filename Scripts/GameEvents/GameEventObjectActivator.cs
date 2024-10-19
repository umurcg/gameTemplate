using UnityEngine;

namespace CorePublic.GameEvents
{
    public class GameEventObjectActivator : MonoBehaviour, IGameEventListener
    {
        public bool startActive = false;
        public bool activateOnEvent = true;
        public GameEventReference eventReference;

        [SerializeField] private bool checkPersistantEventsAtStart = true;

        public void Start()
        {
            if (eventReference != null && eventReference.gameEvent != null)
            {
                eventReference.gameEvent.RegisterListener(this);
            }
            gameObject.SetActive(startActive);

            if (checkPersistantEventsAtStart && eventReference != null && eventReference.gameEvent != null && eventReference.gameEvent.numberOfInvocations > 0)
            {
                OnEventRaise();
            }
        }

        public void OnEventRaise()
        {
            gameObject.SetActive(activateOnEvent);
            Destroy(this);
        }

        public void OnDestroy()
        {
            if (eventReference != null && eventReference.gameEvent != null)
            {
                eventReference.gameEvent.UnregisterListener(this);
            }
        }
    }
}
