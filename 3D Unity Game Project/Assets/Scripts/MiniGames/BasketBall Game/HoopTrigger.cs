using UnityEngine;

public class HoopTrigger : MonoBehaviour
{
    private int score = 0;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            score++;
            Debug.Log($"Basket Scored! Score is {score} ");
        }
    }
}
