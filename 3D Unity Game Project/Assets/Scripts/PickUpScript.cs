using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float throwForce = 500f;
    public float pickUpRange = 5f;
    private float rotationSensitivity = 1f;
    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private bool canDrop = true;
    private int LayerNumber;
    public Camera playerCamera; // Add this public field

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer");
    }

    void Update()
    {
        // Keep held object at hold position every frame
        if (heldObj != null)
        {
            MoveObject();
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform;
            heldObj.layer = LayerNumber;
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }

    public void PickUpAgain(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (heldObj == null)
        {
            RaycastHit hit;
            // Use camera position and forward
            Vector3 origin = playerCamera != null ? playerCamera.transform.position : transform.position;
            Vector3 direction = playerCamera != null ? playerCamera.transform.forward : transform.forward;
            Debug.DrawRay(origin,direction*20,Color.red);
            if (Physics.Raycast(origin, direction, out hit, pickUpRange))
            {
                if (hit.transform.gameObject.CompareTag("canPickUp"))
                {
                    PickUpObject(hit.transform.gameObject);
                }
            }
        }
        else
        {
            if (canDrop)
            {
                StopClipping();
                DropObject();
            }
        }
    }

    void DropObject()
    {
        if (heldObj == null) return;
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObj = null;
    }

    void MoveObject()
    {
        if (heldObj != null)
            heldObj.transform.position = holdPos.transform.position;
    }

    public void RotateObject(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (heldObj != null)
        {
            canDrop = false;
            Vector2 mouseDelta = context.ReadValue<Vector2>();
            float XaxisRotation = mouseDelta.x * rotationSensitivity;
            float YaxisRotation = mouseDelta.y * rotationSensitivity;
            heldObj.transform.Rotate(Vector3.down, XaxisRotation, Space.World);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation, Space.World);
        }
        else
        {
            canDrop = true;
        }
    }

    public void ThrowObject(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (heldObj == null) return;
        if (canDrop)
        {
            StopClipping();
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            heldObj.layer = 0;
            heldObjRb.isKinematic = false;
            heldObj.transform.parent = null;
            heldObjRb.AddForce(transform.forward * throwForce);
            heldObj = null;
        }
    }

    void StopClipping()
    {
        if (heldObj == null) return;
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        if (hits.Length > 1)
        {
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
        }
    }
}