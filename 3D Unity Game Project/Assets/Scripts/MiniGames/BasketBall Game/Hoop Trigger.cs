using System;
using UnityEngine;

public class HoopTrigger : MonoBehaviour
{
    private int score = 0;
    public static event Action<int> OnHoopScored;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            score++;
            OnHoopScored?.Invoke(score);
            Debug.Log($"Basket Scored! Score is {score} ");
        }
    }
}
