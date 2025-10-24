using System;
using NUnit.Framework;
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

    //Camera 
    private Camera camera;

    //Layers
    int unavailableLayerMask = 1 << UnAvailableGameLayer;

    //Constants

    const int UnAvailableGameLayer = 8;

    private void Awake()
    {
        camera = Camera.main;

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
        for (panelIndex = 0; panelIndex < panelLibraries.Length; panelIndex++)
        {
            panelLibraries[panelIndex].visibilityState = panelLibraries[panelIndex].panelObject.activeSelf;
            panelIndex++;
        }
    }

    private void Update()
    {
        UnAvailableGamePanelTrigger();
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
        for (int i = 0; i < panelLibraries.Length; i++)
        {
            //The initial visible states of each panel
            panelLibraries[i].visibilityState = panelLibraries[i].panelObject.activeSelf;
        }
    }
    

    private void UnAvailableGamePanelTrigger()
    {
        RaycastHit hit;

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        if (Physics.Raycast(ray, out hit,5f,unavailableLayerMask))
        {
            Debug.Log($"{hit.collider.name} is in the layer: {hit.collider.gameObject.layer}");
            foreach (var panelbook in panelLibraries)
            {
                if (panelbook.panelName == PanelType.UnAvailableGame)
                {
                    panelbook.panelObject.SetActive(true);
                }
                else
                {
                    panelbook.panelObject.SetActive(false);
                }
            }
        }
        else
        {
            ResetToHUDPanel(true);
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
    BasketBallGame,
    UnAvailableGame
}

[System.Serializable]
public struct panelLibrary
{
    public PanelType panelName;
    public GameObject panelObject;
    public bool visibilityState;
}
