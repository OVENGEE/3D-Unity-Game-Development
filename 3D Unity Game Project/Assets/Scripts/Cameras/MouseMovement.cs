
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MouseMovement : MonoBehaviour
{
    CustomInputSystem inputs;
    InputAction lookAction;
    

    //Mouse settings
    public float mouseSensitivity = 50f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    //Transform
    [SerializeField] private Transform head; 

    //Camera variables
    private Camera camera;


    void Awake()
    {
        camera = Camera.main;
        inputs = new CustomInputSystem();
        lookAction = inputs?.Player.Look;
    }

    void OnEnable()
    {
        inputs.Enable();
        PanelController.OnEnablePlayerInput += EnablePlayerInput;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse input directions
        float mouseX = lookAction.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookAction.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;

        //Control rotation around x axis ( look up and down)
        xRotation -= mouseY;

        //clamp the rotation so we cant over-rotation (like in real life)
        xRotation = Mathf.Clamp(xRotation, -45f, 68f);

        //Control rotation around y axis ( look up and down)
        yRotation += mouseX;
        //yRotation = Mathf.Clamp(yRotation, 30f, 90f);

        head.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Yaw (left/right) — affects the whole body
        transform.Rotate(Vector3.up * mouseX);
    }

    private void EnablePlayerInput(bool state)
    {
        if (state)
            inputs?.Player.Enable();
        else
            inputs?.Player.Disable();
    }

    void OnDisable()
    {
        PanelController.OnEnablePlayerInput -= EnablePlayerInput;
        inputs.Disable();
    }
}

// 2)Title: EventSystems
//  Author: Chatpgt
//  Date accessed:  16/08/2025
//  Availability: https://chatgpt.com/c/68a1baed-8408-8330-b836-79bdfe824920

// 1)Title: 3D Survival Game Tutorial | Unity | Part 1: Getting Started & Player Movement
//  Author: Mike's Code
//  Date accessed:  1/08/2025
//  Availability: https://www.youtube.com/watch?v=Nxg0vQk05os&list=PLtLToKUhgzwnk4U2eQYridNnObc2gqWo-

