
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovement : MonoBehaviour
{
    InputSystem_Actions inputs;
    InputAction lookAction;


    //Mouse settings
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;


    void Awake()
    {
        if (inputs == null)
        {
            inputs = new InputSystem_Actions();
            Debug.Log("new instance of the input system assigned!");
            if (lookAction == null)
            {
                Debug.Log("look action assigned!");
                lookAction = inputs.Player.Look;
            }
        }

        //Locking the cursor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        inputs.Enable();
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

//Code references:
// The mouse movement and look mechanics for the player logic:https://www.youtube.com/watch?v=Nxg0vQk05os&list=PLtLToKUhgzwnk4U2eQYridNnObc2gqWo-

