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


    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
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
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CancelQuitMenu()
    {
        quitMenu.SetActive(false);
    }


}
