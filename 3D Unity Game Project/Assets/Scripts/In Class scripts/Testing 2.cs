using UnityEngine;

public class Testing2 : MonoBehaviour
{
    public PotionData potion;

    void Start()
    {
        Debug.Log("Potion Name:" + potion.name + "Potion amount:" + potion.healAmount);
    }
}
