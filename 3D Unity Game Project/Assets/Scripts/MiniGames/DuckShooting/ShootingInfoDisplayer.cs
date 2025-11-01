using UnityEngine;
using TMPro;

public class ShootingInfoDisplayer : MonoBehaviour
{
    private TextMeshProUGUI displayText;

    void Start()
    {
        displayText = GetComponent<TextMeshProUGUI>();
        displayText.text = "Shoot 5 targets: (0/5)";
    }

    void OnEnable()
    {
        PlayerShootState.OnTargetShot += UpdateDisplay;
    }
    
    void OnDisable()
    {
        PlayerShootState.OnTargetShot -= UpdateDisplay;
    }


    void UpdateDisplay(int score)
    {
        displayText.text = $"Shoot 5 targets: ({score}/5)";
    }


}
