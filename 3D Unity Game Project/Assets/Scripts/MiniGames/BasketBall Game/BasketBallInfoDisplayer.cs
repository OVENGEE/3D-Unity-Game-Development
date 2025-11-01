using UnityEngine;
using TMPro;

public class BasketBallInfoDisplayer : MonoBehaviour
{
    private TextMeshProUGUI displayText;

    void Start()
    {
        displayText = GetComponent<TextMeshProUGUI>();
        displayText.text = "Score 5 goals: (0/5)";
    }

    void OnEnable()
    {

    }
    
    void OnDisable()
    {
        
    }


    void UpdateDisplay(int score)
    {
        displayText.text = $"Score 5 goals: ({score}/5)";
    }

}
