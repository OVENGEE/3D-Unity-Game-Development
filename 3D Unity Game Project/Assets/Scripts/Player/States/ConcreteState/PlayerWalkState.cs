using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerState
{
    //Constants
    float GRAVITY = -9.81f;

    //Input
    InputAction moveAction;
    InputAction crouchAction;
    InputAction SprintAction;

    //Vectors
    private Vector2 moveDirectionInput;
    private Vector3 velocity;
    private CharacterController controller;


    public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        controller = base.player.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        NullChecks();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        moveDirectionInput = moveAction.ReadValue<Vector2>();

        if (crouchAction.WasPressedThisFrame())
        {
            playerStateMachine.SwitchState(new PlayerCrouchState(player, playerStateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        HandleMovement();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    void HandleMovement()
    {
        //Motion calculation
        Vector3 move = base.player.transform.right * moveDirectionInput.x + base.player.transform.forward * moveDirectionInput.y;
        float moveSpeed = base.player.MoveSpeed;

        //Apply motion to controller
        controller.Move(move * moveSpeed * Time.deltaTime);

        //Ground check for controller
        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;

        velocity.y += GRAVITY * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    void NullChecks()
    {
        if (moveAction == null)
        {
            moveAction = base.player.inputs.Player.Move;
        }

        if (crouchAction == null)
        {
            crouchAction = base.player.inputs.Player.Crouch;
        }
    }
}
