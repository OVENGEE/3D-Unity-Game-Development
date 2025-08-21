using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "PotionData", menuName = "Scriptable Objects/Potion Data")]
public class PotionData : ScriptableObject
{
    public int healAmount;
    public string potionName;
}
