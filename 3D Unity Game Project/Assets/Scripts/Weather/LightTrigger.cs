using System;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    public static event Action OnLightTrigger;
    private bool isTriggered;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isTriggered = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            OnLightTrigger?.Invoke();
            // isTriggered = true;
        }
    }
}
