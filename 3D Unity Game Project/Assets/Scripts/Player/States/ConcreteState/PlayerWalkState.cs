 using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerWalkState : PlayerState
{
    //Constants
    const float GRAVITY = -9.81f;
    const float JUMPHEIGHT = 2f;

    //Input
    InputAction moveAction;
    InputAction crouchAction;
    InputAction SprintAction;
    InputAction jumpAction;

    //Vectors
    private Vector2 moveDirectionInput;
    private Vector3 velocity;
    protected Vector3 move;
    protected CharacterController controller;

    // Player references
    protected Slider slider;


    public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        controller = base.player.GetComponent<CharacterController>();
        slider = base.player.StaminaSlider;
        NullChecks();
        base.player.stateText.text = "Walk";

        //Event Subscriptions
        crouchAction.performed += OnCrouch;
        SprintAction.performed += OnSprint;
        jumpAction.performed += OnJump;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        moveDirectionInput = moveAction.ReadValue<Vector2>();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        HandleMovement();
    }

    public override void ExitState()
    {
        base.ExitState();

        //Event un-Subscriptions
        crouchAction.performed -= OnCrouch;
        SprintAction.performed -= OnSprint;
        jumpAction.performed -= OnJump;
    }
    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    void HandleMovement()
    {
        //Motion calculation
        move = base.player.transform.right * moveDirectionInput.x + base.player.transform.forward * moveDirectionInput.y;
        float moveSpeed = base.player.MoveSpeed;

        //Apply motion to controller
        controller.Move(move * moveSpeed * Time.deltaTime);

        //Ground check for controller
        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;

        velocity.y += GRAVITY * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    public void HandleJump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(JUMPHEIGHT * -2f * GRAVITY);
        }
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

        if (SprintAction == null)
        {
            SprintAction = base.player.inputs.Player.Sprint;
        }

        if (jumpAction == null)
        {
            jumpAction = base.player.inputs.Player.Jump;
        }
    }

    public void FOVTransition(float newFOV)
    {
        float initialFOV = base.player.camera.fieldOfView;
        base.player.camera.fieldOfView = Mathf.Lerp(initialFOV, newFOV, 0.5f);
    }

    //Event Handlers
    private void OnCrouch(InputAction.CallbackContext context)
    {
        playerStateMachine.SwitchState(new PlayerCrouchState(player, playerStateMachine));
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if (base.player.canSprint)
        {
            playerStateMachine.SwitchState(new PlayerSprintState(player, playerStateMachine));
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        HandleJump();
    }
}

// Code references:
// 1)Title: A Better Way to Code Your Characters in Unity | Finite State Machine | Tutorial
//  Author: Sasquatch B Studios
//  Date accessed:  3/08/2025
//  Availability: https://www.youtube.com/watch?v=RQd44qSaqww&ab_channel=SasquatchBStudios
