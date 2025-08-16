using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    //Input 
    CustomInputSystem inputs;
    InputAction interact;

    private void Awake()
    {
        NullChecks();
    }

    private void OnEnable()
    {
        //Enable input and subscribe events
        inputs.Enable();
        interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        //unsubscribe events and disable input
        interact.performed -= OnInteract;
        inputs.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact");
    }

    void NullChecks()
    {
        //Checks if input null assign new instance
        if (inputs == null)
        {
            inputs = new CustomInputSystem();

            if (interact == null)
            {
                interact = inputs.Player.Interact;
            }
        }
    }
}
