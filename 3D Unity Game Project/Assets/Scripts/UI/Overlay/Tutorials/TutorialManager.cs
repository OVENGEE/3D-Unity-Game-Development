using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Windows.WebCam;

public class TutorialManager : MonoBehaviour
{
    //Video declarations
    [Header("Video Requirements")]
    [SerializeField]private RawImage tutorialScreen;
    private VideoPlayer videoPlayer;
    //Tutorial declarations
    [Header("Tutorial Library")]
    public TutorialLibrary[] tutorialLibraries;
    private Dictionary<TutorialType, TutorialLibrary> tutorialMap;

    private TutorialLibrary currentTutorial;



    private void Awake()
    {
        if (tutorialScreen == null)
        {
            Debug.LogError("TutorialManager: no RawImage found in scene.");
            return;
        }

        videoPlayer = tutorialScreen?.GetComponent<VideoPlayer>();

        ////Build the dictionary for quick access
        tutorialMap = new Dictionary<TutorialType, TutorialLibrary>();
        foreach (var tutorialInfo in tutorialLibraries)
        {
            if (!tutorialMap.ContainsKey(tutorialInfo.tutorialName))
            {
                tutorialMap.Add(tutorialInfo.tutorialName, tutorialInfo);
                Debug.Log($"{tutorialInfo.tutorialName} tutorial added");
            }
                

        }
    }

    private void OnEnable()
    {
        TutorialID.OnTutorialTypeTrigger += TutorialPlay;
    }

    private void OnDisable()
    {
        TutorialID.OnTutorialTypeTrigger -= TutorialPlay;
    }

    private void PlayVideo(VideoClip clip)
    {
        Debug.Log($"{clip} is being played");
        videoPlayer.clip = clip;
        videoPlayer.Play();
    }

    private void TutorialPlay(TutorialType tutorial)
    {
        currentTutorial = tutorialMap[tutorial];
        // Debug.Log($"Current tutorial: {tutorial}");
        if (!currentTutorial.played)
        {
            var currentTutClip = currentTutorial.tutorialClip;
            PlayVideo(currentTutClip);
        }

    }
    
    private void TutorialMessage(string message)
    {
        currentTutorial.tutorialDescription = message;
        //Add the text to be updated
    }
    
    public void CloseVideo()
    {
        currentTutorial.played = true;
    }
}

public enum TutorialType
{
    Sprint,
    Crouch,
    BasketBallTut,
    ShootingTut,
    Jump
}

[System.Serializable]
public struct TutorialLibrary
{
    public TutorialType tutorialName;
    public VideoClip tutorialClip;
    public string tutorialDescription;
    public bool played;
}