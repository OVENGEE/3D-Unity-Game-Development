
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

        if (!canSprint)
        {
            sprintCooldownTimer -= Time.deltaTime;
            if (sprintCooldownTimer <= 0f)
            {
                canSprint = true;
                Debug.Log("Sprint rest, cooldown done!");
            }
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
        canSprint = false;
        sprintCooldownTimer = sprintCooldown;
    }



    #region Animation Triggers

    //Deals with all animations for the player

    #endregion
}

// Code reference:
// The FSM Logic is from this video: https://www.youtube.com/watch?v=RQd44qSaqww
