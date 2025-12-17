using System;
using System.Collections.Generic;
using UnityEngine;

public static class Eventer
{
    private static readonly Dictionary<Type, Delegate> Subscribers = new();

    public static void Subscribe<T>(Action<T> handler)
    {
        var type = typeof(T);
        if (!Subscribers.TryAdd(type, handler)) Subscribers[type] = (Action<T>)Subscribers[type] + handler;
    }

    public static void Unsubscribe<T>(Action<T> handler)
    {
        var type = typeof(T);
        if (!Subscribers.TryGetValue(type, out var existing)) return;
        existing = (Action<T>)existing - handler;
        if (existing == null) Subscribers.Remove(type);
        else Subscribers[type] = existing;
    }

    public static void Invoke<T>(T eventData) where T : Event
    {
        var type = typeof(T);
        if (Subscribers.TryGetValue(type, out var del)) (del as Action<T>)?.Invoke(eventData);
    }

    public static void ClearSubscribers() { Subscribers.Clear(); }
}