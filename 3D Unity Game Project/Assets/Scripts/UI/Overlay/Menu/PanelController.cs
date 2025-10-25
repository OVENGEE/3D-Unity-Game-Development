using System;
using System.Collections.Generic;
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

    //Timing variables
    private float unavailablePanelCooldown = 0.2f;
    private float timeSinceLastHit;

    //Panel dictionary
    private Dictionary<PanelType, GameObject> panelMap;

    //Camera 
    private Camera camera;

    //Events
    public static event Action OnUnAvailableGame;


    //Layer Setup
    int unavailableLayerMask;
    const int UnAvailableGameLayer = 8;

    private void Awake()
    {
        camera = Camera.main;
        unavailableLayerMask = 1 << UnAvailableGameLayer;
        timeSinceLastHit = Time.time;

        if (inputs == null)
        {
            inputs = new CustomInputSystem();
            Quit = inputs?.UI.Quit;
            Menu = inputs?.UI.Menu;
        }

        //Build the dictionary for quick access
        panelMap = new Dictionary<PanelType, GameObject>();
        foreach (var panel in panelLibraries)
        {
            if (!panelMap.ContainsKey(panel.panelName))
                panelMap.Add(panel.panelName, panel.panelObject);
        }
        
        for(int i =0; i < panelLibraries.Length; i++)
        {
            panelLibraries[i].visibilityState = panelLibraries[i].panelObject.activeSelf;
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

        if (Enum.TryParse(uiInputCall, out PanelType panelType))
        {
            bool isActive = panelMap.ContainsKey(panelType) && panelMap[panelType].activeSelf;
            bool newState = !isActive;

            Debug.Log($"{panelType} Panel: Toggle state = {newState}");

            if (newState)
            {
                SetActivePanel(panelType);
            }
            else
            {
                ResetToHUDPanel();
            }
        }
        else
        {
            Debug.LogWarning($"No panel found matching input action '{uiInputCall}'.");
        }
    }

    private void Update()
    {
        UnAvailableGamePanelTrigger();
    }

    private void ResetToHUDPanel()
    {
        foreach (var pair in panelMap)
        {
            bool shouldBeActive = (pair.Key == PanelType.PlayerHUD || pair.Key == PanelType.Tutorial);
            pair.Value.SetActive(shouldBeActive);
        }

        UpdateVisibilityState();
    }



    private void SetActivePanel(PanelType targetPanel)
    {
        foreach (var pair in panelMap)
        {
            pair.Value.SetActive(pair.Key == targetPanel);
        }



        UpdateVisibilityState();
    }

    private void UpdateVisibilityState()
    {
        for(int i =0; i < panelLibraries.Length; i++)
        {
            if (panelMap.TryGetValue(panelLibraries[i].panelName, out var obj))
                panelLibraries[i].visibilityState = obj.activeSelf;
        }
    }

    private void UnAvailableGamePanelTrigger()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 5f, unavailableLayerMask))
        {
            timeSinceLastHit = Time.time;
            foreach (var panelbook in panelLibraries)
            {
                if (panelbook.panelName == PanelType.UnAvailableGame)
                {          
                    OnUnAvailableGame?.Invoke();//Invoke the event if the UnAvailableGamePanel on
                    panelbook.panelObject.SetActive(true);
                }
                else
                    panelbook.panelObject.SetActive(false);
            }
        }
        else if (Time.time - timeSinceLastHit > unavailablePanelCooldown)
        {
            ResetToHUDPanel();
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
