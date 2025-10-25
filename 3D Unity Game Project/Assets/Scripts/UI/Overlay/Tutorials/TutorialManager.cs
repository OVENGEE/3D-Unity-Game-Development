using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private RawImage tutorialScreen;

    private void Awake()
    {
        tutorialScreen = GetComponentInChildren<RawImage>();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        
    }
}
