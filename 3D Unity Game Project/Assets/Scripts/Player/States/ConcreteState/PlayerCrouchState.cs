using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouchState : PlayerWalkState
{
    //InputActions
    InputAction crouch;


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
        Debug.Log("In crouch state!");

        if (crouch == null)
        {
            crouch = base.player.inputs.Player.Crouch;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        base.controller.height = 2f;
        Debug.Log("left crouch state!");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        if (!crouch.IsPressed())
        {
            //Transition back to walking state if not crouching
            playerStateMachine.SwitchState(new PlayerWalkState(player, playerStateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        HandleCrouch();
    }

    public void HandleCrouch()
    {
        float crouchHeight = 1f;
        base.controller.height = crouchHeight;
        float crouchSpeed = -0.5f * (base.player.MoveSpeed);//Crouch movement speed

        base.controller.Move(base.move * crouchSpeed * Time.deltaTime);
    }
}
