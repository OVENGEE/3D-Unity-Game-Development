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
    [SerializeField] private GameObject interactSlider;


    //Raycast variables
    RaycastHit hit;
    Ray ray;


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
        //if cannot interact then exit function!
        if (!_canInteract) return;

        //Checks if the interactable object contains this script!
        if (!hit.transform.TryGetComponent(out IInteractable interactable)) return;
        interactable.Interact();
        Debug.Log("Interacted!");
    }

    private void OnViewInteractable(InputAction.CallbackContext context)
    {
        ray = new Ray(InteractCamera.transform.position, InteractCamera.transform.forward);

        //checks if raycast has hit any object with the interact layer
        if (!Physics.Raycast(ray, out hit, InteractRange, interactLayer))
        {
            _canInteract = false;
            interactSlider.SetActive(_canInteract);
        }
        else
        {
            _canInteract = true;
            interactSlider.SetActive(_canInteract);
        }
        
    }

    

    void NullChecks()
    {

        if (interactSlider == null)
        {
            Debug.Log("the slider has not been assigned inspector! Interaction script");
            return;
        }

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
