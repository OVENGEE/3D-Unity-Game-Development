using UnityEngine;

public class ControlData : MonoBehaviour
{
    public ControlType controlType;
}

public enum ControlType
{
    Interact,
    PickUp
}
