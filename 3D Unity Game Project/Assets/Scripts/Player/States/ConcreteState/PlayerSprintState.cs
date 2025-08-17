using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintState : PlayerWalkState
{
    //Inputs
    InputAction sprintAction;

    //Sprint variables
    float staminaTimer, MaxStamina,ChargeRate;

    //FOV variables
    float sprintFOV = 90f;

    public PlayerSprintState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        base.player.stateText.text = "Sprint";

        //Assigning from monobehaviour class
        MaxStamina = base.player.MaxStamina;
        ChargeRate = base.player.ChargeRate;
        staminaTimer = MaxStamina;

        if (sprintAction == null)
        {
            // assigning action if null
            sprintAction = base.player.inputs.Player.Sprint;
        }

        //Event subscription
        sprintAction.canceled += OnSprintReleased;
    }

    public override void ExitState()
    {
        base.ExitState();

        base.FOVTransition(60f);
        base.controller.Move(Vector3.zero);
        Debug.Log("Left Sprint State!");

        // Only one coroutine
        if (base.player.recharge != null) base.player.StopCoroutine(base.player.recharge);
        base.player.recharge = base.player.StartCoroutine(StaminaRecover(staminaTimer));
        
        //Event unsubscription
        sprintAction.canceled -= OnSprintReleased;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        staminaTimer -= Time.deltaTime;
        base.player.StaminaSlider.value = staminaTimer / MaxStamina;
        
        //if Sprint button released or duration exceeded, exit sprint 
        if (staminaTimer <= 0f)
        {
            base.playerStateMachine.SwitchState(new PlayerWalkState(player, playerStateMachine));
            return;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //Sprint movement logic
        base.controller.Move(base.move * base.player.MoveSpeed * Time.deltaTime);

        //change the fov
        base.FOVTransition(sprintFOV);
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    public IEnumerator StaminaRecover   (float currentStamina)
    {
        yield return new WaitForSeconds(1f);

        staminaTimer = currentStamina;
        while (staminaTimer < MaxStamina)
        {
            staminaTimer += ChargeRate * 0.05f* Time.deltaTime;
            if (staminaTimer > MaxStamina) staminaTimer = MaxStamina;
            base.player.StaminaSlider.value = staminaTimer / MaxStamina;
            yield return null;
        }
    }

    private void OnSprintReleased(InputAction.CallbackContext context)
    {
        base.playerStateMachine.SwitchState(new PlayerWalkState(player, playerStateMachine));
    }


}
// Code references:
// 1)Title: A Better Way to Code Your Characters in Unity | Finite State Machine | Tutorial
//  Author: Sasquatch B Studios
//  Date accessed:  3/08/2025
//  Availability: https://www.youtube.com/watch?v=RQd44qSaqww&ab_channel=SasquatchBStudios

// The sprint mechanic logic comes from the dash logic comes from these 2 referecences

// 2)Title: BEST WAY to Dash through Enemies - 2D Platformer Unity #19
//  Author: Game Code Library
//  Date accessed:  3/08/2025
//  Availability: https://www.youtube.com/watch?v=721TkkJ-CNM&t=8s

// 3)Title: Stamina Bar in Unity Tutorial
//  Author: Gatsby
//  Date accessed:  5/08/2025
//  Availability: https://www.youtube.com/watch?v=ju1dfCpDoF8

// After realizing that the controller move is additive if I have one script inheriting from a script which uses the move and move the charater controller again 
// Therefore in the exit state I must subtract the additional movement I added;

// 3)Title: Sprint state code Issues
//  Author: Chatgpt
//  Date accessed:  17/08/2025
//  Availability: https://chatgpt.com/c/68a1f68e-ee4c-8325-b1ef-620909d09d33
// Chatgpt helped debug my logic as my recovery coroutine was not very smooth;
