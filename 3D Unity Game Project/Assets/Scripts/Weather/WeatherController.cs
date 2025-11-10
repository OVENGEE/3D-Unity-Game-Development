using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{

    Light celestialLight;
    MiniGameManager gameManager;

    [SerializeField] TimePhase[] timePhases;
    [SerializeField] Cubemap initialSkyboxCubemap;
    private Dictionary<SunPhase, TimePhase> timePhaseMap;

    private SunPhase currentPhase;
    private TimePhase previousPhase;
    private TimePhase targetPhase;

    private float transitionTimer;
    private float transitionDuration = 3f;
    private bool isTransitioning;


    void Awake()
    {
        celestialLight = GetComponentInChildren<Light>();
        gameManager = FindAnyObjectByType<MiniGameManager>(FindObjectsInactive.Include);
        currentPhase = SunPhase.None;

        if (initialSkyboxCubemap == null)
        {
            // Debug.LogError("cubemap not assigned in inspector");
            return;
        }

        RenderSettings.skybox.SetTexture("_Tex1", initialSkyboxCubemap);
        RenderSettings.skybox.SetTexture("_Tex2", initialSkyboxCubemap);
        RenderSettings.skybox.SetFloat("_Blend", 0f);

        timePhaseMap = new Dictionary<SunPhase, TimePhase>();
        foreach (var timePhase in timePhases)
        {
            if (!timePhaseMap.ContainsKey(timePhase.sunType))
                timePhaseMap.Add(timePhase.sunType, timePhase);
        }

        // Capture the current lighting as baseline
        previousPhase = new TimePhase
        {
            sunType = SunPhase.None,
            AmbientColour = MakeSolidGradient(RenderSettings.ambientLight),
            FogColour = MakeSolidGradient(RenderSettings.fogColor),
            DirectionalColour = MakeSolidGradient(celestialLight.color),
            DirectionalLightRotation = celestialLight.transform.rotation.eulerAngles,
            SkyboxCubemap = initialSkyboxCubemap
        };
    }

    private void Update()
    {
        if (isTransitioning)
            TransitionLighting();
    }


    void OnEnable()
    {
        LightTrigger.OnLightTrigger += UpdatePhase;
    }

    void OnDisable()
    {
        LightTrigger.OnLightTrigger -= UpdatePhase;
    }

    private void UpdatePhase()
    {
        switch (currentPhase)
        {
            case SunPhase.None:
                currentPhase = SunPhase.SunSet;
                break;
            case SunPhase.SunSet:
                currentPhase = SunPhase.Night;
                break;
        }

        BeginTransition(currentPhase);
    }

    private void BeginTransition(SunPhase newPhase)
    {
        if (!timePhaseMap.TryGetValue(newPhase, out targetPhase))
            return;

        // start transition
        transitionTimer = 0f;
        isTransitioning = true;
        currentPhase = newPhase;
        RenderSettings.skybox.SetTexture("_Tex1", previousPhase.SkyboxCubemap);
        RenderSettings.skybox.SetTexture("_Tex2", targetPhase.SkyboxCubemap);
    }

    private void TransitionLighting()
    {
        transitionTimer += Time.deltaTime;
        float t = Mathf.SmoothStep(0f, 1f, transitionTimer / transitionDuration);

        Color ambientStart = previousPhase.AmbientColour.Evaluate(0.5f);
        Color ambientEnd = targetPhase.AmbientColour.Evaluate(0.5f);

        Color fogStart = previousPhase.FogColour.Evaluate(0.5f);
        Color fogEnd = targetPhase.FogColour.Evaluate(0.5f);

        Color lightStart = previousPhase.DirectionalColour.Evaluate(0.5f);
        Color lightEnd = targetPhase.DirectionalColour.Evaluate(0.5f);

        RenderSettings.ambientLight = Color.Lerp(ambientStart, ambientEnd, t);
        RenderSettings.fogColor = Color.Lerp(fogStart, fogEnd, t);
        celestialLight.color = Color.Lerp(lightStart, lightEnd, t);

        Quaternion startRot = Quaternion.Euler(previousPhase.DirectionalLightRotation);
        Quaternion endRot = Quaternion.Euler(targetPhase.DirectionalLightRotation);
        celestialLight.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
        RenderSettings.skybox.SetFloat("_Blend", t);


        if (t >= 1f)
        {
            isTransitioning = false;

            // lock into the final skybox (fully target)
            RenderSettings.skybox.SetFloat("_Blend", 1f);

            // target becomes new baseline
            previousPhase = targetPhase;

            // and this becomes the new starting cubemap for next transition
            RenderSettings.skybox.SetTexture("_Tex1", previousPhase.SkyboxCubemap);
        }
    }
    
    // helper function to make a single-color gradient
    private Gradient MakeSolidGradient(Color c)
    {
        Gradient g = new Gradient();
        g.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(c, 0f),
                new GradientColorKey(c, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(c.a, 0f),
                new GradientAlphaKey(c.a, 1f)
            }
        );
        return g;
    }

}


[System.Serializable]
public struct TimePhase
{
    public SunPhase sunType;
    public Gradient AmbientColour;
    public Gradient DirectionalColour;
    public Gradient FogColour;
    public Vector3 DirectionalLightRotation;
    public Cubemap SkyboxCubemap;
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

// Title: Sunset and nightTime gradient
//Author: Chatgpt
//Date accessed: 2025/11/09
//Code version:
// Availability: https://chatgpt.com/c/6910d10b-945c-8326-9810-25ede1c33596

// This gave me the gradients and vectors required to make a nice day night cycle