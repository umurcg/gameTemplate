using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ActionTriggers;
using Core.Interfaces;
using Helpers;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Managers
{
    public delegate void Callback<T>(T param);
    public delegate void Callback();

    public class ActionManager : Singleton<ActionManager>
    {
        //Template actions
        public Action OnGameStarted;
        public Action OnGameEnded;
        public Action OnNewLevelLoaded;
        public Action OnGameLost;
        public Action OnGameWin;
        public Action OnGameRestarted;
        public Action<int> OnLevelChanged;
        public Action<float> OnGameMoneyChanged;
        public Action<float> OnLevelMoneyChanged;
     
        
        //Editor descriptions. Fill for the ones that are not obvious.
        public Dictionary<string, string> Descriptions = new Dictionary<string, string>()
        {
            { "OnGameStarted", "Triggered when player or auto play functionality starts the game" },
            { "OnGameLost", "Triggered when player lost the game" },
        };
        
        public void ConnectEventToAction(UnityEvent @event, string actionName)
        {
            var actionOwners = GetActionComponents();

            foreach (MonoBehaviour actionOwner in actionOwners)
            {
                var fields = actionOwner.GetType().GetFields();
                foreach (var fi in fields)
                {
                    if (fi.Name == actionName)
                    {
                        var action = (Action)fi.GetValue(actionOwner);
                        action += @event.Invoke;
                        fi.SetValue(actionOwner, action);
                        return;
                    }
                }
            }
        }
        

        public void ConnectEventToAction<T>(UnityEvent<T> @event, string actionName)
        {
            var actionOwners = GetActionComponents();

            foreach (MonoBehaviour actionOwner in actionOwners)
            {
                var fields = actionOwner.GetType().GetFields();
                foreach (var fi in fields)
                {
                    if (fi.Name == actionName)
                    {
                        var action = (Action<T>)fi.GetValue(actionOwner);
                        action += @event.Invoke;
                        fi.SetValue(actionOwner, action);
                        return;
                    }
                }
            }
        }


        public List<MonoBehaviour> GetActionComponents()
        {
            List<MonoBehaviour> actionOwners = new List<MonoBehaviour>();
            actionOwners.Add(this);

            var ss = FindObjectsOfType<MonoBehaviour>().OfType<IAction>();
            foreach (IAction s in ss) {
                actionOwners.Add(s as MonoBehaviour);
            }

            return actionOwners;
        }

        public string[] GetActionNames()
        {
            // Debug.Log("Getting action name!");
            
            List<string> eventNames = new();

            var actionOwners = GetActionComponents();
            
            foreach (var o in actionOwners)
            {
                foreach (var fi in o.GetType().GetFields())
                {
                    if (fi.FieldType == typeof(Action) ||
                        fi.FieldType == typeof(Action<int>) ||
                        fi.FieldType == typeof(Action<float>) ||
                        fi.FieldType == typeof(Action<string>) ||
                        fi.FieldType == typeof(Action<bool>)
                        )
                    {
                        eventNames.Add(fi.Name);
                    }
                }
            }
            return eventNames.ToArray();
        }

   

        public ActionTrigger.Event.EventTypes GetActionType(string actionName)
        {
            var actionOwners = GetActionComponents();

            foreach (MonoBehaviour owner in actionOwners)
            {
                foreach (var fi in owner.GetType().GetFields())
                {
                    if (fi.Name == actionName)
                    {
                        if (fi.FieldType == typeof(Action)) return ActionTrigger.Event.EventTypes.Void;
                        if (fi.FieldType == typeof(Action<int>)) return ActionTrigger.Event.EventTypes.Int;
                        if (fi.FieldType == typeof(Action<float>)) return ActionTrigger.Event.EventTypes.Float;
                        if (fi.FieldType == typeof(Action<string>)) return ActionTrigger.Event.EventTypes.String;
                        if (fi.FieldType == typeof(Action<bool>)) return ActionTrigger.Event.EventTypes.Bool;
                    }
                }

            }
            
            Debug.LogError("Action not found: " + actionName);
            return ActionTrigger.Event.EventTypes.Void;
        }
        
  
        
        public void AddListenerToAction<T>(string actionName,Callback<T> callback)
        {
            var actionOwners = GetActionComponents();

            foreach (MonoBehaviour actionOwner in actionOwners)
            {
                var fields= actionOwner.GetType().GetFields();
                foreach (var fi in fields)
                {
                    if (fi.Name == actionName)
                    {
                        var action=(Action<T>)fi.GetValue(actionOwner);
                        action += callback.Invoke;
                        fi.SetValue(actionOwner, action);
                        return;
                    }
                }
            }

            Debug.LogError($"Action {actionName} not found");
        }
        
        public void AddListenerToAction(string actionName,Callback callback)
        {
            var actionOwners = GetActionComponents();

            foreach (MonoBehaviour actionOwner in actionOwners)
            {
                var fields= actionOwner.GetType().GetFields();
                foreach (var fi in fields)
                {
                    if (fi.Name == actionName)
                    {
                        var action=(Action)fi.GetValue(actionOwner);
                        action += callback.Invoke;
                        fi.SetValue(actionOwner, action);
                        return;
                    }
                }
            }

            Debug.LogError($"Action {actionName} not found");
        }
        
    }
}