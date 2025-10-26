using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    //Movement Settings
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;


    //State Information
    [Header("State Data")]
    [SerializeField] public PlayerState playerState;
    public enum PlayerState
    {
        Walk,
        Crouch,
        Shoot,
        Sprint,
        Throw
    } 

    public PlayerStateMachine StateMachine { get; set; }
    public PlayerWalkState WalkState { get; set; }
    public PlayerCrouchState CrouchState { get; set; }

    public PlayerShootState ShootState { get; set; }
    public PlayerSprintState SprintState { get; set; }
    public PlayerThrowState ThrowState { get; set; }

    //State event
    public static event Action<PlayerState> OnPlayerStateChange;

    //Input 
    public CustomInputSystem inputs;
    private InputAction throwAction;
    private InputAction pickUpAction;

    //Sprint variables
    [Header("Sprint Variables")]
    public bool canSprint = true;
    public float sprintCooldownTimer = 0f;
    public float sprintCooldown = 3f;
    public float MaxStamina = 4f;
    public float ChargeRate = 33f;
    public Coroutine recharge;  
    private float staminaTimer = 0f;

    public ParticleSystem sprintEffect;
    

    //Camera reference
    public Camera camera;

    //UI dependancies
    [Header("UI references")]
    public TextMeshProUGUI stateText;
    public GameObject InteractSlider;

    [Header("PickUp Settings")]
    public float PickUpRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Throwing Settings")]
    public float throwForce = 10;
    public float throwUpwardBoost = 1f;

    [SerializeField]
    public LineRenderer LineRenderer;


    [SerializeField]
    public Transform ReleasePosition;

    [Header("Display Controls")]

    [SerializeField]
    [Range(10, 100)]
    public int LinePoints = 25;
    [SerializeField]
    [Range(0.01f, 0.25f)]
    public float TimeBetweenPoints = 0.1f;

    [Header("Gun Settings")]

    public GameObject tempGun;
    public ParticleSystem muzzleflash;

    void Awake()
    {
        StateMachine = new PlayerStateMachine();
        WalkState = new PlayerWalkState(this, StateMachine);
        CrouchState = new PlayerCrouchState(this, StateMachine);
        ShootState = new PlayerShootState(this, StateMachine);
        SprintState = new PlayerSprintState(this, StateMachine);
        ThrowState = new PlayerThrowState(this, StateMachine);
        NullChecks();
    }

    void Start()
    {
        StateMachine.Initialise(WalkState);
    }

    public void OnEnable()
    {
        //Enable input and subscribe events
        inputs.Enable();
        PanelController.OnEnablePlayerInput += EnablePlayerInput;
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
        //Unsubscribe the events and disable input
        PanelController.OnEnablePlayerInput -= EnablePlayerInput;
        inputs.Disable();
    }

    public void SwitchToShootState()
    {
        InteractSlider.SetActive(false);
        StateMachine.SwitchState(ShootState);
        tempGun.SetActive(true);
    }

    public void SwitchToThrowState()
    {
        StateMachine.SwitchState(ThrowState);
    }

    public void StartSprintCooldown()
    {
        sprintCooldownTimer = sprintCooldown;
        canSprint = false;
    }

    public IEnumerator StaminaRecover(float currentStamina)
    {
        yield return new WaitForSeconds(1f);
        float staminaValue;
        staminaTimer = currentStamina;
        while (staminaTimer < MaxStamina)
        {
            staminaTimer += ChargeRate / 10f;
            if (staminaTimer > MaxStamina) staminaTimer = MaxStamina;
            staminaValue = staminaTimer / MaxStamina;
            UpdateStaminaSlider(staminaValue);
            yield return new WaitForSeconds(.1f);
        }

        canSprint = true;
    }

    public static event Action<float> OnSliderChange;

    public void UpdateStaminaSlider(float value)
    {
        OnSliderChange?.Invoke(value);
    }

    public void UpdateState(PlayerState state)
    {
        OnPlayerStateChange?.Invoke(state);
    }

    void OnTriggerExit(Collider other)
    {
        (StateMachine.CurrentPlayerState as ITriggerHandler)?.OnTriggerExit(other);
    }

    private void EnablePlayerInput(bool state)
    {
        if (state)
            inputs?.Player.Enable();
        else
            inputs?.Player.Disable();
    }
    
    private void NullChecks()
    {
        if (inputs == null)
        {
            inputs = new CustomInputSystem();

            if (throwAction == null)
            {
                throwAction = inputs.Player.Throw;
            }

            if (pickUpAction == null)
            {
                pickUpAction = inputs.Player.PickUp;
            }
        }

        if (camera == null)
        {
            camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }

        if (InteractSlider == null)
        {
            Debug.Log("the slider is not assigned to the Player inspector!");
            return;
        }

        if (tempGun == null)
        {
            Debug.Log("the tempGun is not assigned to the Player inspector!");
            return;
        }

        if (muzzleflash == null || sprintEffect == null)
        {
            Debug.Log("the Particle system is not assigned to the player inspector!");
            return;
        }
    }



    #region Animation Triggers

    //Deals with all animations for the player

    #endregion
}

// Code reference:
// The FSM Logic is from this video: https://www.youtube.com/watch?v=RQd44qSaqww
