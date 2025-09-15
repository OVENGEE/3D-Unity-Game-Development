using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] soundEffectGroups[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;
    void Awake()
    {

    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach (var soundEffectGroup in soundEffectGroups)
        {
            soundDictionary[soundEffectGroup.name] = soundEffectGroup.audioClips;
        }
    }

    // public AudioClip GetRandomClip(string name)
    // {
    //     if (soundDictionary.ContainsKey(name))
    //     {
    //         var audioClips = soundDictionary[name];
    //         if (audioClips.Count > 0)
    //         {

    //         }
    //     }
    // }
}


[System.Serializable]
public struct soundEffectGroups
{
    public string name;
    public List<AudioClip> audioClips;
}


//  Code references:
// 1) Title:Add Sound Effects to Your Game! - Top Down Unity 2D #18
//    Author:GameCode Library
//    Date: 19/08/2025
//    Availiability: https://www.youtube.com/watch?v=AaudFyM3KV0 (wrong link)