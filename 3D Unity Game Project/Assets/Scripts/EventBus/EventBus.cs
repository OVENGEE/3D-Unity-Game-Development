using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public static class EventBus
{
    public static readonly Dictionary<Type, Delegate> subscribers = new Dictionary<Type, Delegate>();

    public static void Subscribe<T>(Action<T> action)
    {

        var type = typeof(T);//receives the type/class of T


        //Checks if there is a subscribers linked to the eventkey
        // if there is one then we add another to the list using multicast otherwise assign the first subscriber
        if (subscribers.TryGetValue(type, out Delegate existing))
        {
            subscribers[type] = Delegate.Combine(action);
        }
        else subscribers[type] = action;

    }

    //removing a specific delegate from the dictionary
    public static void UnSubscribe<T>(Action<T> action)
    {
        var type = typeof(T);

        if (!subscribers.TryGetValue(type, out Delegate existing)) return;

        Delegate newDelegateList = Delegate.Remove(existing, action);
        if (newDelegateList == null)
        {
            //remove the event key
            subscribers.Remove(type);
        }
        else subscribers[type] = newDelegateList;
    }

    public static void Publish<T>(T eventData)
    {
        var type = typeof(T);
        if (subscribers.TryGetValue(type, out var existing))
        {
            var action = existing as Action<T>;
            action?.Invoke(eventData);
        }
    }
}


// Code Reference:
// Title: Delegates Class
//Author: Microsoft Ignite
//Date: 2025/09/27
//Code Version: 9.0
//Availability: https://learn.microsoft.com/en-us/dotnet/api/system.delegate?view=net-9.0

// This gave me the understanding of the class Delegates to make use of multicasting.

// Title: Simple Event Bus
//Author: Chatgpt-5
//Date: 2025/09/27
//Code Version: 
//Availability: https://chatgpt.com/c/68d6f0f6-7a40-8329-b012-ec7237b27252

// This helped me with the understanding of how I could be more efficient with the events to have multiple things be notified by one thing
// I am trying to avoid a dependance for AI but I think Event buses are an advanced design pattern which I needed guidance for.


