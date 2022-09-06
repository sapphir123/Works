using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventCenter
{
    public EventCenter()
    {
        EventTable.Clear();
    }
    public static Dictionary<EventType, Delegate> EventTable = new Dictionary<EventType, Delegate>();

    public static readonly Dictionary<EventType, Delegate> EventTableRead = new Dictionary<EventType, Delegate>();

    public static void OnListenerAdding(EventType eventType, Delegate callback)
    {
        if (!EventTable.ContainsKey(eventType))
        {
            EventTable.Add(eventType, null);
        }
        Delegate d = EventTable[eventType];
        if (d != null && d.GetType() != callback.GetType())
        {
            throw new Exception($"添加事件失败：事件{eventType}对应的类型为{d.GetType()}，尝试添加的类型为{callback.GetType()}");
        }
    }

    public static void OnListenerRemoving(EventType eventType, Delegate callback)
    {
        if (EventTable.ContainsKey(eventType))
        {
            EventTable.TryGetValue(eventType, out Delegate d);
            if (d == null)
            {
                throw new Exception($"移除事件失败：事件码{eventType}没有对应的事件");
            }
            else if (d.GetType() != callback.GetType())
            {
                throw new Exception($"移除事件失败：事件码{eventType}对应的类型为{d.GetType()}，尝试移除的类型为{callback.GetType()}");
            }
        }
        else
        {
            throw new Exception($"移除事件失败：没有对应的事件码{eventType}");
        }
    }

    public static void OnListenerRemoved(EventType eventType)
    {
        if (EventTable[eventType] == null)
            EventTable.Remove(eventType);
    }

    public static void AddListener(EventType eventType, Callback callback)
    {
        OnListenerAdding(eventType, callback);
        EventTable[eventType] = (Callback)EventTable[eventType] + callback;
    }

    public static void AddListener<T>(EventType eventType, Callback<T> callback)
    {
        OnListenerAdding(eventType, callback);
        EventTable[eventType] = (Callback<T>)EventTable[eventType] + callback;
    }

    public static void AddListener<T0, T1>(EventType eventType, Callback<T0, T1> callback) where T0 : Component where T1 : EventArgs
    {
        OnListenerAdding(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1>)EventTable[eventType] + callback;
    }
    public static void AddListener<T0, T1, T2>(EventType eventType, Callback<T0, T1, T2> callback) where T0 : Component where T1 : EventArgs where T2 : class
    {
        OnListenerAdding(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1, T2>)EventTable[eventType] + callback;
    }

    public static void AddListener<T0, T1, T2, T3>(EventType eventType, Callback<T0, T1, T2, T3> callback) where T0 : Component where T1 : EventArgs where T2 : class where T3 : class
    {
        OnListenerAdding(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1, T2, T3>)EventTable[eventType] + callback;
    }

    public static void RemoveListener(EventType eventType, Callback callback)
    {
        OnListenerRemoving(eventType, callback);
        EventTable[eventType] = (Callback)EventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }

    public static void RemoveListener<T>(EventType eventType, Callback<T> callback)
    {
        OnListenerRemoving(eventType, callback);
        EventTable[eventType] = (Callback<T>)EventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }

    public static void RemoveListener<T0, T1>(EventType eventType, Callback<T0, T1> callback) where T0 : Component where T1 : EventArgs
    {
        OnListenerRemoving(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1>)EventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }

    public static void RemoveListener<T0, T1, T2>(EventType eventType, Callback<T0, T1, T2> callback) where T0 : Component where T1 : EventArgs where T2 : class
    {
        OnListenerRemoving(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1, T2>)EventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }

    public static void RemoveListener<T0, T1, T2, T3>(EventType eventType, Callback<T0, T1, T2, T3> callback) where T0 : Component where T1 : EventArgs where T2 : class where T3 : class
    {
        OnListenerRemoving(eventType, callback);
        EventTable[eventType] = (Callback<T0, T1, T2, T3>)EventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }

    public static void Broadcast(EventType eventType)
    {
        if (EventTable.TryGetValue(eventType, out Delegate d))
        {
            Callback callback = d as Callback;
            callback?.Invoke();
        }
    }

    public static void Broadcast<T>(EventType eventType, T t)
    {
        if (EventTable.TryGetValue(eventType, out Delegate d))
        {
            Callback<T> callback = d as Callback<T>;
            callback?.Invoke(t);
        }
    }

    public static void Broadcast<T0, T1>(EventType eventType, T0 t, T1 e) where T0 : Component where T1 : EventArgs
    {
        if (EventTable.TryGetValue(eventType, out Delegate d))
        {
            Callback<T0, T1> callback = d as Callback<T0, T1>;
            callback?.Invoke(t, e);
        }
    }

    public static void Broadcast<T0, T1, T2>(EventType eventType, T0 t, T1 e, T2 l) where T0 : Component where T1 : EventArgs where T2 : class
    {
        if (EventTable.TryGetValue(eventType, out Delegate d))
        {
            Callback<T0, T1, T2> callback = d as Callback<T0, T1, T2>;
            callback?.Invoke(t, e, l);
        }
    }

    public static void Broadcast<T0, T1, T2, T3>(EventType eventType, T0 t, T1 e, T2 l, T3 w) where T0 : Component where T1 : EventArgs where T2 : class where T3 : class
    {
        if (EventTable.TryGetValue(eventType, out Delegate d))
        {
            Callback<T0, T1, T2, T3> callback = d as Callback<T0, T1, T2, T3>;
            callback?.Invoke(t, e, l, w);
        }
    }
}

public enum EventType
{
    Event_EnemyDeath,
    Event_PlayerDeath,
    Event_PlayerHpDown,
}
