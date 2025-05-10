

using System;
using System.Collections.Generic;

public static class UIEventBus
{
    private static Dictionary<UIEventType, Action> eventListeners = new Dictionary<UIEventType, Action>();

    public static void AddListener(UIEventType eventType, Action callback)
    {
        if (eventListeners.TryGetValue(eventType, out var listeners))
        {
            listeners += callback;
            eventListeners[eventType] = listeners;
        }
        else
        {
            eventListeners[eventType] = callback;
        }
    }

    public static void RemoveListener(UIEventType eventType, Action callback)
    {
        if (eventListeners.TryGetValue(eventType, out var listeners))
        {
            listeners -= callback;
            if (listeners == null)
            {
                eventListeners.Remove(eventType);
            }
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
    OnModelChanged,
    OnModelDeleted,
}