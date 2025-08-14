using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootState : PlayerWalkState
{
    //Shoot references
    private float firetimer;
    private float range;
    private Camera camera;
    Ray debugray;
    bool shoot;

    //Input actions
    InputAction shootAction;

    public PlayerShootState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        range = 20f;
        camera = base.player.camera;

        if (shootAction == null)
        {
            shootAction = base.player.inputs.Player.Shoot;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        firetimer -= Time.deltaTime;
        shootFunction();
        
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    void shootFunction()
    {
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        debugray = ray;
        
        //Play animation and    particle effect
        if (Physics.Raycast(ray, out hit, range))
        {
            // Debug.Log("Player hit something!");

            GameObject target = hit.collider.gameObject;
            if (target.tag == "Target" && shootAction.WasPerformedThisFrame())
            {
                Debug.Log($"{hit.collider.name} has been hit!");
                GameObject.Destroy(target);
            }
            
        }

        firetimer = 0.1f;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(debugray.origin, debugray.direction * range);
    }


}

//Code reference:
// The logic for the shooting with raycasting :https://www.youtube.com/watch?v=xasmH86e4PE