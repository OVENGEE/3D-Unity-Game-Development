using System;
using UnityEngine;

public class TutorialID : MonoBehaviour
{
    [SerializeField] TutorialType tutorialType;
    public static event Action<TutorialType> OnTutorialTypeTrigger;
    public static event Action<PanelType> OnTutorialTrigger;

    private bool isTriggered;

    private void Awake()
    {
        isTriggered = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            TriggerTutorial();
        }
    }


    public void TriggerTutorial()
    {
        OnTutorialTypeTrigger?.Invoke(tutorialType);
        OnTutorialTrigger?.Invoke(PanelType.Tutorial);
        isTriggered = true;
    }

}
