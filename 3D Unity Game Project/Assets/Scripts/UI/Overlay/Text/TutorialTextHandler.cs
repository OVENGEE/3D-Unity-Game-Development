using UnityEngine;
using TMPro;

public class TutorialTextHandler : MonoBehaviour
{
    [SerializeField] GameObject tutorialTextObj;
    TextMeshProUGUI tutorialText;
    void Start()
    {
        if (tutorialTextObj == null)
        {
            Debug.LogError("tutorialTextObj is not assigned in TutorialTextHandler");
        }
        tutorialText = tutorialTextObj?.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        TutorialMessageDisplayer.OnTutorialUIUpdate += UpdateState;
    }

    private void OnDisable()
    {
        TutorialMessageDisplayer.OnTutorialUIUpdate -= UpdateState;
    }

    void UpdateState(string TutorialMessage)
    {
        tutorialText.text = TutorialMessage;
        Debug.Log("TutorialText is being updated");
    }
}
