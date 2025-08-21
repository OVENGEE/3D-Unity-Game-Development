using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] soundEffectGroups[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;
    void Start()
    {
        int CubeCounter = 0;
        var tickets = FindObjectsByType<Ticket>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var _ticket in tickets)
        {
            CubeCounter++;
        }

        Debug.Log("No. of tickets: " + CubeCounter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public struct soundEffectGroups
{
    public string name;
    public List<AudioClip> audioClips;
}