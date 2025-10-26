using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class TutorialCloseGUI : MonoBehaviour
{
    private Image closeImage;
    [SerializeField] private Color highlightColour;
    private void Awake()
    {
        closeImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        closeImage.color = Color.white;
    }

    public void CloseHighlighter()
    {
        closeImage.color = highlightColour;
    }

    public void closeResetColour()
    {
        closeImage.color = Color.white;
    }



}
