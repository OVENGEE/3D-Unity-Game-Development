using UnityEngine;

public class ControlsGuide : MonoBehaviour
{
    [SerializeField] Controls[] controls;
}


[System.Serializable]
struct Controls
{
    public string controlName;
    public Sprite keyboardSprite;
    public Sprite playStationSprite;
    public Sprite xboxSprite;
}
