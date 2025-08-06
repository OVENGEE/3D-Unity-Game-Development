using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintState : PlayerWalkState
{
    //Inputs
    InputAction sprintAction;

    //Sprint variables
    float sprintSpeed;
    float sprintDuration = 2f;
    float sprintCooldown = 1f;
    bool canSprint = true;
    bool isSprinting = false;


    public PlayerSprintState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered Sprint State!");

        if (sprintAction == null)
        {
            sprintAction = base.player.inputs.Player.Sprint;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Left Sprint State!");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (canSprint && sprintAction.IsPressed())
        {
            //Starts the sprint coroutine
            base.player.StartCoroutine(sprintCoroutine());
        }
        else
        {
            //return to walking state if not running currently
            base.playerStateMachine.SwitchState(new PlayerWalkState(player, playerStateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    private IEnumerator sprintCoroutine()
    {
        //Toggle current state of the 

        yield return null;
    }
}

// The sprint mechanic logic comes from the dash logic from this video:

