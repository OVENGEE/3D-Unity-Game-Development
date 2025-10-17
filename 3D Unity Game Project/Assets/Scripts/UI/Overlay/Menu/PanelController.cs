using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanelController : MonoBehaviour
{
    [SerializeField] panelLibrary[] panelLibraries;

    //Input variables
    CustomInputSystem inputs;
    InputAction Quit;
    InputAction Menu;

    private void Awake()
    {
        if (inputs == null)
        {
            inputs = new CustomInputSystem();
            Quit = inputs?.UI.Quit;
            Menu = inputs?.UI.Menu;
        }
    }


    void OnEnable()
    {
        // Input actions enabling
        inputs?.Enable();
        Quit?.Enable();
        Menu?.Enable();

        //Action subscriptions
        Quit.performed += ActiveInputPanelManager;
        Menu.performed += ActiveInputPanelManager;
    }


    void OnDisable()
    {
        //Action unsubscriptions
        Quit.performed -= ActiveInputPanelManager;
        Menu.performed -= ActiveInputPanelManager; 

        //Input Actions disabling
        Quit?.Disable();
        Menu?.Disable();
        inputs?.Disable();
       
    }

    private void ActiveInputPanelManager(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        string uiInputCall = context.action.name;

        foreach(panelLibrary panel in panelLibraries)
        {
            string currentPanelName = panel.panelName.ToString();
            if (currentPanelName == uiInputCall)
            {
                panel.panelObject?.SetActive(true);
            }
            else
            {
                panel.panelObject?.SetActive(false);
            }
        }

        
    }
    
    
}

public enum PanelType
{
    Tutorial,
    PlayerHUD,
    Menu,
    Quit,
    DuckShootingGame,
    BasketBallGame
}

[System.Serializable]
public struct panelLibrary
{
    public PanelType panelName;
    public GameObject panelObject;
}
