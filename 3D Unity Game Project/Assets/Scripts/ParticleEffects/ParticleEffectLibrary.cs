using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectLibrary : MonoBehaviour
{
    [SerializeField] ParticleEffectGroup[] particleEffectGroups;
    private Dictionary<string, List<ParticleSystem>> particleDictionary;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public struct ParticleEffectGroup
{
    string name;
    List<ParticleSystem> particleSystems;
}


//The particle system logic: came from the sound logic from https://www.youtube.com/watch?v=rAX_r0yBwzQ