using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [Header("Time Settings")]
    [Range(0, 24)][SerializeField] float TimeofDay = 6f;


    [Header("Sun Settings")]
    [SerializeField] Gradient sunColor;
    [SerializeField] AnimationCurve sunTemperature;
    [SerializeField] AnimationCurve sunIntensity;
    private Light celestialLight;

    [Header("Moon Settings")]
    [SerializeField] Gradient moonColor;


    void Awake()
    {
        celestialLight = GetComponentInChildren<Light>();
    }


}


// Code references:
// Title: Creating a progressive sun skybox
//Author: Deepseek
//Date accessed: 2025/09/26
//Code version:
// Availability: https://chat.deepseek.com/a/chat/s/b4623ae4-829c-4948-91c8-a1ad4705dd51

//I was struggling to visualize how to make a progressive sun skybox. So Ai suggested that I make a SunState class which is basically a cd which gets played.
// When the player completes a mini game and event is sent to change the sun state with a transition.