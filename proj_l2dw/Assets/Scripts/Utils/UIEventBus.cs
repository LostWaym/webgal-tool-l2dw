

using System;
using System.Collections.Generic;
using UnityEngine;

public static class UIEventBus
{
    private static Dictionary<UIEventType, Action> eventListeners = new Dictionary<UIEventType, Action>();

    public static void AddListener(UIEventType eventType, Action callback)
    {
        if (eventListeners.TryGetValue(eventType, out var listeners))
        {
            listeners = Delegate.Combine(listeners, callback) as Action;
            eventListeners[eventType] = listeners;
            //Debug.Log("AddListener" + eventType + " " + (listeners.GetInvocationList()?.Length ?? 0));
        }
        else
        {
            eventListeners[eventType] = callback;
            //Debug.Log("AddListener" + eventType + " " + (callback.GetInvocationList()?.Length ?? 0));
        }

    }

    public static void RemoveListener(UIEventType eventType, Action callback)
    {
        if (eventListeners.TryGetValue(eventType, out var listeners))
        {
            listeners = Delegate.Remove(listeners, callback) as Action;
            if (listeners == null)
            {
                eventListeners.Remove(eventType);
                //Debug.Log("clean " + eventType);
                return;
            }
            eventListeners[eventType] = listeners;
            //Debug.Log("RemoveListener" + eventType + " " + (listeners.GetInvocationList()?.Length ?? 0));
        }

    }

    public static void SendEvent(UIEventType eventType)
    {
        if (eventListeners.TryGetValue(eventType, out var listeners))
        {
            listeners?.Invoke();
        }
    }
}

public enum UIEventType
{
    LockXChanged,
    LockYChanged,
    GroupTransformChanged,
    CameraTransformChanged,
    ModelTransformChanged,
    BGTransformChanged,
    BGChanged,
    OnModelChanged,
    OnModelDeleted,
    OnModificationSelectedIndexesChanged,
}