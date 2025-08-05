
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    //Movement Settings
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;

    private CharacterController controller;

    //State instances 

    public PlayerStateMachine StateMachine { get; set; }
    public PlayerWalkState WalkState { get; set; }
    public PlayerCrouchState CrouchState { get; set; }

    public PlayerShootState ShootState { get; set; }

    //Input 
    public CustomInputSystem inputs;



    void Awake()
    {
        controller = GetComponent<CharacterController>();
        StateMachine = new PlayerStateMachine();
        WalkState = new PlayerWalkState(this, StateMachine);
        CrouchState = new PlayerCrouchState(this, StateMachine);
        ShootState = new PlayerShootState(this, StateMachine);

        if (inputs == null)
        {
            inputs = new CustomInputSystem();
            Debug.Log("Custom input new instance made!");
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
