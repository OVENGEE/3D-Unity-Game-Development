using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintState : PlayerWalkState
{
    //Inputs
    InputAction sprintAction;

    //Sprint variables
    float sprintDuration = 2f;
    float sprintTimer = 0f;

    public PlayerSprintState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        sprintTimer = 0f;

        Debug.Log("Entered Sprint State!");

        if (sprintAction == null)
        {
            sprintAction = base.player.inputs.Player.Sprint;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        base.controller.Move(base.move * -base.player.MoveSpeed * Time.deltaTime);
        Debug.Log("Left Sprint State!");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        sprintTimer += Time.deltaTime;

        //if Sprint button released or duration exceeded, exit sprint 
        if (!sprintAction.IsPressed() || sprintTimer >= sprintDuration)
        {
            base.player.StartSprintCooldown();
            base.playerStateMachine.SwitchState(new PlayerWalkState(player, playerStateMachine));
            return;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //Sprint movement logic
        base.controller.Move(base.move * base.player.MoveSpeed * Time.deltaTime);
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

}

// The sprint mechanic logic comes from the dash logic from this video:
// https://www.youtube.com/watch?v=721TkkJ-CNM&t=8s

// After realizing that the controller move is additive if I have one script inheriting from a script which uses the move and move the charater controller again 
// Therefore in the exit state I must subtract the additional movement I added;
