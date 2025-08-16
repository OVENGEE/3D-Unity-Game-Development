using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    //Movement Settings
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;


    //State instances 

    public PlayerStateMachine StateMachine { get; set; }
    public PlayerWalkState WalkState { get; set; }
    public PlayerCrouchState CrouchState { get; set; }

    public PlayerShootState ShootState { get; set; }
    public PlayerSprintState SprintState { get; set; }

    //Input 
    public CustomInputSystem inputs;

    //Sprint variables
    [Header("Sprint Variables")]
    public bool canSprint = true;
    public float sprintCooldown = 1f;

    private float sprintCooldownTimer = 0f;

    //Camera reference
    public Camera camera;

    //UI dependancies
    public TextMeshProUGUI stateText;

    [Header("PickUp Settings")]
    public float PickUpRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Throwing Settings")]
    public float throwForce = 10;
    public float throwUpwardBoost = 1f;



    void Awake()
    {
        StateMachine = new PlayerStateMachine();
        WalkState = new PlayerWalkState(this, StateMachine);
        CrouchState = new PlayerCrouchState(this, StateMachine);
        ShootState = new PlayerShootState(this, StateMachine);
        SprintState = new PlayerSprintState(this, StateMachine);

        if (inputs == null)
        {
            inputs = new CustomInputSystem();
            Debug.Log("Custom input new instance made!");
        }

        if (camera == null)
        {
            camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    void Start()
    {
        StateMachine.Initialise(WalkState);
    }

    public void OnEnable()
    {
        inputs.Enable();
    }

    void Update()
    {
        StateMachine.CurrentPlayerState.FrameUpdate();

        //This reset the cooldown of the sprint state even if you are not in the sprint state
        if (!canSprint)
        {
            sprintCooldownTimer -= Time.deltaTime;
            if (sprintCooldownTimer <= 0f)
            {
                canSprint = true;
                Debug.Log("Sprint rest, cooldown done!");
            }
        }
        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }
    }

    void FixedUpdate()
    {
        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }

    public void OnDisable()
    {
        inputs.Disable();
    }

    public void StartSprintCooldown()
    {
        //Starts the sprintCooldown;
        canSprint = false;
        sprintCooldownTimer = sprintCooldown;
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

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (heldObject == null) return;

        Vector3 dir = camera.transform.forward;
        Vector3 impulse = dir * throwForce + Vector3.up * throwUpwardBoost;

        heldObject.Throw(impulse);
        heldObject = null;
    } 


    #region Animation Triggers

    //Deals with all animations for the player

    #endregion
}

// Code reference:
// The FSM Logic is from this video: https://www.youtube.com/watch?v=RQd44qSaqww
