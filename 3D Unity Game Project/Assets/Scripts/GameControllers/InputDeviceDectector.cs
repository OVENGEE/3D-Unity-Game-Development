using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceDectector : MonoBehaviour
{
    private InputAction input;
    public DeviceType currentDevice;
    void Awake()
    {
        input = new InputAction(binding: "/*/<button>");
        input.performed += OnInputPressed;
        currentDevice = DeviceType.Keyboard;
    }

    void OnEnable()
    {
        input.Enable();
    }


    void OnDisable()
    {
        input.Disable();
    }
    void OnInputPressed(InputAction.CallbackContext context)
    {
        InputDevice inputDevice = context.control.device;

        if (inputDevice is Keyboard)
        {
            currentDevice = DeviceType.Keyboard;
            Debug.Log("Keyboard was used in input");
            return;
        }

        if (inputDevice is Gamepad)
        {
            currentDevice = DeviceType.Gamepad;
            Debug.Log("Gamepad was used in input");
            return;
        }
        
        if (inputDevice is Joystick)
        {
            currentDevice = DeviceType.JoyStick;
            Debug.Log("Joystick was used in input");
        } 

    }


    public enum DeviceType
    {
        Keyboard,
        Gamepad, 
        JoyStick,
    };
}
