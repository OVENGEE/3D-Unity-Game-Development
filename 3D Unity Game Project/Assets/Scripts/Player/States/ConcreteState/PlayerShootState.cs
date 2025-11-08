using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootState : PlayerWalkState,ITriggerHandler
{
    //Shoot references
    private float firetimer;
    private float range;
    private Camera camera;
    Ray debugray;
    bool shoot;
    int score;

    //Input actions
    InputAction shootAction;
    InputAction sprintAction;

    //Events
    public static event Action<int> OnTargetShot;
    public static event Action<PanelType> OnShootPanelTrigger;
    public static event Action OnShootPanelReset;

    //Particle System
    private ParticleSystem gunflash;    

        public PlayerShootState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            range = 20f;
            camera = base.player.camera;

            score = 0;
            Player.PlayerState currentState = base.player.playerState;
            gunflash = base.player.Gun?.GetComponentInChildren<ParticleSystem>();

        
            currentState = Player.PlayerState.Shoot;
            base.player.UpdateState(currentState);
            OnShootPanelTrigger?.Invoke(PanelType.DuckShootingGame);

            if (shootAction == null)
            {
                shootAction = base.player.inputs.Player.Shoot;
            }

            if (sprintAction == null)
            {
                sprintAction = base.player.inputs.Player.Sprint;
            }

            //Event Subscriptions
            shootAction.performed += OnshootFunction;
            sprintAction.performed += OnSprintActivated;
        }

    public override void ExitState()
    {
            base.ExitState();

            //Event unSubscriptions
            shootAction.performed -= OnshootFunction;
            sprintAction.performed -= OnSprintActivated;
    }

    public override void PhysicsUpdate()
    {
            base.PhysicsUpdate();
    }

    public override void FrameUpdate()
    {
            base.FrameUpdate();
            firetimer -= Time.deltaTime;
    }

    public override void AnimationTriggerEvent()
    {
            base.AnimationTriggerEvent();
    }

    void OnshootFunction(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        //Play animation and particle effect
        gunflash.Play();
        if (Physics.Raycast(ray, out hit, range))
        {

            if (hit.collider.TryGetComponent<Target>(out Target target))
            {
                score++;
                OnTargetShot?.Invoke(score);
                
                Debug.Log($"{hit.collider.name} has been hit!");
                target.PlayAfterShotRoutine();
            }



            firetimer = 0.1f;
        }
    }


    private void OnExitShootState()
    {
        //Switch to the walking state!
        base.player.Gun.SetActive(false);
        OnShootPanelReset?.Invoke();
        playerStateMachine.SwitchState(new PlayerWalkState(player, playerStateMachine));
    }
    public void OnTriggerExit(Collider other)
    {
        //The invisibleBoundary is layerIndex 10
        if (other.gameObject.layer == 10)
        {
            OnExitShootState();
        }
    }

    void OnSprintActivated(InputAction.CallbackContext context)
    {
        //Switch to the sprint state!
        base.player.Gun.transform.SetParent(null);
        base.player.Gun.SetActive(false);
        playerStateMachine.SwitchState(new PlayerSprintState(player, playerStateMachine));
    }
}

//Code references:
// 1)Title: Unity - Shooting in Unity | Raycasting - (10 Minute tutorial - 2022 UPDATED)
//  Author: Game Dev Guru
//  Date accessed:  17/08/2025
//  Availability: https://www.youtube.com/watch?v=xasmH86e4PE

// 2)Title: A Better Way to Code Your Characters in Unity | Finite State Machine | Tutorial
//  Author: Sasquatch B Studios
//  Date accessed:  3/08/2025
//  Availability: https://www.youtube.com/watch?v=RQd44qSaqww&ab_channel=SasquatchBStudios
