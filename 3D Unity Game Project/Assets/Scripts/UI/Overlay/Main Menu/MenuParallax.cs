using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public float offsetMultiplier = 1f;
    public float smoothTime = .3f;

    public Vector2 startPosition;
    public Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.position = Vector3.SmoothDamp(transform.position, startPosition + (offset * offsetMultiplier), ref velocity, smoothTime);
    }
}


//Code references
// 1)Title: Unity Gorgeous PARALLAX MAIN MENU in 410 Seconds
//    Author: Raycastly
//    URL:https://www.youtube.com/watch?v=B40xBPXK97A&t=83s
//    Date accessed:29/10/2025