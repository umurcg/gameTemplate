
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [UnityEngine.CreateAssetMenu(fileName = "GameEvent", menuName = "Game Events/Event")]
    public class GameEvent : ScriptableObject
    {
        public bool persistent;
        private List<IGameEventListener> listeners = new List<IGameEventListener>();
        public string guid;

        public int numberOfInvocations;

        public void Initialize()
        {
            listeners.Clear();
            if (persistent)
            {
                numberOfInvocations = PlayerPrefs.GetInt(guid, 0);
            }
            else
            {
                numberOfInvocations = 0;
            }
        }

        #if UNITY_EDITOR
        private void Reset()
        {
            if (persistent)
            {
                GenerateGuid();
            }
        }

        private void GenerateGuid()
        {
            guid = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif

        public void RegisterListener(IGameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener listener)
        {
            listeners.Remove(listener);
        }

        [ContextMenu("Invoke")]
        public void Invoke()
        {
            numberOfInvocations++;
            if (persistent) PlayerPrefs.SetInt(guid, numberOfInvocations);
            Raise();
        }

        protected void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--) 
            {
                listeners[i].OnEventRaise();
            }
        }

        [ContextMenu("Get Listeners Names")]
        public string[] GetListenersNames()
        {
            string[] names = new string[listeners.Count];
            
            foreach (IGameEventListener listener in listeners)
            {
                names[listeners.IndexOf(listener)] = (listener as MonoBehaviour)?.name;
            }

            return names;
        }

        public static GameEvent[] GetAllGameEvents()
        {
            return Resources.LoadAll<GameEvent>("GameEvents");
        }
    }
}
