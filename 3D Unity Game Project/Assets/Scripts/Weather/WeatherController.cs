using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{

    Light celestialLight;
    MiniGameManager gameManager;

    [SerializeField] TimePhase[] timePhases;
    private Dictionary<SunPhase, TimePhase> timePhaseMap;
    private SunPhase currentPhase;

    void Awake()
    {
        celestialLight = GetComponentInChildren<Light>();
        gameManager = FindAnyObjectByType<MiniGameManager>(FindObjectsInactive.Include);
        currentPhase = SunPhase.None;

        timePhaseMap = new Dictionary<SunPhase, TimePhase>();
        foreach (var timePhase in timePhases)
        {
            if (!timePhaseMap.ContainsKey(timePhase.sunType))
                timePhaseMap.Add(timePhase.sunType, timePhase);
        }
    }



    void OnEnable()
    {

    }

    void OnDisable()
    {

    }



}


[System.Serializable]
public struct TimePhase
{
    public SunPhase sunType;
    public Gradient AmbientColour;
    public Gradient DirectionalColour;
    public Gradient FogColour;
}

public enum SunPhase
{
    None,
    SunSet,
    Night
}


// Code references:
// Title: Creating a progressive sun skybox
//Author: Deepseek
//Date accessed: 2025/09/26
//Code version:
// Availability: https://chat.deepseek.com/a/chat/s/b4623ae4-829c-4948-91c8-a1ad4705dd51

//I was struggling to visualize how to make a progressive sun skybox. So Ai suggested that I make a SunState class which is basically a cd which gets played.
// When the player completes a mini game and event is sent to change the sun state with a transition.