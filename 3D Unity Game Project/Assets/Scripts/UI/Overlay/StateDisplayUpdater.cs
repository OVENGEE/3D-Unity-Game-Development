using UnityEngine;
using TMPro;

public class StateDisplayUpdater : MonoBehaviour
{
    TextMeshProUGUI stateText;

    void Start()
    {
        stateText = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        Player.OnPlayerStateChange += UpdateState;
    }

    void OnDisable()
    {
        Player.OnPlayerStateChange -= UpdateState;
    }

    void UpdateState(Player.PlayerState playerstate)
    {
        stateText.text = playerstate.ToString();
    }
}
