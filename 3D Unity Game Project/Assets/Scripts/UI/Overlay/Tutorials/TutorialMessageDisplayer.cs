using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialMessageDisplayer : MonoBehaviour
{
    //Input variables
    CustomInputSystem input;
    private PlayerInput playerInput;

    private PlayerInput currentInput;



    //Dictionary
    private Dictionary<TutorialType, List<InputAction>> tutorialInputMap;


    //Event declaration
    public static event Action<string> OnTutorialUIUpdate;


    private void Awake()
    {
        input = new CustomInputSystem();
        playerInput = FindFirstObjectByType<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput not found");
        }

        // Build the mapping between TutorialType and InputAction
        tutorialInputMap = new Dictionary<TutorialType, List<InputAction>>()
        {
        { TutorialType.Jump,   new List<InputAction> { input.Player.Jump } },
        { TutorialType.Sprint, new List<InputAction> { input.Player.Sprint } },
        { TutorialType.Crouch, new List<InputAction> { input.Player.Crouch } },
        { TutorialType.ShootingTut, new List<InputAction> { input.Player.Shoot } },
        { TutorialType.BasketBallTut, new List<InputAction> { input.Player.Aim, input.Player.PickUp, input.Player.Throw} },

        };

    }

    private void OnEnable()
    {
        playerInput.onControlsChanged += UpdateTutorialUI;
        TutorialID.OnTutorialTypeTrigger += UpdateTutorialUI;
    }


    private void OnDisable()
    {
        playerInput.onControlsChanged -= UpdateTutorialUI;
        TutorialID.OnTutorialTypeTrigger -= UpdateTutorialUI;
    }

        // Overloaded helper: default when only control scheme changes
    private void UpdateTutorialUI(PlayerInput input)
    {
        // If you want to update all displayed hints when the player swaps devices
        Debug.Log($"Control scheme changed to: {input.currentControlScheme}");
        // currentInput = input;
    }

    // Main logic for a specific tutorial
    private void UpdateTutorialUI(TutorialType tutorialType)
    {
        
        if (!tutorialInputMap.TryGetValue(tutorialType, out var action))
        {
            Debug.LogWarning($"No action found for tutorial type: {tutorialType}");
            return;
        }

        string scheme = playerInput.currentControlScheme;
        string bindingText = GetBindingDisplayName(action, scheme);
        Debug.Log(bindingText);
        OnTutorialUIUpdate?.Invoke(bindingText);
    }


    private string GetBindingDisplayName(List<InputAction> actions, string scheme)
    {
        string actionresult= "";
        foreach (var action in actions)
        {
            foreach (var binding in action.bindings)
            {
                // Split groups safely (they might be "Keyboard&Mouse;Gamepad")
                var groups = (binding.groups ?? "").Split(';');
                foreach (var group in groups)
                {
                    if (group.Trim().Equals(scheme, System.StringComparison.OrdinalIgnoreCase))
                    {
                        actionresult = actionresult + " Press [" + InputControlPath.ToHumanReadableString(
                           binding.effectivePath,
                           InputControlPath.HumanReadableStringOptions.OmitDevice
                       ) + "] to " + action.name + ".";
                    }
                }
            }
        }

        Debug.Log($"actionresult is :{actionresult} ");
        string trueResult = (actionresult == "") ? "???" : actionresult;
        return trueResult;
    }

}

// Code References:
// Title: New input system binding groups
// Author: ChatGPT
// url: https://chatgpt.com/c/68fcd4e7-be48-8330-93df-4b9ea779dc91
// date accessed: 2025/10/25

// I really struggled conceptualizing how the new input system works with binding. So chatgpt explained how I could get an individual binding from the active device from the binding groups
