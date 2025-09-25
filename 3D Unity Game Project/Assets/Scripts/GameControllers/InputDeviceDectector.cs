using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceDectector : MonoBehaviour
{
    private InputAction input;
    public DeviceType currentDevice;
    public static event Func<DeviceType,DeviceType> OnDeviceChange;

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
        string deviceName = inputDevice.displayName.ToLower();
        currentDevice = DeviceType.Keyboard;

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
