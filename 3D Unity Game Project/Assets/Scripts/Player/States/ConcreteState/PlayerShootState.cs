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
        InputAction moveAction;

        public PlayerShootState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            range = 20f;
            camera = base.player.camera;

            Player.PlayerState currentState = base.player.playerState;
            currentState = Player.PlayerState.Shoot;
            base.player.UpdateState(currentState);

            if (shootAction == null)
            {
                shootAction = base.player.inputs.Player.Shoot;
            }

            if (moveAction == null)
            {
                moveAction = base.player.inputs.Player.Move;
            }

            //Event Subscriptions
            shootAction.performed += OnshootFunction;
        }

        public override void ExitState()
        {
            base.ExitState();

            //Event unSubscriptions
            shootAction.performed -= OnshootFunction;
            OnExitShootState();
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

        //Play animation and    particle effect
        base.player.muzzleflash.Play();

        if (Physics.Raycast(ray, out hit, range))
        {

            if (hit.collider.TryGetComponent<Target>(out Target target))
            {
                Debug.Log($"{hit.collider.name} has been hit!");
                target.PlayAfterShotRoutine();
            }



            firetimer = 0.1f;
        }
    }


    private void OnExitShootState()
    {
        //Switch to the walking state!
        base.player.tempGun.SetActive(false);
        playerStateMachine.SwitchState(new PlayerWalkState(player, playerStateMachine));
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
