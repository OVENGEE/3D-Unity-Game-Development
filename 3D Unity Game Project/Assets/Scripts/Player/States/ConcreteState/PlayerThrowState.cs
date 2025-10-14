using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowState : PlayerWalkState,ITriggerHandler
{
    //Input Action Declaration
    InputAction aimAction;
    InputAction pickUpAction;
    InputAction throwAction;

    //Pick Up variables
    private float PickUpRange;
    private Transform holdPoint;
    private PickUpObject heldObject;

    //Throw variables
    private float throwForce;
    private float throwUpwardBoost;
    private Transform releasePosition;

    //Line renderer
    private LineRenderer lineRenderer;

    public PlayerThrowState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        //Assigning from player monobehaviour
        PickUpRange = base.player.PickUpRange;
        holdPoint = base.player.holdPoint;
        throwForce = base.player.throwForce;
        throwUpwardBoost = base.player.throwUpwardBoost;
        lineRenderer = base.player.LineRenderer;
        releasePosition = base.player.ReleasePosition;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public void OnTriggerExit(Collider other)
    {
        //The invisibleBoundary is layerIndex 10
        if (other.gameObject.layer == 10)
        {
            ExitState();
        }
    }

    private void InputActionAssignment()
    {
        //Assigning all Input actions of the state
        if (pickUpAction == null)
        {
            pickUpAction = base.player.inputs.Player.PickUp;
        }

        if (throwAction == null)
        {
            throwAction = base.player.inputs.Player.Throw;
        }
    }



    // PickUpObject Logic:
    // ========================
    //     public void OnPickUp(InputAction.CallbackContext context)
    //     {
    //         if (!context.performed) return;

    //         if (heldObject == null)
    //         {
    //             Ray ray = new Ray(camera.transform.position, camera.transform.forward);
    //             if (Physics.Raycast(ray, out RaycastHit hit, PickUpRange))
    //             {
    //                 PickUpObject pickUp = hit.collider.GetComponent<PickUpObject>();
    //                 if (pickUp != null)
    //                 {
    //                     pickUp.PickUp(holdPoint);
    //                     heldObject = pickUp;
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             heldObject.Drop();
    //             heldObject = null;
    //         }
    //     }


    //Throw logic:
    // ===========================
    //     public void OnThrow(InputAction.CallbackContext context)
    // {
    //     if (!context.performed) return;
    //     if (heldObject == null) return;

    //     Vector3 dir = camera.transform.forward;
    //     Vector3 impulse = dir * throwForce + Vector3.up * throwUpwardBoost;

    //     heldObject.Throw(impulse);
    //     heldObject = null;

    //     LineRenderer.enabled = false; // Hide the trajectory line immediately after throw
    // }
}
