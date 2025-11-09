using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouchState : PlayerWalkState
{
    //InputActions
    InputAction crouch;

    //Camera settings


    //Crouch variables
    float crouchFOV = 50f;

    public PlayerCrouchState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.PlayerState currentState = base.player.playerState;
        currentState = Player.PlayerState.Crouch;
        base.player.UpdateState(currentState);

        base.FOVTransition(crouchFOV); //change to crouch FOV
        if (crouch == null)
        {
            crouch = base.player.inputs.Player.Crouch;
        }

        //Events subscriptions
        crouch.canceled += OnUnCrouch;
    }

    public override void ExitState()
    {
        base.ExitState();
        base.controller.height = 3.1f; //Return back to standing height
        base.FOVTransition(60f); //return to base FOV
        //Events unsubscriptions
        crouch.canceled -= OnUnCrouch;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        HandleCrouch();
    }

    public void HandleCrouch()
    {
        float crouchHeight = 1f;
        base.controller.height = crouchHeight; //set crouch height
        float crouchSpeed = -0.5f * (base.player.MoveSpeed);//Crouch movement speed

        base.controller.Move(base.move * crouchSpeed * Time.deltaTime);
    }

    //Event Handlers

    private void OnUnCrouch(InputAction.CallbackContext context)
    {
        //Transition back to walking state if not crouching
        playerStateMachine.SwitchState(new PlayerWalkState(player, playerStateMachine));
    }
}
// Code references:
// 2)Title: A Better Way to Code Your Characters in Unity | Finite State Machine | Tutorial
//  Author: Sasquatch B Studios
//  Date accessed:  17/08/2025
//  Availability: https://www.youtube.com/watch?v=RQd44qSaqww&ab_channel=SasquatchBStudios