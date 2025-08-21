using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Interaction Event")]
    [SerializeField] private UnityEvent _onInteract;
    UnityEvent IInteractable.onInteract
    {
        get => _onInteract;
        set => _onInteract = value;
    }

    public void Interact() => _onInteract.Invoke();

}

//Code references:
// 1)Title: Interactions with Unity Events - New Input System
//  Author: ErenCode
//  Date accessed:  16/08/2025
//  Availability: https://www.youtube.com/watch?v=ZNiEbRL85Vc

// This helped me with the the logic for interactions!