using UnityEngine;

public class PickUpObject : MonoBehaviour,IHoldable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody rb;

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
        Equip();
    }

    public void Drop()
    {
        rb.useGravity = true;
        transform.SetParent(null);
        this.GetComponent<Collider>().enabled = true;
        UnEquip();
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

    public void Equip()
    {
        Debug.Log("BasketBall equipped!");
    }

    public void UnEquip()
    {
        Debug.Log("BasketBall unequipped");
    }
}
