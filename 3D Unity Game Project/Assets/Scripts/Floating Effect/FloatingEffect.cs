using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public float speed = 2f;
    public float height = 0.5f;

    public Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

// 1)Title: How To Make 2D/3D Object Float Up and Down In Unity (Coins, Collectibles)
//  Author: Unity Unlocked
//  Date accessed:  09/11/2025
//  Availability: https://www.youtube.com/watch?v=nnDFXmoNBOo