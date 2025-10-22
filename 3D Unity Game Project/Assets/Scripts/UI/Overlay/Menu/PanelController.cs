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


    //Panel variables
    int panelIndex;

    private void Awake()
    {
        if (inputs == null)
        {
            inputs = new CustomInputSystem();
            Quit = inputs?.UI.Quit;
            Menu = inputs?.UI.Menu;
        }

        InitialVisibilityState();
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
        panelIndex = 0;

        foreach (panelLibrary panel in panelLibraries)
        {

            string currentPanelName = panel.panelName.ToString();
            if (currentPanelName == uiInputCall)
            {
                bool newState = !panel.panelObject.activeSelf;
                Debug.Log($"{panel.panelName.ToString()} Panel: Toggle state = {newState}");
                panel.panelObject?.SetActive(newState);
                ResetToHUDPanel(newState);
            }
            else
            {
                panel.panelObject?.SetActive(false);

            }
        }
        
        //Update visibility state
        for(panelIndex =0; panelIndex < panelLibraries.Length; panelIndex++)
        {
            panelLibraries[panelIndex].visibilityState = panelLibraries[panelIndex].panelObject.activeSelf;
            panelIndex++;
        }
    }

    private void ResetToHUDPanel(bool state)
    {
        if (state) return;
        panelIndex = 0;
        foreach (var panel in panelLibraries)
        {
            if (panel.panelName == PanelType.Tutorial || panel.panelName == PanelType.PlayerHUD)
            {
                panel.panelObject.SetActive(true);
            }
        }
        
        //Update visibility state
        for(panelIndex =0; panelIndex < panelLibraries.Length; panelIndex++)
        {
            panelLibraries[panelIndex].visibilityState = panelLibraries[panelIndex].panelObject.activeSelf;
        }
    }
    

    
    private void InitialVisibilityState()
    {
        for(int i = 0; i < panelLibraries.Length; i++)
        {
            //The initial visible states of each panel
            panelLibraries[i].visibilityState = panelLibraries[i].panelObject.activeSelf;
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
    public bool visibilityState;
}
