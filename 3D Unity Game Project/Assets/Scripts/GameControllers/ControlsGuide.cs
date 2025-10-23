using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsGuide : MonoBehaviour
{

    //Control declaration
    [SerializeField] Controls[] controls;
    private Dictionary<ControlType, Controls> controlDictionary;
    private InputDeviceDectector.DeviceType controlDevice = InputDeviceDectector.DeviceType.Keyboard;
    private bool controlVisibilty = false;

    //Raycast declaration
    [SerializeField] float range = 5f;
    private Camera maincamera;


    //Event declaration
    public static event Action<Sprite> OnControlImageChange;
    void Awake()
    {
        maincamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        InitializeDictionary();
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(maincamera.transform.position, maincamera.transform.forward);
        controlVisibilty = false;

        //Any object (that needs to display) interactables or controls are layer indices 7 and 9 respectively
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.TryGetComponent<ControlData>(out ControlData controlData))
            {
                controlVisibilty = true;
                OnControlImageChange?.Invoke(GetControlSprite(controlData.controlType));
            }
        }

        if(!controlVisibilty)
        {
            OnControlImageChange?.Invoke(null);
        }
    }

    private void InitializeDictionary()
    {
        controlDictionary = new Dictionary<ControlType, Controls>();
        foreach (var control in controls)
        {
            controlDictionary[control.controlName] = control;
        }
    }


    public Sprite GetControlSprite(ControlType name)
    {
        if (controlDictionary.ContainsKey(name) && controlVisibilty)
        {
            var control = controlDictionary[name];
            switch (controlDevice)
            {
                case InputDeviceDectector.DeviceType.Keyboard:
                    return control.keyboardSprite;
                case InputDeviceDectector.DeviceType.Xbox:
                    return control.xboxSprite;
                case InputDeviceDectector.DeviceType.PlayStation:
                    return control.playStationSprite;
                
            }
        }

        return null;
    }


    InputDeviceDectector.DeviceType UpdateControlDevice(InputDeviceDectector.DeviceType deviceType)
    {
        controlDevice = deviceType;
        return deviceType;
    }

    void OnEnable()
    {
        InputDeviceDectector.OnDeviceChange += UpdateControlDevice;
    }

    void OnDisable()
    {
        InputDeviceDectector.OnDeviceChange -= UpdateControlDevice;
    }
}


[System.Serializable]
struct Controls
{
    public ControlType controlName;
    public Sprite keyboardSprite;
    public Sprite playStationSprite;
    public Sprite xboxSprite;
}


