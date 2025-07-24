using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMovement : MonoBehaviour
{
    //Input system and its actions
    InputSystem_Actions inputActions;
    InputAction moveInput;
    InputAction jumpInput;
    InputAction crouchInput;

    [Header("References")]
    public Rigidbody rb;
    // public Transform playerMouse;


    [Header("Ground Check (Sphere)")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    [Space(5f)]


    [Header("Player variables")]
    public float jumpforce = 4f;
    public float movementspeed = 4f;


    private Vector3 _movedirection;




    [Header("Check flags")]
    //check flags read only
    public bool _isGrounded { get; private set; }
    public bool _isJumpPressed{ get; private set; }
    public bool _isCrouchPressed{ get; private set; }



    void Awake()
    {
        _isJumpPressed = false;
        _isCrouchPressed = false;
        NullCheck();
    }

    void NullCheck()
    {
        if (rb == null)
        {
            Debug.Log("The rigidbody has not been assigned the inspector");
            return;
        }

        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();
            Debug.Log("inputActions now set!");

            if (moveInput == null)
            {
                moveInput = inputActions.Player.Move;
                Debug.Log("moveInput now set!");
            }

            if (jumpInput == null)
            {
                jumpInput = inputActions.Player.Jump;
                Debug.Log("jumpInput now set!");
            }

            if (crouchInput == null)
            {
                crouchInput = inputActions.Player.Crouch;
                Debug.Log("crouchInput now set!");
            }
        }

        if (groundCheckPoint == null)
        {
            Debug.Log("One of the transforms are not assigned in the inspector");
            return;
        }


    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    
    void Update()
    {
        //player controller movement
        _movedirection = moveInput.ReadValue<Vector2>();
    
        //Jump checks
        if (jumpInput.WasPressedThisFrame())
        {
            Debug.Log("Jump pressed:" + jumpInput.WasPressedThisFrame());
            _isJumpPressed = true;  
        }

    }

    void FixedUpdate()
    {
        // Ground Check
        _isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);


        // Horizontal movement
        Vector3 horizontal = transform.right * _movedirection.x + transform.forward*_movedirection.y;
        horizontal *= movementspeed;

        rb.linearVelocity = new Vector3(horizontal.x, rb.linearVelocity.y, horizontal.z);

        // Jump
        if (_isJumpPressed && _isGrounded)
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            Debug.Log("Player is jumping!");
            _isJumpPressed = false;
        }
    }

    void Oncrouch()
    {
        if (crouchInput.WasPressedThisFrame())
        {
            _isCrouchPressed = !_isCrouchPressed;

            if (_isCrouchPressed)
            {
                
            }

        }
    }


    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }

    void OnDisable()
    {
        inputActions.Disable();
    }
}
