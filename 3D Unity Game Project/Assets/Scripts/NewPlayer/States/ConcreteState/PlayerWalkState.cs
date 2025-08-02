using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }
}
