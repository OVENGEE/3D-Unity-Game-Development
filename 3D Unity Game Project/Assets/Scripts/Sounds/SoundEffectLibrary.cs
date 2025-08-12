using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] soundEffectGroups[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public struct soundEffectGroups
{
    string name;
    List<AudioClip> audioClips;
}