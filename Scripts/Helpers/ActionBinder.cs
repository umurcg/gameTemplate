using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Reflection;

[Serializable] public class UnityEventFloat : UnityEvent<float> { }
[Serializable] public class UnityEventString : UnityEvent<string> { }
[Serializable] public class UnityEventInt : UnityEvent<int> { }

[RequireComponent(typeof(MonoBehaviour))]
public class ActionBinder : MonoBehaviour
{
    [SerializeField] private List<UnityEvent> _unityEvents = new List<UnityEvent>();
    [SerializeField] private List<UnityEventFloat> _unityEventsFloat = new List<UnityEventFloat>();
    [SerializeField] private List<UnityEventString> _unityEventsString = new List<UnityEventString>();
    [SerializeField] private List<UnityEventInt> _unityEventsInt = new List<UnityEventInt>();

    private void Start()
    {
        BindActionsToEvents();
    }

    private void BindActionsToEvents()
    {
        var components = GetComponents<MonoBehaviour>();

        foreach (var component in components)
        {
            var type = component.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();

                if (parameters.Length == 0 && method.ReturnType == typeof(void))
                {
                    var unityEvent = new UnityEvent();
                    unityEvent.AddListener(() => method.Invoke(component, null));
                    _unityEvents.Add(unityEvent);
                }
                else if (parameters.Length == 1 && method.ReturnType == typeof(void))
                {
                    var parameterType = parameters[0].ParameterType;

                    if (parameterType == typeof(float))
                    {
                        var unityEventFloat = new UnityEventFloat();
                        unityEventFloat.AddListener((float value) => method.Invoke(component, new object[] { value }));
                        _unityEventsFloat.Add(unityEventFloat);
                    }
                    else if (parameterType == typeof(string))
                    {
                        var unityEventString = new UnityEventString();
                        unityEventString.AddListener((string value) => method.Invoke(component, new object[] { value }));
                        _unityEventsString.Add(unityEventString);
                    }
                    else if (parameterType == typeof(int))
                    {
                        var unityEventInt = new UnityEventInt();
                        unityEventInt.AddListener((int value) => method.Invoke(component, new object[] { value }));
                        _unityEventsInt.Add(unityEventInt);
                    }
                }
            }
        }
    }
}
