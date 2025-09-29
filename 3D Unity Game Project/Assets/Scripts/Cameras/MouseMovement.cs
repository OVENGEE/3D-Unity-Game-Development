
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MouseMovement : MonoBehaviour
{
    CustomInputSystem inputs;
    InputAction lookAction;

    //Pointer info
    [SerializeField] GameObject selectedObject;


    //Mouse settings
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;


    void Awake()
    {
        if (inputs == null)
        {
            inputs = new CustomInputSystem();
            Debug.Log("new instance of the input system assigned!");
            if (lookAction == null)
            {
                lookAction = inputs.Player.Look;
            }
        }

        if (selectedObject == null)
        {
            Debug.Log("SelectedObject has not been assigned in the inspector!");
            return;
        }

        //Locking the cursor to the middle of the screen 
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        inputs.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the pointer is on a UI element if so make it appear!
        if (EventSystem.current.IsPointerOverGameObject() && selectedObject.tag == "Quit")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }




        //Mouse input directions
        float mouseX = lookAction.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookAction.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;

        //Control rotation around x axis ( look up and down)
        xRotation -= mouseY;

        //clamp the rotation so we cant over-rotation (like in real life)
        xRotation = Mathf.Clamp(xRotation, -45f, 90f);

        //Control rotation around y axis ( look up and down)
        yRotation += mouseX;

        //applying both rotations
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void OnDisable()
    {
        inputs.Disable();
        Cursor.visible = true;
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

