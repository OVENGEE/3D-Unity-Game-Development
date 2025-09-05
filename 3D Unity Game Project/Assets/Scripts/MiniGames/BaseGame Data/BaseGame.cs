using UnityEngine;

[CreateAssetMenu(fileName = "BaseGame", menuName = "Scriptable Objects/BaseGame")]
public class BaseGame : ScriptableObject
{
    public string GameName;
    public int RequiredTickets;
}
