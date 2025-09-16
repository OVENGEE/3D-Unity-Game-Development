using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private Rigidbody rb;
    internal Vector3 position;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PickUp(Transform holdpoint)
    {
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.SetParent(holdpoint);
        transform.localPosition = Vector3.zero;

        this.GetComponent<Collider>().enabled = false;
    }

    public void Drop()
    {
        rb.useGravity = true;
        transform.SetParent(null);
        this.GetComponent<Collider>().enabled = true;
    }

    public void Throw(Vector3 impulse)
    {
        transform.SetParent(null);
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(impulse, ForceMode.Impulse);
        this.GetComponent<Collider>().enabled = true;    
    }

    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        rb.MovePosition(targetPosition);
    }

}
