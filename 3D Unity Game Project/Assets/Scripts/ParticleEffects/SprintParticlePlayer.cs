using UnityEngine;

public class SprintParticlePlayer : MonoBehaviour
{
    private ParticleSystem particle;

    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    void OnEnable()
    {
        PlayerSprintState.OnSprintEffectStarted += PlayEffect;
        PlayerSprintState.OnSprintEffectEnded += StopEffect; 
    }

    void OnDisable()
    {
        PlayerSprintState.OnSprintEffectStarted -= PlayEffect;
        PlayerSprintState.OnSprintEffectEnded -= StopEffect;
    }



    void PlayEffect()
    {
        particle.Play();
    }


    void StopEffect()
    {
        particle.Stop();
    }
}
