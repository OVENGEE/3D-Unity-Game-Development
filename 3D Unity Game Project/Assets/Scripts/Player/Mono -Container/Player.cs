
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


    #region Animation Triggers

    //Deals with all animations for the player

    #endregion
}

// Code reference:
// The FSM Logic is from this video: https://www.youtube.com/watch?v=RQd44qSaqww
