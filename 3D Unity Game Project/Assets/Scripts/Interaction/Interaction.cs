using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    //Input 
    CustomInputSystem inputs;
    InputAction interactAction;
    InputAction lookAction;
    //Camera reference:
    [Header("Camera")]
    [SerializeField] private Camera InteractCamera;

    [Header("Interact settings")]
    [SerializeField] private float InteractRange = 5f;
    [SerializeField] private LayerMask interactLayer;

    //private flags
    private bool _canInteract;

    private void Awake()
    {
        NullChecks();
    }

    private void OnEnable()
    {
        //Enable input and subscribe events
        inputs.Enable();
        interactAction.performed += OnInteract;
        lookAction.started += OnViewInteractable;
        lookAction.performed += OnViewInteractable;
        lookAction.canceled += OnViewInteractable;
    }

    private void OnDisable()
    {
        //unsubscribe events and disable input
        interactAction.performed -= OnInteract;
        lookAction.started -= OnViewInteractable;
        lookAction.performed -= OnViewInteractable;
        lookAction.canceled -= OnViewInteractable;
        inputs.Disable();
    }

    //Event Handlers
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!_canInteract) return;

        Debug.Log("Interacted!");
    }

    private void OnViewInteractable(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        Ray ray = new Ray(InteractCamera.transform.position, InteractCamera.transform.forward);


        //checks if raycast has hit any object with the interact layer
        if (!Physics.Raycast(ray, out hit, InteractRange, interactLayer))
        {
            _canInteract = false;
        }
        else _canInteract = true;
        
    }

    

    void NullChecks()
    {

        if (InteractCamera == null)
        {
            Debug.Log("InteractCamera has not been assigned in the inspector!");
            return;
        }


        //Checks if input null assign new instance
        if (inputs == null)
        {
            inputs = new CustomInputSystem();

            if (interactAction == null)
            {
                interactAction = inputs.Player.Interact;
            }

            if (lookAction == null)
            {
                lookAction = inputs.Player.Look;
            }
                
        }

        
    }
}

//Code references:
// 1)Title: Interactions with Unity Events - New Input System
//  Author: ErenCode
//  Date accessed:  16/08/2025
//  Availability: https://www.youtube.com/watch?v=ZNiEbRL85Vc

// This helped me with the the logic for interactions!
