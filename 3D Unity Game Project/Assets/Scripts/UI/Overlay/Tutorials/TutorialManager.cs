using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Windows.WebCam;

public class TutorialManager : MonoBehaviour
{
    //Tutorial declarations
    public TutorialLibrary[] tutorialLibraries;
    private Dictionary<TutorialType, TutorialLibrary> tutorialMap;

    private TutorialLibrary currentTutorial;

    //Video declarations
    private RawImage tutorialScreen;
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        tutorialScreen = GetComponentInChildren<RawImage>();
        videoPlayer = tutorialScreen.GetComponent<VideoPlayer>();

        ////Build the dictionary for quick access
        tutorialMap = new Dictionary<TutorialType, TutorialLibrary>();
        foreach (var tutorialInfo in tutorialLibraries)
        {
            if (!tutorialMap.ContainsKey(tutorialInfo.tutorialName))
                tutorialMap.Add(tutorialInfo.tutorialName, tutorialInfo);
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
        videoPlayer.clip = clip;
        videoPlayer.Play();
    }

    private void TutorialPlay(TutorialType tutorial)
    {
        currentTutorial = tutorialMap[tutorial];
        if (!currentTutorial.played)
        {
            var currentTutClip = currentTutorial.tutorialClip;
            PlayVideo(currentTutClip);           
        }

    }
    
    public void CloseVideo()
    {
        currentTutorial.played = true;
    }
}

public enum TutorialType
{
    Move,
    Sprint,
    Crouch,
    BasketBallTut,
    ShootingTut
}

[System.Serializable]
public struct TutorialLibrary
{
    public TutorialType tutorialName;
    public VideoClip tutorialClip;
    public string tutorialDescription;
    public bool played;
}