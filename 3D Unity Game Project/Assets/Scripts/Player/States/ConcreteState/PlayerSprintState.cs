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

    //FOV variables
    float baseFOV = 0f;
    float sprintFOV = 90f;

    public PlayerSprintState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        sprintTimer = 0f;
        baseFOV = base.player.camera.fieldOfView;
        base.player.stateText.text = "Sprinting";

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

        base.FOVTransition(baseFOV);
        base.controller.Move(base.move * -base.player.MoveSpeed * Time.deltaTime);
        Debug.Log("Left Sprint State!");

        //Event unsubscription
        sprintAction.canceled += OnSprintReleased;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        sprintTimer += Time.deltaTime;

        //if Sprint button released or duration exceeded, exit sprint 
        if (sprintTimer >= sprintDuration)
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

        //change the fov
        base.FOVTransition(sprintFOV);
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }


    private void OnSprintReleased(InputAction.CallbackContext context)
    {
        base.player.StartSprintCooldown();
        base.playerStateMachine.SwitchState(new PlayerWalkState(player, playerStateMachine));
    }


}

// The sprint mechanic logic comes from the dash logic from this video:
// https://www.youtube.com/watch?v=721TkkJ-CNM&t=8s

// After realizing that the controller move is additive if I have one script inheriting from a script which uses the move and move the charater controller again 
// Therefore in the exit state I must subtract the additional movement I added;
