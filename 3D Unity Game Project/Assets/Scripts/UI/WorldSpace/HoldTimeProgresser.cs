using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoldTimeProgresser : MonoBehaviour
{
    [Header("Input reference")]
    [SerializeField] InputActionReference interact;

    [Header("UI reference")]
    public Color HoldConfirmedcolor = Color.green;
    public Color HoldCanceledcolor = Color.red;
    public Color HoldStartedcolor = Color.yellow;

    private void OnEnable()
    {
        // enable events and subscriptions
        interact.action.Enable();
        interact.action.started += OnHoldStarted;
        interact.action.performed += OnHoldPerformed;
        interact.action.canceled += OnHoldCanceled;
    }

    private void OnDisable()
    {
        //unsubscribed events and disable input reference
        interact.action.started -= OnHoldStarted;
        interact.action.performed -= OnHoldPerformed;
        interact.action.canceled -= OnHoldCanceled;
        interact.action.Disable();
    }


    void Update()
    {

    }

    //Event Handlers

    private void OnHoldStarted(InputAction.CallbackContext context)
    {
        //Start event handler

    }

    private void OnHoldPerformed(InputAction.CallbackContext context)
    {
        //Performed event handler
    }

    private void OnHoldCanceled(InputAction.CallbackContext context)
    {
        //canceled event handler
    }
}
