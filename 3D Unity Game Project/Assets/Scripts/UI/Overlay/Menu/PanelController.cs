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
    private bool isManualPanelOpen = false;

    //Camera 
    private Camera camera;

    //Events
    public static event Action OnUnAvailableGame;
    public static event Action<bool> OnEnablePlayerInput;

    


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
        TutorialID.OnTutorialTrigger += SetActivePanel;
        NPC.onDialogueTrigger += SetActivePanel;
        PlayerShootState.OnShootPanelTrigger += SetActivePanel;
        PlayerShootState.OnShootPanelReset += ResetToHUDPanel;
    }


    void OnDisable()
    {
        //Action unsubscriptions
        Quit.performed -= ActiveInputPanelManager;
        Menu.performed -= ActiveInputPanelManager;
        TutorialID.OnTutorialTrigger -= SetActivePanel;
        NPC.onDialogueTrigger -= SetActivePanel;
        PlayerShootState.OnShootPanelTrigger -= SetActivePanel;
        PlayerShootState.OnShootPanelReset -= ResetToHUDPanel;
        
        //Input Actions disabling
        Quit?.Disable();
        Menu?.Disable();
        inputs?.Disable();
       
    }

    private void ActiveInputPanelManager(InputAction.CallbackContext context)
    {
        //Add memory to changing the panel so it remembers the panel it was before input Action
        if (!context.performed) return;
        string uiInputCall = context.action.name;

        // Try to match the input name with a panel type
        if (Enum.TryParse(uiInputCall, out PanelType panelType))
        {
            bool isActive = panelMap.ContainsKey(panelType) && panelMap[panelType].activeSelf;
            bool newState = !isActive;

            Debug.Log($"{panelType} Panel: Toggle state = {newState}");

            if (newState)
            {
                isManualPanelOpen = true;
                SetActivePanel(panelType);
            }
            else
            {
                isManualPanelOpen = false;
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

    public void ResetToHUDPanel()
    {
        isManualPanelOpen = false;
        foreach (var pair in panelMap)
        {
            bool shouldBeActive = (pair.Key == PanelType.PlayerHUD);
            pair.Value.SetActive(shouldBeActive);
        }

        UpdateVisibilityState();
        //Re-enable player input
        OnEnablePlayerInput?.Invoke(true);

        //Cursor lock and make invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }



    private void SetActivePanel(PanelType targetPanel)
    {
        isManualPanelOpen = true;
        foreach (var pair in panelMap)
        {
            bool shouldBeActive = pair.Key == targetPanel;
            Debug.Log($"Target Panel:{targetPanel} == > {shouldBeActive}");
            pair.Value.SetActive(shouldBeActive);

        }
        UpdateVisibilityState();

        //Disable player input
        if (targetPanel == PanelType.Tutorial | targetPanel == PanelType.Dialogue | targetPanel == PanelType.Menu | targetPanel == PanelType.Quit)
        {
            OnEnablePlayerInput?.Invoke(false);

            //Cursor unlock and make visible
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

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
        if (isManualPanelOpen) return;
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
    UnAvailableGame,
    Dialogue
}

[System.Serializable]
public struct panelLibrary
{
    public PanelType panelName;
    public GameObject panelObject;
    public bool visibilityState;
}
