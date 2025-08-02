
using UnityEngine;

public class Player : MonoBehaviour
{
    //Constants
    float GRAVITY = -9.8f;

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
    public PlayerCrouchState CrouchState {get; set;}



    void Awake()
    {
        controller = GetComponent<CharacterController>();
        StateMachine = new PlayerStateMachine();
        WalkState = new PlayerWalkState(this, StateMachine);
        CrouchState = new PlayerCrouchState(this, StateMachine);
    }

    void Start()
    {
        StateMachine.Initialise(CrouchState);
    }

    void Update()
    {
        StateMachine.CurrentPlayerState.FrameUpdate();
    }

    void FixedUpdate()
    {
        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }
    #region Animation Triggers

    //Deals with all animations for the player

    #endregion
}

// Code reference:
// The FSM Logic is from this video: https://www.youtube.com/watch?v=RQd44qSaqww
