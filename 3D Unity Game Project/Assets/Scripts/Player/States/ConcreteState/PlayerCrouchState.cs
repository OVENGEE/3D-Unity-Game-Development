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
        Debug.Log("left crouch state!");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (!crouch.IsPressed())
        {
            playerStateMachine.SwitchState(new PlayerWalkState(player, playerStateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
