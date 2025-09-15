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


    //State instances 

    public PlayerStateMachine StateMachine { get; set; }
    public PlayerWalkState WalkState { get; set; }
    public PlayerCrouchState CrouchState { get; set; }

    public PlayerShootState ShootState { get; set; }
    public PlayerSprintState SprintState { get; set; }

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
    

    //Camera reference
    public Camera camera;

    //UI dependancies
    [Header("UI references")]
    public TextMeshProUGUI stateText;
    public GameObject InteractSlider;
    public Slider StaminaSlider;

    [Header("PickUp Settings")]
    public float PickUpRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Throwing Settings")]
    public float throwForce = 10;
    public float throwUpwardBoost = 1f;

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
        throwAction.performed += OnThrow;
        pickUpAction.performed += OnPickUp;
    }

    void Update()
    {
        StateMachine.CurrentPlayerState.FrameUpdate();

        
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
        //Unsubscribe the events and disable input
        throwAction.performed -= OnThrow;
        pickUpAction.performed -= OnPickUp;
        inputs.Disable();
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

    public void SwitchToShootState()
    {
        InteractSlider.SetActive(false);
        StateMachine.SwitchState(ShootState);
        tempGun.SetActive(true);
    }

    public void StartSprintCooldown()
    {
        sprintCooldownTimer = sprintCooldown;
        canSprint = false;
    }

    public IEnumerator StaminaRecover(float currentStamina)
    {
        yield return new WaitForSeconds(1f);

        staminaTimer = currentStamina;
        while (staminaTimer < MaxStamina)
        {
            staminaTimer += ChargeRate / 10f;
            if (staminaTimer > MaxStamina) staminaTimer = MaxStamina;
            StaminaSlider.value = staminaTimer / MaxStamina;
            yield return new WaitForSeconds(.1f);
        }

        canSprint = true;
    }

    private void NullChecks()
    {
        if (inputs == null)
        {
            inputs = new CustomInputSystem();
            Debug.Log("Custom input new instance made!");

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

        if (InteractSlider == null || StaminaSlider == null)
        {
            Debug.Log("the sliders is not assigned to the Player inspector!");
            return;
        }

        if (tempGun == null)
        {
            Debug.Log("the tempGun is not assigned to the Player inspector!");
            return;
        }

        if (muzzleflash == null)
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
