using UnityEngine.UI;
using UnityEngine;

public class ControlImageUpdater : MonoBehaviour
{
    private Image img;
    private Color imgcolor;
    void Awake()
    {
        img = GetComponent<Image>();
        imgcolor = img.color;
        imgcolor.a = 0f;
        img.color = imgcolor;
    }


    void Update()
    {
        
    }
}
