
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    //Movement Settings
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    //Look requirements
    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;



    private CharacterController controller;

    //State instances 

    public PlayerStateMachine StateMachine { get; set; }
    public PlayerWalkState WalkState { get; set; }
    public PlayerCrouchState CrouchState { get; set; }

    //Input 
    public CustomInputSystem inputs;



    void Awake()
    {
        controller = GetComponent<CharacterController>();
        StateMachine = new PlayerStateMachine();
        WalkState = new PlayerWalkState(this, StateMachine);
        CrouchState = new PlayerCrouchState(this, StateMachine);

        if (inputs == null)
        {
            inputs = new CustomInputSystem();
            Debug.Log("Custom input new instance made!");
        }
    }

    void Start()
    {
        StateMachine.Initialise(CrouchState);
    }

    public void OnEnable()
    {
        inputs.Enable();
    }

    void Update()
    {
        StateMachine.CurrentPlayerState.FrameUpdate();
    }

    void FixedUpdate()
    {
        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }

    public void OnDisable()
    {
        inputs.Disable();
    }
    #region Animation Triggers

    //Deals with all animations for the player

    #endregion
}

// Code reference:
// The FSM Logic is from this video: https://www.youtube.com/watch?v=RQd44qSaqww
