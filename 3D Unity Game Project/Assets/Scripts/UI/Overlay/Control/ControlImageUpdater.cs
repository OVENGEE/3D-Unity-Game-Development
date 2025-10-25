using UnityEngine.UI;
using UnityEngine;

public class ControlImageUpdater : MonoBehaviour
{
    private Image img;
    private Sprite currentSprite;
    private Color imgcolor;
    void Awake()
    {
        img = GetComponent<Image>();
        imgcolor = img.color;
        imgcolor.a = 0f;
        img.color = imgcolor;
    }


    void DisplaySprite(Sprite sprite)
    {
        if (sprite == null)
        {
            imgcolor.a = 0f;
            img.color = imgcolor;
            return;
        }

        imgcolor.a = 1f;
        img.color = imgcolor;
        img.sprite = sprite;
        currentSprite = sprite;
    }

    void OnEnable()
    {
        ControlsGuide.OnControlImageChange += DisplaySprite;
    }

    void OnDisable()
    {
        ControlsGuide.OnControlImageChange -= DisplaySprite;
    }
}
