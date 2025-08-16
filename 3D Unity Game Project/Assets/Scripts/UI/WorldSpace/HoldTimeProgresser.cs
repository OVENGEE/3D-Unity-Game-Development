using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoldTimeProgresser : MonoBehaviour
{
    [Header("Input reference")]
    [SerializeField] InputActionReference interact;


    [Header("UI variables")]
    public float ResetDelay = 0.3f;

    //Input interactions variables
    private float _holdDuration = 0.4f;


    //Slider reference
    private Slider _slider;


    //Private flags
    private bool _isHolding;
    private float _holdTime = 0f;
    private float _currentprogress = 0f;

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

    private void Awake()
    {
        if (interact == null)
        {
            Debug.Log("The inputAction reference is null");
            return;
        }

        if (_slider == null)
        {
            _slider = GetComponent<Slider>();
        }
    }


    void Update()
    {
        if (_isHolding)
        {
            //Calculate the progress of slider
            _currentprogress = Mathf.Clamp01((Time.time - _holdTime) / _holdDuration); // a value between 0 - 1
            _slider.value = _currentprogress; //sets the current progress to the slider value
        }
    }

    //Event Handlers

    private void OnHoldStarted(InputAction.CallbackContext context)
    {
        //Start event handler
        _isHolding = true;
        _holdTime = Time.time;

        //call reset progress after a time lapse
        Invoke("ReleaseInteractHold", ResetDelay);
    }

    private void OnHoldPerformed(InputAction.CallbackContext context)
    {
        //Performed event handler
        //Complete the slider
        _slider.value = 1f;
        Debug.Log("Interaction performed!");
    }

    private void OnHoldCanceled(InputAction.CallbackContext context)
    {
        //canceled event handler
        Debug.Log("Interaction canceled!");

        //call reset progress after a time lapse
        Invoke("ReleaseInteractHold", ResetDelay);
    }

    private void ReleaseInteractHold()
    {
        _isHolding = false;
        _slider.value = 0f;
        _currentprogress = 0f;
    }

    
}

//  Code references:
// 1) Title:C# Invoke in Unity!
//    Author:Unity
//    Date: 16/08/2025
//    Availiability: https://www.youtube.com/watch?v=-YgM4DXGeq4

// 2)Title: Creating a Visual Hold Progress Slider in Unity
// Author: deepseek AI
// Date: 15/08/2025
//Availiability: https://chat.deepseek.com/a/chat/s/00bc1f33-a21c-4a3c-bfd5-47b65bc469de

// This AI suggestion helped me understand and visualize the code works for a circular slider specifically the hold interaction
// AI made use of invokes which i wanted to implement and understand furthur.

// Ask for advice to make it better!
