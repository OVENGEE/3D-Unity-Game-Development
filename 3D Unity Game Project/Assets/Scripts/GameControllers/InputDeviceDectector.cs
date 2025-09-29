using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceDectector : MonoBehaviour
{
    private InputAction input;
    public DeviceType currentDevice;

    public static Func<DeviceType, DeviceType> OnDeviceChange;
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
        currentDevice = DeviceType.Keyboard;
        string deviceName = inputDevice.displayName.ToLower();

        if (inputDevice is Gamepad)
        {
            if (deviceName.Contains("xbox") || deviceName.Contains("xinput"))
            {
                currentDevice = DeviceType.Xbox;
            }
            else if (deviceName.Contains("playstation") || deviceName.Contains("dualshock"))
            {
                currentDevice = DeviceType.PlayStation;
            }

        }
    }

    public enum DeviceType
    {
        Keyboard,
        Xbox, 
        PlayStation,
    };

}



