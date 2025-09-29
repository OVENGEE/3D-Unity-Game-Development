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

    public ParticleSystem sprintEffect;
    

    //Camera reference
    public Camera camera;

    //UI dependancies
    [Header("UI references")]
    public TextMeshProUGUI stateText;
    public GameObject InteractSlider;
    //public Slider StaminaSlider;

    [Header("PickUp Settings")]
    public float PickUpRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Throwing Settings")]
    public float throwForce = 10;
    public float throwUpwardBoost = 1f;

     [SerializeField]
    private LineRenderer LineRenderer;
    private LayerMask CollisionMask;

    [SerializeField]
    private Transform ReleasePosition;

    [Header("Display Controls")]

    [SerializeField]
    [Range(10, 100)]
    private int LinePoints = 25;
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float TimeBetweenPoints = 0.1f;

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

            // Show and update the throw trajectory with collision detection
            LineRenderer.enabled = true;
            LineRenderer.startColor = Color.white;
            Vector3 dir = camera.transform.forward;
            Vector3 impulse = dir * throwForce + Vector3.up * throwUpwardBoost;
            Vector3 startPosition = ReleasePosition.position;
            Vector3 previousPoint = startPosition;
            Vector3 velocity = impulse;

            int points = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
            LineRenderer.positionCount = points;

            for (int i = 0; i < points; i++)
            {
                float time = i * TimeBetweenPoints;
                Vector3 point = startPosition + velocity * time + 0.5f * Physics.gravity * time * time;

                // Raycast from previous point to current point
                if (i > 0)
                {
                    Vector3 segment = point - previousPoint;
                    if (Physics.Raycast(previousPoint, segment.normalized, out RaycastHit hit, segment.magnitude))
                    {
                        LineRenderer.SetPosition(i, hit.point);
                        // Set remaining points to the collision point
                        for (int j = i + 1; j < points; j++)
                            LineRenderer.SetPosition(j, hit.point);
                        break;
                    }
                }

                LineRenderer.SetPosition(i, point);
                previousPoint = point;
            }
        }
        else
        {
            // Hide the trajectory line when not holding an object
            LineRenderer.enabled = false;
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

        LineRenderer.enabled = false; // Hide the trajectory line immediately after throw
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
