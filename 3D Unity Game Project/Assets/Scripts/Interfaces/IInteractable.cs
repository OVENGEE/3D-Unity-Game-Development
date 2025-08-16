using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    public UnityEvent onInteract { get; protected set; }
    public void Interact();

}

//Code references:
// 1)Title: Interactions with Unity Events - New Input System
//  Author: ErenCode
//  Date accessed:  16/08/2025
//  Availability: https://www.youtube.com/watch?v=ZNiEbRL85Vc

// This helped me with the the logic for interactions!