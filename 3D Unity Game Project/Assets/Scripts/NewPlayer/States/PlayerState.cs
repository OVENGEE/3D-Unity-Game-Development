using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
    }


    public virtual void EnterState() { firstSetup(); }
    public virtual void ExitState() { CleanUp(); }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { }

    public virtual void firstSetup()
    {
        player.OnEnable();
    }

    public virtual void CleanUp()
    {
        player.OnDisable();
    }


}
    
