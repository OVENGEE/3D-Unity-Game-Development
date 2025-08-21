using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class QuitApplication : MonoBehaviour
{
    [SerializeField] GameObject quitMenu;

    CustomInputSystem inputActions;
    InputAction quitAction;


    void Awake()
    {
        if (quitMenu == null)
        {
            Debug.LogError("quitMenu not assigned in inspector of QuitApplication!");
            return;
        }

        if (inputActions == null)
        {
            inputActions = new CustomInputSystem();
            if (quitAction == null)
            {
                quitAction = inputActions.UI.Escape;
            }
        }
    }


    void OnEnable()
    {
        inputActions.UI.Enable();
        quitAction.performed += OnQuitTrigger;
        Debug.Log("Escape button pressed!");
    }

    void OnDisable()
    {
        quitAction.performed -= OnQuitTrigger;
        inputActions.UI.Disable();
    }

    private void OnQuitTrigger(InputAction.CallbackContext context)
    {
        quitMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void CancelQuitMenu()
    {
        quitMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}

// Code references:
// 1)Title: Cursor visibility and lockstates
//  Author: Chatpgt
//  Date accessed:  17/08/2025
//  Availability: https://chatgpt.com/c/68a1baed-8408-8330-b836-79bdfe824920

// Chatgpt was used to help me understand and debug why I could not interact with my button even tho I was clicking it
// The issue was when I clicked the cursor would become invisible and return to the middle of the screent instead of interacting with the button!
