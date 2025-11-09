using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] Animator animator;


    void Awake()
    {
        if (animator == null)
        {
            Debug.LogError("the animator was not assigned in inspector");
            return;
        }
    }

    public void PlayAnimation(AnimationData data)
    {
        string stateName = GetStateName(data.type);

        if (data.useTrigger)
        {
            animator.SetTrigger(stateName);
        }
        else
        {
            animator.CrossFadeInFixedTime(stateName, data.fadeDuration);
        }

        StartCoroutine(FadeLayer(data.layer, data.targetWeight, data.fadeDuration));
    }

    private IEnumerator FadeLayer(int layer, float target, float duration)
    {
        float start = animator.GetLayerWeight(layer);
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            animator.SetLayerWeight(layer, Mathf.Lerp(start, target, t / duration));
            yield return null;
        }
    }

    private string GetStateName(AnimationType type)
    {
        switch (type)
        {
            case AnimationType.Idle: return "Idle";
            case AnimationType.Walk: return "Walk";
            case AnimationType.Run: return "Run";
            case AnimationType.HoldBall: return "HoldBall";
            case AnimationType.HoldGun: return "HoldGun";
            default: return "";
        }
    }

}

[System.Serializable]
public struct AnimationData
{
    public AnimationType type;
    public int layer;

    public float fadeDuration;
    public float targetWeight;
    public bool useTrigger;
}


public enum AnimationType
{
    Idle,
    Walk,
    Run,
    HoldBall,
    HoldGun

}

// Code references:
// 1)Title: Scalable animation manager
//  Author: Chatgpt
//  Date accessed:  09/11/2025
//  Availability: https://chatgpt.com/c/690211cc-42fc-832a-8f37-f47fd4e44749

//This helped me realize information I was missing to layer my animations correctly