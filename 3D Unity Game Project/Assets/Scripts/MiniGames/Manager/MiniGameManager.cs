using System;
using UnityEngine;
using System;
public class MiniGameManager : MonoBehaviour
{
    public event Action onAnyGameCompleted;

    public void RegisterMiniGame(IGameCompleted miniGame)
    {
        miniGame.OnGameCompleted += HandleMiniGame;
    }

    private void HandleMiniGame()
    {
        onAnyGameCompleted?.Invoke();
    }
}
