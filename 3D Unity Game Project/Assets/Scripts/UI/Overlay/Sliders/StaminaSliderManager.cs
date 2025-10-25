using UnityEngine;
using UnityEngine.UI;

public class StaminaSliderManager : MonoBehaviour
{
    private Slider slider;
    private bool sliderVisible = true;
    public bool SliderVisible
    {
        get => sliderVisible;
        set
        {
            if (sliderVisible != value)
            {
                sliderVisible = value;
            }
        }
    }

    void Awake()
    {
        slider = GetComponent<Slider>();
        ToggleVisibility(false);
    }

    void OnEnable()
    {
        Player.OnSliderChange += SliderValueChange;
    }

    void OnDisable()
    {
        Player.OnSliderChange -= SliderValueChange;
    }

    void SliderValueChange(float value)
    {
        slider.value = value;
        SliderVisible = !Mathf.Approximately(slider.value, 1f);
        ToggleVisibility(sliderVisible);
        
    }

    void ToggleVisibility(bool state)
    {
        Image[] images = GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            image.enabled = state;
        }
    }
}
