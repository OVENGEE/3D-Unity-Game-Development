using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowState : PlayerState,ITriggerHandler
{
    //Input Action Declaration
    InputAction aimAction;
    InputAction pickUpAction;
    InputAction throwAction;
    InputAction crouchAction;
    InputAction sprintAction;
    InputAction moveAction;

    //Vector
    private Vector2 moveDirectionInput;
    private Vector3 velocity;
    private Vector3 move;

    //Controller
    private CharacterController controller;

    //Constants
    const float GRAVITY = -9.81f;



    //Pick Up variables
    private float PickUpRange;
    private Transform holdPoint;
    private PickUpObject heldObject;

    //Throw variables
    private float throwForce;
    private float throwUpwardBoost;
    private Transform releasePosition;

    //Calculations
    private int linePoints;
    private float timeBetweenPoints;

    //camera
    private Camera camera;

    //Line renderer
    private LineRenderer lineRenderer;

    public PlayerThrowState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        controller = base.player.GetComponent<CharacterController>();

        Player.PlayerState currentState = base.player.playerState;
        currentState = Player.PlayerState.Throw;
        base.player.UpdateState(currentState);

        //Assigning from player monobehaviour
        PickUpRange = base.player.PickUpRange;
        holdPoint = base.player.holdPoint;
        throwForce = base.player.throwForce;
        throwUpwardBoost = base.player.throwUpwardBoost;
        lineRenderer = base.player.LineRenderer;
        releasePosition = base.player.ReleasePosition;
        camera = base.player.camera;
        linePoints = base.player.LinePoints;
        timeBetweenPoints = base.player.TimeBetweenPoints;

        InputActionAssignment();


        //Event Subscriptions
        throwAction.performed += OnThrow;
        pickUpAction.performed += OnPickUp;
        sprintAction.performed += OnExitThrowStateToSprint;
        crouchAction.performed += OnExitThrowStateToCrouch;
    }

    public override void ExitState()
    {
        base.ExitState();
        heldObject?.Drop();

        //Event unsubscriptions
        throwAction.performed -= OnThrow;
        pickUpAction.performed -= OnPickUp;
        sprintAction.performed -= OnExitThrowStateToSprint;
        crouchAction.performed -= OnExitThrowStateToCrouch;
    }

    public void OnTriggerExit(Collider other)
    {
        //The invisibleBoundary is layerIndex 10
        if (other.gameObject.layer == 10)
        {
            playerStateMachine.SwitchState(new PlayerWalkState(player, playerStateMachine));
        }
    }

    private void InputActionAssignment()
    {
        //Assigning all Input actions of the state
        aimAction = base.player.inputs?.Player.Aim;
        pickUpAction = base.player.inputs?.Player.PickUp;
        throwAction = base.player.inputs?.Player.Throw;
        sprintAction = base.player.inputs?.Player.Sprint;
        crouchAction = base.player.inputs?.Player.Crouch;
        moveAction = base.player.inputs?.Player.Move;

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
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        moveDirectionInput = moveAction.ReadValue<Vector2>();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        HandleMovement();
        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);

            // Show and update the throw trajectory with collision detection

            if (aimAction.IsPressed())
            {
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.enabled = false;
            }


            Vector3 dir = camera.transform.forward;
            Vector3 impulse = dir * throwForce + Vector3.up * throwUpwardBoost;
            Vector3 startPosition = releasePosition.position;
            Vector3 previousPoint = startPosition;
            Vector3 velocity = impulse;

            int points = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
            lineRenderer.positionCount = points;

            for (int i = 0; i < points; i++)
            {
                float time = i * timeBetweenPoints;
                Vector3 point = startPosition + velocity * time + 0.5f * Physics.gravity * time * time;

                // Raycast from previous point to current point
                if (i > 0)
                {
                    Vector3 segment = point - previousPoint;
                    if (Physics.Raycast(previousPoint, segment.normalized, out RaycastHit hit, segment.magnitude))
                    {
                        lineRenderer.SetPosition(i, hit.point);
                        // Set remaining points to the collision point
                        for (int j = i + 1; j < points; j++)
                            lineRenderer.SetPosition(j, hit.point);
                        break;
                    }
                }

                lineRenderer.SetPosition(i, point);
                previousPoint = point;
            }
        }
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (heldObject == null)
        {
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, PickUpRange))
            {
                PickUpObject pickUp = hit.collider.GetComponent<PickUpObject>();
                if (pickUp != null)
                {
                    pickUp.PickUp(holdPoint);
                    heldObject = pickUp;
                }
            }
        }
        else
        {
            heldObject.Drop();
            heldObject = null;
        }
    }


    //Throw logic:
    // ===========================
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (heldObject == null) return;

        Vector3 dir = camera.transform.forward;
        Vector3 impulse = dir * throwForce + Vector3.up * throwUpwardBoost;

        heldObject.Throw(impulse);
        heldObject = null;

        lineRenderer.enabled = false; // Hide the trajectory line immediately after throw
    }

    public void OnExitThrowStateToSprint(InputAction.CallbackContext context)
    {
        playerStateMachine.SwitchState(new PlayerSprintState(player, playerStateMachine));
    }

        public void OnExitThrowStateToCrouch(InputAction.CallbackContext context)
    {
        playerStateMachine.SwitchState(new PlayerCrouchState(player, playerStateMachine));
    }

}
