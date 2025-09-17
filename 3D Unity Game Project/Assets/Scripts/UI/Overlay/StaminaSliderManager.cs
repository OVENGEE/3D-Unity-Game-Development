using UnityEngine;
using UnityEngine.UI;

public class StaminaSliderManager : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
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
    }

    void SliderDissapear()
    {
        
    }
}
