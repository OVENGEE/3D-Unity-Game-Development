using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    public static readonly Dictionary<Type, Delegate> subscribers = new Dictionary<Type, Delegate>();

    public static void Subscribe<T>(Action<T> action)
    {
        
    }
}
