using UnityEngine;

public class RotationByZ : MonoBehaviour
{
    public float rotationSpeedz = -0.5f;
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeedz);
    }
}
